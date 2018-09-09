/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public static class Serializer
{
	public enum State
	{
		Idle = 0,
		Serializing,
		Deserializing
	}

	public static State CurrentState = State.Idle;

	public struct UndoState
	{
		public enum DataType
		{
			Unknown = 0,
			Json,
			Binary
		}

		public byte[] BlockState;
		public DataType Type;
		public Vector3 PlayerPosition;
		public Vector3 PlayerFacingDirection;
		public int PlayerStateIndex;
		public float PlatformYPosition;
		public int[] PlayerScores;
	}

	public static int MaxUndoLevels = 99;
	private static List<UndoState> _undoStates = new List<UndoState>();
	private static List<UndoState> _redoStates = new List<UndoState>();
	private static byte[] _startState = null;
	private static string _lastCheckpointState;

	private static bool _undoEnabled = true;
	private static bool _redoEnabled = false;
	public static bool UndoEnabled
	{
		get
		{
			return _undoEnabled;
		}
		set
		{
			_undoEnabled = value;
			if (value)
				LogController.Log(MaxUndoLevels + " Undos On");
			else
				LogController.Log("Undo off");
		}
	}

	public static bool RedoEnabled
	{
		get
		{
			return (UndoEnabled && _redoEnabled);
		}
		set
		{
			_redoEnabled = value;
			if (value)
				LogController.Log(MaxUndoLevels + " Redos On");
			else
				LogController.Log("Redo off");
		}
	}

	/// <summary>
	/// Workaround for https://issuetracker.unity3d.com/issues/system-dot-configuration-dot-configurationerrorsexception-failed-to-load-configuration-section-for-datacontractserializer
	/// </summary>
	public static bool UseBinaryFiles
	{
		get
		{
			return Application.platform == RuntimePlatform.WSAPlayerX64 
				|| Application.platform == RuntimePlatform.WSAPlayerX86
				|| Application.platform == RuntimePlatform.WSAPlayerARM
				|| Application.platform == RuntimePlatform.OSXPlayer
				|| Application.platform == RuntimePlatform.OSXEditor
				|| Application.platform == RuntimePlatform.LinuxEditor
				|| Application.platform == RuntimePlatform.LinuxPlayer
				|| Application.platform == RuntimePlatform.WebGLPlayer
				|| UIManager.Instance.BinaryFormat.isOn;
		}
	}

	/// <summary>
	/// Used for auto-saving and saving while creating stages as
	/// well as reloading a level from a file.
	/// </summary>
	public static string LastAccessedFileName;

	/// <summary>
	/// When turned on, BlockManager will record BlockManager.MaxUndoLevels worth of states.
	/// Note that this is the same as setting the BlockManager.UndoEnabled property to its
	/// inverse.
	/// </summary>
	/// code to another class
	public static void ToggleUndoEnabled()
	{
		UndoEnabled = !UndoEnabled;
	}

	public static void ToggleRedoEnabled()
	{
		RedoEnabled = !RedoEnabled;
	}

	public static void ClearUndoStates()
	{
		_undoStates.Clear();
	}

	public static bool HasStartState()
	{
		return _startState != null;
	}

	public static void ClearStartState()
	{
		_startState = null;
	}

	public static void RecordStartState()
	{
		_startState = BlocksToBinaryStream();
	}

	public static void ReloadStartState()
	{
		if (_startState != null)
		{
			BlockManager.Clear();
			BlocksFromBinaryStream(_startState);
		}
	}

	public static void RecordUndo(bool clearRedo = true)
	{
		if (!UndoEnabled)
			return;
		if (_undoStates.Count > MaxUndoLevels)
		{
			_undoStates.RemoveAt(0);
		}
		_undoStates.Add(CurrentUndoState());
		if (clearRedo)
			_redoStates.Clear();
	}

	public static void DeleteLastUndo()
	{
		if (_undoStates.Count > 0)
		{
			_undoStates.RemoveAt(_undoStates.Count - 1);
		}
	}

	public static void RecordRedo()
	{
		if (!RedoEnabled)
			return;
		if (_redoStates.Count > MaxUndoLevels)
		{
			_redoStates.RemoveAt(0);
		}
		_redoStates.Add(CurrentUndoState());
	}

	private static UndoState CurrentUndoState()
	{
		try
		{
			UndoState state = new UndoState
			{
				//TODO support binary
				BlockState = BlocksToBinaryStream(),
				Type = UndoState.DataType.Binary,
				PlayerPosition = PlayerManager.Player1Location(),
				PlayerFacingDirection = PlayerManager.Player1FacingDirection(),
				PlayerStateIndex = PlayerManager.PlayerState(),
				PlatformYPosition = BlockManager.ActiveFloor.transform.position.y,
				PlayerScores = new int[GameManager.PlayerScores.Length]
			};
			Array.Copy(GameManager.PlayerScores, state.PlayerScores, GameManager.PlayerScores.Length);
			return state;
		}
		catch (Exception e)
		{
		   LogController.Log(e.Message);
		}
		return default(UndoState);
	}

	public static int AvailableUndoCount { get { if (!UndoEnabled) return 0; return _undoStates.Count; } }
	public static int AvailableRedoCount { get { if (!UndoEnabled) return 0; return _redoStates.Count; } }

	public static void Undo()
	{
		if (!UndoEnabled)
			return;
		if (_undoStates.Count > 0)
		{
			LogController.Log("Undo Started");
			RecordRedo();
			UndoState state = _undoStates[_undoStates.Count - 1];
			RestoreUndoState(state);
			_undoStates.RemoveAt(_undoStates.Count - 1);
			LogController.Log("Undo Complete");
		}
		else
			LogController.Log("No undos left");
	}

	//TODO
	public static void Redo()
	{
		if (!UndoEnabled)
			return;
		if (!RedoEnabled)
		{
			RedoEnabled = true;
			return;
		}

		if (_redoStates.Count > 0)
		{
			RecordUndo(false);
			UndoState state = _redoStates[_redoStates.Count - 1];
			RestoreUndoState(state);
			_redoStates.RemoveAt(_redoStates.Count - 1);
			LogController.Log("Redo");
		}
		else
			LogController.Log("No redos left");
	}

	private static void RestoreUndoState(UndoState state)
	{
		BlockManager.ClearForUndo();
		PlayerManager.HideAllPlayers();

		BlockManager.ActiveFloor.transform.position = new Vector3(0f, state.PlatformYPosition, 0f);
		if (state.Type == UndoState.DataType.Json)
			BlocksFromJsonStream(state.BlockState);
		else if (state.Type == UndoState.DataType.Binary)
			BlocksFromBinaryStream(state.BlockState);
		GameManager.PlayerScores = state.PlayerScores;
		BlockManager.Instance.StartCoroutine(RestoreUndoHelper(state));
	}

	/// <summary>
	/// Used to set the player location, facing direction, etc after an undo.
	/// Needed because the player spawn is part of the player spawn item and
	/// happens automatically. This will try to restore the player every
	/// frame for 420 frames before giving up.
	/// </summary>
	/// <param name="state"></param>
	/// <returns></returns>
	private static IEnumerator RestoreUndoHelper(UndoState state)
	{
		int frameCount = 420;
		IPlayerCharacter player = PlayerManager.Player(0);
		while (player == null && --frameCount > 0)
		{
			yield return new WaitForEndOfFrame();
			player = PlayerManager.Player(0);
		}
		if (player == null)
		{
			LogController.Log("Could not restore player");
		}
		PlayerManager.SetPlayer1Location(state.PlayerPosition);
		PlayerManager.SetPlayer1FacingDirection(state.PlayerFacingDirection);
		PlayerManager.SetPlayer1State(state.PlayerStateIndex);
		PlayerManager.ShowAllPlayers();
		yield return null;
	}


	public static string BlocksToPrettyJson()
	{
		CurrentState = State.Serializing;
		string output = "";
		StageJsonDelegate stage = new StageJsonDelegate(BlockManager.Instance);
		StageCollection collection = new StageCollection(stage);
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

		try
		{
			MemoryStream ms = new MemoryStream();
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(StageCollection));
			XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, true, "    ");
			serializer.WriteObject(writer, collection);
			writer.Flush();
			output += Encoding.UTF8.GetString(ms.ToArray());
		}
		catch (System.Exception exception)
		{
			LogController.Log(exception.ToString());
		}
		finally
		{
			CurrentState = State.Idle;
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
		CurrentState = State.Idle;
		return output;
	}

	public static string BlocksToCondensedJson()
	{
		CurrentState = State.Serializing;
		string output = "";
		StageJsonDelegate stage = new StageJsonDelegate(BlockManager.Instance);
		StageCollection collection = new StageCollection(stage);
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

		try
		{
			MemoryStream ms = new MemoryStream();
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(StageCollection));
			XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, false);
			serializer.WriteObject(writer, collection);
			writer.Flush();
			output += Encoding.UTF8.GetString(ms.ToArray());
		}
		catch (System.Exception exception)
		{
			LogController.Log(exception.ToString());
		}
		finally
		{
			CurrentState = State.Idle;
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
		CurrentState = State.Idle;
		return output;
	}

	public static byte[] BlocksToCondensedJsonStream()
	{
		CurrentState = State.Serializing;
		StageJsonDelegate stage = new StageJsonDelegate(BlockManager.Instance);
		StageCollection collection = new StageCollection(stage);
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

		try
		{
			MemoryStream ms = new MemoryStream();
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(StageCollection));
			XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, true, false);
			serializer.WriteObject(writer, collection);
			writer.Flush();
			return ms.ToArray();
		}
		catch (System.Exception exception)
		{
			LogController.Log(exception.ToString());
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
			CurrentState = State.Idle;
		}
		CurrentState = State.Idle;
		return null;
	}

	public static byte[] BlocksToBinaryStream()
	{
		CurrentState = State.Serializing;
		StageBinaryDelegate stage = new StageBinaryDelegate();
		StageCollectionBinaryDelegate collection = new StageCollectionBinaryDelegate(stage);

		try
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(ms, collection);
			ms.Close();
			return ms.ToArray();
		}
		catch (System.Exception exception)
		{
			CurrentState = State.Idle;
			LogController.Log(exception.ToString());
		}
		CurrentState = State.Idle;
		return null;
	}

	public static void QuickSave()
	{
		if (string.IsNullOrWhiteSpace(LastAccessedFileName))
			UIManager.SaveToJson();
		else
			SaveFileUsingPath(LastAccessedFileName);
	}

	public static void AutoSave()
	{
		if (CurrentState == State.Idle && !BlockManager.PlayMode && !string.IsNullOrWhiteSpace(LastAccessedFileName) && PlayerPrefs.GetInt("DestructivePlayMode", 0) != 1)
		{
			SaveFileUsingPath(LastAccessedFileName.Replace(".json", "_autosave.json"));
			LogController.Log("Autosaved");
		}
	}


	public static void BlocksFromJson(Uri path, bool startPlayMode = false, string[] startPositionOverrides = null)
	{
		CurrentState = State.Deserializing;
		StageCollection deserializedCollection = new StageCollection(BlockManager.Instance);
		if (Application.platform != RuntimePlatform.Android)
		{
			LogController.Log("Loading from " + path.ToString());
			WebClient webClient = new WebClient();
			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
			Stream fs = webClient.OpenRead(path);
			HandleLoad(fs, true);
		}
		else
		{
			LogController.Log("Loading from " + path.Host + path.PathAndQuery + path.Fragment);
			Stream fs = File.OpenRead(path.Host + path.PathAndQuery + path.Fragment);
			HandleLoad(fs, true);
		}
		if(startPositionOverrides != null)
		{
			for(int i=0;i<startPositionOverrides.Length;++i)
			{
				IBlock startBlock = null;
				foreach(IBlock block in BlockManager.BlockCache)
				{
					if (block.Name == startPositionOverrides[i])
						startBlock = block;
				}
				if (startBlock != null)
					ItemManager.CreateItemAt(startBlock.Position, "Cat5", "Player Start");
			}
		}
		BlockManager.ResetCursor();
		RecordStartState();
		if (startPlayMode || PlayerPrefs.GetInt("AutoPlayOnLoad", 1) == 1)
		{
			if (!BlockManager.PlayMode)
				BlockManager.Instance.TogglePlayMode(0.25f);
		}
		CurrentState = State.Idle;
	}

	public static void HandleLoad(Stream stream, bool clearFirst = true)
	{
		UIManager.ShowNetworkStatus("Setting the stage...", false);
		CurrentState = State.Deserializing;
		if (clearFirst)
			BlockManager.Clear();
		StageCollection deserializedCollection = new StageCollection(BlockManager.Instance);
		DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedCollection.GetType());
		deserializedCollection = ser.ReadObject(stream) as StageCollection;
		stream.Close();
		BlockManager.ResetCursor();
		LogController.Log("Loaded " + deserializedCollection.Stages.Count + " stage(s)");
		CurrentState = State.Idle;
		UIManager.HideNetworkStatus();
	}

	public static void HandleBinaryLoad(Stream stream, bool clearFirst = true)
	{
		CurrentState = State.Deserializing;
		if (clearFirst)
			BlockManager.Clear();
		BinaryFormatter formatter = new BinaryFormatter();
		StageCollectionBinaryDelegate deserializedCollection = formatter.Deserialize(stream) as StageCollectionBinaryDelegate;
		BlockManager.ResetCursor();
		stream.Close();
		CurrentState = State.Idle;
	}


	public static void BlocksFromJson(string json)
	{
		MemoryStream stream = new MemoryStream();
		StreamWriter writer = new StreamWriter(stream);
		writer.Write(json);
		writer.Flush();
		stream.Position = 0;
		HandleLoad(stream, false);
	}

	public static void BlocksFromJsonStream(byte[] bytes, bool clearFirst = false)
	{
		CurrentState = State.Deserializing;
		MemoryStream stream = new MemoryStream(bytes);
		stream.Position = 0;
		HandleLoad(stream, clearFirst);
		CurrentState = State.Idle;
	}

	public static void BlocksFromBinaryStream(byte[] bytes, bool clearFirst = false)
	{
		CurrentState = State.Deserializing;
		MemoryStream stream = new MemoryStream(bytes);
		stream.Position = 0;
		HandleBinaryLoad(stream, clearFirst);
		CurrentState = State.Idle;
	}

	// Saves a file with the textToSave using a path
	public static void SaveFileUsingPath(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			Uri location = new Uri("file:///" + path);
			string directory = System.IO.Path.GetDirectoryName(location.AbsolutePath);
			PlayerPrefs.SetString("LastSaveDir", directory);

			if (UseBinaryFiles)
			{
				byte[] data = BlocksToBinaryStream();
				//TODO this probably can throw an exception?
				if (data != null && data.Length > 0)
					System.IO.File.WriteAllBytes(path, data);
				if (!path.Contains("_autosave."))
				{
					LastAccessedFileName = path;
					File.Delete(path.Replace(".bin", "_autosave.bin"));
					LogController.Log("Saved & Autosave");
				}
			}
			else
			{
				string json = BlocksToPrettyJson();
				//TODO this probably can throw an exception?
				if (!string.IsNullOrWhiteSpace(json))
					System.IO.File.WriteAllText(path, json);
				if (!path.Contains("_autosave."))
				{
					LastAccessedFileName = path;
					File.Delete(path.Replace(".json", "_autosave.json"));
					LogController.Log("Saved & Autosave");
				}
			}
		}
		else
		{
			LogController.Log("Invalid path");
		}
	}

	public static void LoadFileUsingHTTP(Uri path)
	{
		//TODO ensure file is valid
		if (path.IsAbsoluteUri && (path.Scheme == Uri.UriSchemeHttp || path.Scheme == Uri.UriSchemeHttps))
		{
			BlocksFromJson(path);
		}
		else
		{
			LogController.Log("Invalid path");
		}
	}

	// Loads a file using a path
	public static void LoadFileUsingLocalPath(string path)
	{
		//TODO ensure file is valid
		if (!string.IsNullOrWhiteSpace(path))
		{
			Uri location = new Uri((Application.platform == RuntimePlatform.WebGLPlayer ? "" : "file:///") + path);
			string directory;
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			{
				int index = path.LastIndexOf("/");
				if (index > 0)
					directory = path.Substring(0, index + 1);
				else
					directory = "";
			}
			else if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				directory = location.ToString();
				int index = directory.LastIndexOf("/");
				if (index > 0)
					directory = directory.Substring(0, index + 1);
				else
					directory = "";

				PlayerPrefs.SetString("LastLoadDir", directory);
				BlockManager.Instance.StartCoroutine(LoadBinaryNetworkHelper(location));
				return;
			}
			else
				directory = System.IO.Path.GetDirectoryName(location.AbsolutePath);


			PlayerPrefs.SetString("LastLoadDir", directory);

			if (UseBinaryFiles)
			{
				if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer)
				{
					if(BetterStreamingAssets.FileExists(path))
						BlocksFromBinaryStream(BetterStreamingAssets.ReadAllBytes(path),true);
					else
						BlocksFromBinaryStream(File.ReadAllBytes(path),true);
				}
				else
					BlocksFromBinaryStream(File.ReadAllBytes(path),true);

				if (PlayerPrefs.GetInt("AutoPlayOnLoad", 1) == 1)
				{
					if (!BlockManager.PlayMode)
						BlockManager.Instance.TogglePlayMode(0.25f);
				}
				CurrentState = State.Idle;
			}
			else
			{
				if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer)
				{
					if (BetterStreamingAssets.FileExists(path))
						BlocksFromJsonStream(BetterStreamingAssets.ReadAllBytes(path),true);
					else
						BlocksFromJsonStream(File.ReadAllBytes(path),true);

					if (PlayerPrefs.GetInt("AutoPlayOnLoad", 1) == 1)
					{
						if (!BlockManager.PlayMode)
							BlockManager.Instance.TogglePlayMode(0.25f);
					}
					CurrentState = State.Idle;
				}
				else
					BlocksFromJson(location);
			}
			if (path.Contains("_autosave"))
			{
				LastAccessedFileName = string.Empty;
				LogController.Log("AUTOSAVE OFF");
			}
			else
				LastAccessedFileName = path;
		}
		else
		{
			LogController.Log("Invalid path");
		}
		BlockManager.ResetCursor();
		GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true);
	}

	public static void SaveToPastebin()
	{
		BlockManager.Instance.StartCoroutine(SaveToPastebinHelper());
	}

	public static void LoadFromPastebin(string key)
	{
		BlockManager.Instance.StartCoroutine(LoadFromPastebinHelper(key));
	}

	private static IEnumerator SaveToPastebinHelper()
	{
		UIManager.ShowNetworkStatus("Sharing to glot.io...", false);

		string json = BlocksToCondensedJson();

		json = json.Replace("\"", "\\\"");

		string filename = null;
		if (LastAccessedFileName != null)
		{
			filename = LastAccessedFileName.Replace("_autosave", "").Replace(".bin", ".json").Replace(".json", "");
			if (filename.Contains("\\"))
				filename = filename.Substring(filename.LastIndexOf("\\") + 1);

			if (filename.Contains("/"))
				filename = filename.Substring(filename.LastIndexOf("/") + 1);
		}
		if (string.IsNullOrWhiteSpace(filename))
			filename = "Anonymous/Untitled";

		string data = "{\"language\": \"json\", \"title\": \""+filename+"\", \"public\": true, \"files\": [{\"name\": \""+filename+".json\", \"content\": \""+json+"\"}]}";

		using (UnityWebRequest www = new UnityWebRequest("https://snippets.glot.io/snippets", "POST"))
		{
			www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
			www.downloadHandler = new DownloadHandlerBuffer();
			//TODO regenerate this and make it not in the public repository somehow
			www.SetRequestHeader("Authorization", "Token 3832e929-129f-4ede-b5f8-9a4afab29681");
			www.SetRequestHeader("Content-Type", "application/json");
			www.chunkedTransfer = false;
			yield return www.SendWebRequest();

			if (www.isNetworkError)
			{
				Debug.Log("Network Error:" + www.error);
				Debug.Log(www.downloadHandler.text);
				UIManager.ShowNetworkStatus("Share failed", true);
			}
			else if (www.isHttpError)
			{
				Debug.Log("HTTP Error:" + www.error);
				Debug.Log(www.downloadHandler.text);
				UIManager.ShowNetworkStatus("Share failed", true);
			}
			else
			{

				// Or retrieve results as binary data
				byte[] results = www.downloadHandler.data;
				string id = Encoding.UTF8.GetString(results).Substring(7,10);
				UIManager.ShowNetworkStatus("Success! Your stage key is: ", true, id);
			}
		}
	}

	private static IEnumerator LoadFromPastebinHelper(string key)
	{
		UIManager.ShowNetworkStatus("Fetching stage " + key + " from glot.io", false);

		using (UnityWebRequest www = UnityWebRequest.Get("https://snippets.glot.io/snippets/" + key.ToLower()))
		{
			www.downloadHandler = new DownloadHandlerBuffer();
			www.SetRequestHeader("Content-Type", "application/json");
			www.chunkedTransfer = false;
			yield return www.SendWebRequest();

			if (www.isNetworkError)
			{
				Debug.Log("Network Error:" + www.error);
				Debug.Log(www.downloadHandler.text);
				UIManager.ShowNetworkStatus("Load failed: " + www.downloadHandler.text, true);

			}
			else if (www.isHttpError)
			{
				Debug.Log("HTTP Error:" + www.error);
				Debug.Log(www.downloadHandler.text);
				UIManager.ShowNetworkStatus("Load failed: " + www.downloadHandler.text, true);
			}
			else
			{
				// Show results as text
				Debug.Log(www.downloadHandler.text);

				// Or retrieve results as binary data
				byte[] results = www.downloadHandler.data;
				string stripped = Encoding.UTF8.GetString(results);
				stripped = stripped.Substring(stripped.IndexOf("\"content\":") + 11);
				stripped = stripped.Substring(0, stripped.Length - 4);
				stripped = stripped.Replace("\\\"", "\"");
				BlocksFromJsonStream(Encoding.UTF8.GetBytes(stripped),true);
				PlayerPrefs.SetString("LastLoadDir", "glot.io");
				BlockManager.Instance.TogglePlayMode(0.4f);
			}
		}
	}

	public static void SaveToGameJolt()
	{
		GameJolt.API.DataStore.Set("test1", "worked", true);
		GameJolt.API.DataStore.Set("test2", "worked", true);
		BlockManager.Instance.StartCoroutine(Test());
	}

	private static IEnumerator Test()
	{
		yield return new WaitForSeconds(1);
		GameJolt.API.DataStore.GetKeys(true, PrintResult);
	}

	private static void PrintResult(string[] results)
	{
		foreach(string result in results)
		{
			Debug.Log(result);
		}
	}

	public static void LoadBinaryFileUsingHTTP(Uri path)
	{
		//TODO ensure file is valid
		if (path.IsAbsoluteUri && (path.Scheme == Uri.UriSchemeHttp || path.Scheme == Uri.UriSchemeHttps))
		{
			BlockManager.Instance.StartCoroutine(LoadBinaryNetworkHelper(path));
		}
		else
		{
			LogController.Log("Invalid path");
		}
	}

	public static IEnumerator LoadBinaryNetworkHelper(Uri path)
	{
		Debug.Log("loading " + path.ToString());
		using (WWW loader = new WWW(path.ToString().Replace(".json", ".bin")))
		{
			BlockManager.Clear();
			yield return loader;

			BlocksFromBinaryStream(loader.bytes);
			yield return new WaitForEndOfFrame();
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: true);
		}
	}

	//TODO
	public static IEnumerator LoadJsonNetworkHelper(Uri path)
	{
		using (WWW loader = new WWW(path.ToString().Replace(".bin", ".json")))
		{
			BlockManager.Clear();
			yield return loader;
			BlocksFromBinaryStream(loader.bytes);
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: true);
		}
	}

	public static void ReloadCurrentLevel()
	{
		LoadFileUsingLocalPath(LastAccessedFileName);
	}

	public static bool TryReloadCurrentLevel()
	{
		if (!string.IsNullOrWhiteSpace(LastAccessedFileName))
		{
			ReloadCurrentLevel();
			return true;
		}
		return false;
	}

	public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		bool isOk = true;
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None)
		{
			for (int i = 0; i < chain.ChainStatus.Length; i++)
			{
				if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
				{
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build((X509Certificate2)certificate);
					if (!chainIsValid)
					{
						isOk = false;
					}
				}
			}
		}
		return isOk;
	}
}

