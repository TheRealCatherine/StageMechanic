/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Cathy1TeleportBlock : Cathy1Block
{
	public ParticleSystem IdleEffect;
	public ParticleSystem ActiveEffect;
	public Vector3 EffectOffset;
	public AudioClip SoundEffect;
	public string DestinationStage;
	public string DestinationBlock;
	public bool hasPlayer = false;

	public override string TypeName
	{
		get
		{
			return "Teleport";
		}
		set
		{

		}
	}

	public override void ApplyTheme(Cathy1BlockTheme theme)
	{
		if (theme.IdleTeleport == null)
		{
			Debug.Assert(theme.IdleGoal != null);
			Model1 = theme.IdleGoal;
		}
		Model1 = theme.IdleTeleport;
		Model2 = theme.ActiveTeleport;
		IdleEffect = theme.IdleTeleportEffect;
		ActiveEffect = theme.ActiveTeleportEffect;
		EffectOffset = theme.TeleportEffectsOffset;
		SoundEffect = theme.TeleportSound;
	}

	public override void Awake()
	{
		base.Awake();
		//Make block immobile
		WeightFactor = 0f;
	}

	bool _hasPlayedSound = false;
	bool _hasShownDialog = false;
	virtual internal void HandlePlayer(PlayerMovementEvent ev)
	{
		if (hasPlayer)
			return;

		if (ev.Location != PlayerMovementEvent.EventLocation.Top)
			return;
		string statename = ev.Player.StateNames[ev.Player.CurrentStateIndex];
		if (statename == "Idle" || statename == "Walk" || statename == "Center")
		{
			if (!_hasPlayedSound)
			{
				if (SoundEffect != null)
					AudioEffectsManager.PlaySound(this, SoundEffect);
				_hasPlayedSound = true;
			}

			string[] startPos = null;
			if (!string.IsNullOrWhiteSpace(DestinationBlock))
			{
				startPos = new string[1];
				startPos[0] = DestinationBlock;
			}
			if(string.IsNullOrWhiteSpace(DestinationStage) && !string.IsNullOrWhiteSpace(DestinationBlock))
			{
				IBlock startBlock = null;
				foreach (IBlock block in BlockManager.BlockCache)
				{
					for (int i = 0; i < startPos.Length; ++i)
					{
						if (block.Name == startPos[i])
							startBlock = block;
					}
				}
				if(startBlock as Cathy1TeleportBlock)
					(startBlock as Cathy1TeleportBlock).hasPlayer = true;
				PlayerManager.SetPlayer1Location(startBlock.Position + Vector3.up);
			}
			//Serializer.BlocksFromJson(location, startPlayMode: true, startPositionOverrides: startPos);

			//TODO support game jolt level chaining
			if (!string.IsNullOrWhiteSpace(DestinationStage) && PlayerPrefs.HasKey("LastLoadDir") && !PlayerPrefs.GetString("LastLoadDir").StartsWith("#/"))
			{
				Uri location = new Uri(PlayerPrefs.GetString("LastLoadDir") + "/" + DestinationStage);
				BlockManager.Instance.TogglePlayMode();
				if (Serializer.UseBinaryFiles)
				{
					string loc = PlayerPrefs.GetString("LastLoadDir") + "/" + DestinationStage;
					loc = loc.Replace(".json", ".bin");
					if (Application.platform == RuntimePlatform.WebGLPlayer)
					{
						BlockManager.Instance.TogglePlayMode(1f);
						BlockManager.Instance.StartCoroutine(Serializer.LoadBinaryNetworkHelper(new Uri(loc)));
					}
					if (Application.platform == RuntimePlatform.Android)
						BlockManager.Instance.TogglePlayMode(1f);
					if (Application.platform == RuntimePlatform.OSXEditor
						|| Application.platform == RuntimePlatform.OSXPlayer
						|| Application.platform == RuntimePlatform.LinuxEditor
						|| Application.platform == RuntimePlatform.LinuxPlayer)
						BlockManager.Instance.TogglePlayMode(0.4f);


					if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.WindowsPlayer && BetterStreamingAssets.FileExists(loc))
					{
						BlockManager.Instance.TogglePlayMode(0.4f);
						Serializer.BlocksFromBinaryStream(BetterStreamingAssets.ReadAllBytes(loc), true);
					}
					else
					{
						BlockManager.Instance.TogglePlayMode(0.4f);
						//TODO test if webgl player uses \ or /
						if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
							loc.Replace("/", "\\");
						Serializer.BlocksFromBinaryStream(File.ReadAllBytes(loc), true);
					}
				}
				else
				{
					if (Application.platform == RuntimePlatform.Android
						|| Application.platform == RuntimePlatform.OSXEditor
						|| Application.platform == RuntimePlatform.OSXPlayer
						|| Application.platform == RuntimePlatform.LinuxEditor
						|| Application.platform == RuntimePlatform.LinuxPlayer)
					{
						if (Application.platform == RuntimePlatform.Android)
							BlockManager.Instance.TogglePlayMode(1f);
						if (Application.platform == RuntimePlatform.OSXEditor
							|| Application.platform == RuntimePlatform.OSXPlayer 
							|| Application.platform == RuntimePlatform.LinuxEditor 
							|| Application.platform == RuntimePlatform.LinuxPlayer)
							BlockManager.Instance.TogglePlayMode(0.4f);

						string loc = PlayerPrefs.GetString("LastLoadDir") + "/" + DestinationStage;
						if (BetterStreamingAssets.FileExists(loc))
						{
							BlockManager.Instance.TogglePlayMode(0.4f);
							Serializer.BlocksFromJsonStream(BetterStreamingAssets.ReadAllBytes(loc), true);
						}
						else
						{
							BlockManager.Instance.TogglePlayMode(0.4f);
							//TODO test if webgl player uses \ or /
							if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
								loc.Replace("/", "\\");
							Serializer.BlocksFromJsonStream(File.ReadAllBytes(loc), true);
						}

					}
					else
					{
						BlockManager.Instance.TogglePlayMode(0.4f);
						Serializer.LoadFileUsingLocalPath(location.LocalPath);
					}
				}
			}
			else if (string.IsNullOrWhiteSpace(DestinationStage) && string.IsNullOrWhiteSpace(DestinationBlock))
			{
				IBlock startBlock = null;
				List<IBlock> blocks = BlockManager.GetBlocksOfType("Teleport");
				if (blocks.Count > 1) {
					blocks.Remove(this);
					startBlock = blocks.RandomElement();
				}
				if (startBlock as Cathy1TeleportBlock)
				{
					(startBlock as Cathy1TeleportBlock).hasPlayer = true;
					PlayerManager.SetPlayer1Location(startBlock.Position + Vector3.up);
				}
			}
		}
	}

	protected override void OnPlayerEnter(PlayerMovementEvent ev)
	{
		base.OnPlayerEnter(ev);
		ShowModel(2);
		HandlePlayer(ev);
		hasPlayer = true;
	}

	protected override void OnPlayerStay(PlayerMovementEvent ev)
	{
		base.OnPlayerStay(ev);
		ShowModel(2);
		HandlePlayer(ev);
		hasPlayer = true;
	}

	protected override void OnPlayerLeave(PlayerMovementEvent ev)
	{
		base.OnPlayerLeave(ev);
		ShowModel(1);
		_hasPlayedSound = false;
		hasPlayer = false;
	}

	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			//TODO ret.Add("Destination Stage", new DefaultValue { TypeInfo = typeof(string), Value = string.Empty });
			ret.Add("Destination Block", new DefaultValue { TypeInfo = typeof(string), Value = string.Empty });
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (!string.IsNullOrEmpty(DestinationStage) && !string.IsNullOrWhiteSpace(DestinationStage))
				ret.Add("Destination Stage", DestinationStage);
			if (!string.IsNullOrEmpty(DestinationBlock) && !string.IsNullOrWhiteSpace(DestinationBlock))
				ret.Add("Destination Block", DestinationBlock);
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Destination Stage"))
				DestinationStage = value["Destination Stage"];
			if (value.ContainsKey("Destination Block"))
				DestinationBlock = value["Destination Block"];
		}
	}
}
