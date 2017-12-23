using UnityEngine;

using System;
using System.IO;
using System.Linq;

using GracesGames.Common.Scripts;
using GracesGames.SimpleFileBrowser.Scripts.UI;

namespace GracesGames.SimpleFileBrowser.Scripts {

	// Enum used to define save and load mode
	public enum FileBrowserMode {
		Save,
		Load
	}

	// Enum used to define landscape or portrait view mode
	public enum ViewMode {
		Landscape,
		Portrait
	}

	public class FileBrowser : MonoBehaviour {

		// ----- PUBLIC UI ELEMENTS -----

		// The File Browser UI Landscape mode as prefab
		public GameObject FileBrowserLandscapeUiPrefab;

		// The File Browser UI Portrait mode as prefab
		public GameObject FileBrowserPortraitUiPrefab;

		// ----- PUBLIC FILE BROWSER SETTINGS -----

		// Whether directories and files should be displayed in one panel
		public ViewMode ViewMode = ViewMode.Landscape;

		// Whether files with incompatible extensions should be hidden
		public bool HideIncompatibleFiles;

		// ----- PRIVATE UI ELEMENTS ------

		// The user interface script for the Level Editor
		private UserInterface _uiScript;

		// String used to filter files on name basis 
		private string _searchFilter = "";

		// ----- Private FILE BROWSER SETTINGS -----

		// Variable to set save or load mode
		private FileBrowserMode _mode;

		// MonoBehaviour script used to call this script
		// Saved for the call back with the (empty) result
		private MonoBehaviour _callerScript;

		// Method to be called of the callerScript when selecting a file or closing the file browser
		private string _callbackMethod;

		// The current path of the file browser
		// Instantiated using the current directory of the Unity Project
		private string _currentPath;

		// The currently selected file
		private string _currentFile;

		// The name for file to be saved
		private string _saveFileName;

		// Location of Android root directory, can be different for different device manufacturers
		private string _rootAndroidPath;

		// Stacks to keep track for backward and forward navigation feature
		private readonly FiniteStack<string> _backwardStack = new FiniteStack<string>();

		private readonly FiniteStack<string> _forwardStack = new FiniteStack<string>();

		// String file extension to filter results and save new files
		private string _fileExtension;

		// ----- METHODS -----

		// Method used to setup the FileBrowser
		public void SetupFileBrowser(ViewMode newViewMode) {
			// Set the view mode (landscape or portrait)
			ViewMode = newViewMode;

			// Find the canvas so UI elements can be added to it
			GameObject uiCanvas = GameObject.Find("Canvas");
			// Instantiate the file browser UI using the transform of the canvas
			// Then call the Setup method of the SetupUserInterface class to setup the User Interface using the set values
			if (uiCanvas != null) {
				GameObject userIterfacePrefab =
					ViewMode == ViewMode.Portrait ? FileBrowserPortraitUiPrefab : FileBrowserLandscapeUiPrefab;
				GameObject fileBrowserUi = Instantiate(userIterfacePrefab, uiCanvas.transform, false);
				_uiScript = fileBrowserUi.GetComponent<UserInterface>();
				_uiScript.Setup(this);
			} else {
				Debug.LogError("Make sure there is a canvas GameObject present in the Hierarcy (Create UI/Canvas)");
			}
			SetupPath();
		}

		// Sets the current path (Android or other devices)
		private void SetupPath() {
			if (IsAndroidPlatform()) {
				SetupAndroidVariables();
				_currentPath = _rootAndroidPath;
			} else {
				_currentPath = Directory.GetCurrentDirectory();
			}
		}

		// Set up Android external storage root directory, else default to Directory.GetCurrentDirectory()
		private void SetupAndroidVariables() {
			_rootAndroidPath = GetAndroidExternalFilesDir();
		}

		// Returns the external files directory for Android OS, else default to Directory.GetCurrentDirectory()
		private String GetAndroidExternalFilesDir() {
			string path = "";
			if (IsAndroidPlatform()) {
				try {
					using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Environment")) {
						path = androidJavaClass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory")
							.Call<string>("getAbsolutePath");
					}
				}
				catch (Exception e) {
					Debug.LogWarning("Error fetching native Android external storage dir: " + e.Message);
					path = Directory.GetCurrentDirectory();
				}
			}
			return path;
		}

		// Returns the current mode (save or load)
		public FileBrowserMode GetMode() {
			return _mode;
		}

		// Returns to the previously selected directory (inverse of DirectoryForward)
		public void DirectoryBackward() {
			// See if there is anything on the backward stack
			if (_backwardStack.Count > 0) {
				// If so, push it to the forward stack
				_forwardStack.Push(_currentPath);
			}
			// Get the last path entry
			string backPath = _backwardStack.Pop();
			if (backPath != null) {
				// Set path and update the file browser
				_currentPath = backPath;
				UpdateFileBrowser();
			}
		}

		// Goes forward to the previously selected directory (inverse of DirectoryBackward)
		public void DirectoryForward() {
			// See if there is anything on the redo stack
			if (_forwardStack.Count > 0) {
				// If so, push it to the backward stack
				_backwardStack.Push(_currentPath);
			}
			// Get the last level entry
			string forwardPath = _forwardStack.Pop();
			if (forwardPath != null) {
				// Set path and update the file browser
				_currentPath = forwardPath;
				UpdateFileBrowser();
			}
		}

		// Moves one directory up and update file browser
		// When there is no parent, show the drives of the computer
		public void DirectoryUp() {
			_backwardStack.Push(_currentPath);
			if (!IsTopLevelReached()) {
				_currentPath = Directory.GetParent(_currentPath).FullName;
				UpdateFileBrowser();
			} else {
				UpdateFileBrowser(true);
			}
		}

		// Parent directory check as Android throws a permission error if it tries to go above the root external storage directory
		private bool IsTopLevelReached() {
			if (IsAndroidPlatform()) {
				return Directory.GetParent(_currentPath).FullName == Directory.GetParent(_rootAndroidPath).FullName;
			}
			return Directory.GetParent(_currentPath) == null;

		}

		// Closes the file browser and send back an empty string
		public void CloseFileBrowser() {
			SendCallbackMessage("");
		}

		// When a file is selected (save/load button clicked), 
		// send a message to the caller script
		public void SelectFile() {
			// When saving, send the path and new file name, else the selected file
			if (_mode == FileBrowserMode.Save) {
				string inputFieldValue = _uiScript.GetSaveFileText();
				// Additional check for invalid input field value
				// Should never be true due to onValueChanged check with toggle on save button
				if (String.IsNullOrEmpty(inputFieldValue)) {
					Debug.LogError("Invalid file name given");
				} else {
					SendCallbackMessage(_currentPath + "/" + inputFieldValue);
				}
			} else {
				SendCallbackMessage(_currentFile);
			}
		}

		// Sends back a message to the callerScript and callbackMethod
		// Then destroys the FileBrowser
		private void SendCallbackMessage(string message) {
			_callerScript.SendMessage(_callbackMethod, message);
			Destroy();
		}

		// Checks the current value of the InputField. If it is an empty string, disable the save button
		public void CheckValidFileName(string inputFieldValue) {
			_uiScript.ToggleSelectFileButton(inputFieldValue != "");
		}

		// Updates the search filter and filters the UI
		public void UpdateSearchFilter(string searchFilter) {
			_searchFilter = searchFilter;
			UpdateFileBrowser();
		}

		// Updates the file browser by updating the path, file name, directories and files
		private void UpdateFileBrowser(bool topLevel = false) {
			UpdatePathText();
			UpdateLoadFileText();
			_uiScript.ResetParents();
			BuildDirectories(topLevel);
			BuildFiles();
		}

		// Updates the path text
		private void UpdatePathText() {
			_uiScript.UpdatePathText(_currentPath);
		}

		// Updates the file to load text
		private void UpdateLoadFileText() {
			_uiScript.UpdateLoadFileText(_currentFile);
		}

		// Creates a DirectoryButton for each directory in the current path
		private void BuildDirectories(bool topLevel) {
			// Get the directories
			string[] directories = Directory.GetDirectories(_currentPath);
			// If the top level is reached return the drives
			if (topLevel) {
				if (IsWindowsPlatform()) {
					directories = Directory.GetLogicalDrives();
				} else if (IsMacOsPlatform()) {
					directories = Directory.GetDirectories("/Volumes");
				} else if (IsAndroidPlatform()) {
					_currentPath = _rootAndroidPath;
					directories = Directory.GetDirectories(_currentPath);
				}
			}
			// For each directory in the current directory, create a DirectoryButton and hook up the DirectoryClick method
			foreach (string dir in directories) {
				_uiScript.CreateDirectoryButton(dir);
			}
		}

		// Returns whether the application is run on a Windows Operating System
		private bool IsWindowsPlatform() {
			return (Application.platform == RuntimePlatform.WindowsEditor ||
			        Application.platform == RuntimePlatform.WindowsPlayer);
		}

		private bool IsAndroidPlatform() {
			return Application.platform == RuntimePlatform.Android;
		}

		// Returns whether the application is run on a Mac Operating System
		private bool IsMacOsPlatform() {
			return (Application.platform == RuntimePlatform.OSXEditor ||
			        Application.platform == RuntimePlatform.OSXPlayer);
		}

		// Creates a FileButton for each file in the current path
		private void BuildFiles() {
			// Get the files
			string[] files = Directory.GetFiles(_currentPath);
			// Apply search filter when not empty
			if (!String.IsNullOrEmpty(_searchFilter)) {
				files = ApplyFileSearchFilter(files);
			}

			// For each file in the current directory, create a FileButton and hook up the FileClick method
			foreach (string file in files) {
				// Hide files (no button) with incompatible file extensions when enabled
				if (HideIncompatibleFiles) {
					if (CompatibleFileExtension(file)) {
						_uiScript.CreateFileButton(file);
					}
				} else {
					_uiScript.CreateFileButton(file);
				}
			}
		}

		// Apply search filter to string array of files and return filtered string array
		private string[] ApplyFileSearchFilter(string[] files) {
			// Keep files that whose name contains the search filter text
			return files.Where(file =>
				(!String.IsNullOrEmpty(file) &&
				 Path.GetFileName(file).IndexOf(_searchFilter, StringComparison.OrdinalIgnoreCase) >= 0)).ToArray();
		}

		// Returns whether the file given is compatible (correct file extension)
		public bool CompatibleFileExtension(string file) {
			return file.EndsWith("." + _fileExtension);
		}

		// When a directory is clicked, update the path and the file browser
		public void DirectoryClick(string path) {
			_backwardStack.Push(_currentPath.Clone() as string);
			_currentPath = path;
			UpdateFileBrowser();
		}

		// When a file is click, validate and update the save file text or current file and update the file browser
		public void FileClick(string clickedFile) {
			// When in save mode, update the save name to the clicked file name
			// Else update the current file text
			if (_mode == FileBrowserMode.Save) {
				string clickedFileName = Path.GetFileNameWithoutExtension(clickedFile);
				CheckValidFileName(clickedFileName);
				_uiScript.SetFileNameInputField(clickedFileName, _fileExtension);
			} else {
				_currentFile = clickedFile;
			}
			UpdateFileBrowser();
		}

		// Opens a file browser in save mode
		// Requires a caller script and a method for the callback result
		// Also requires a default file and a file extension
		public void SaveFilePanel(MonoBehaviour callerScript, string callbackMethod, string defaultName,
			string fileExtension) {
			// Make sure the file extension is not null, else set it to "" (no extension for the file to save)
			if (fileExtension == null) {
				fileExtension = "";
			}
			_mode = FileBrowserMode.Save;
			_uiScript.SetSaveMode(defaultName, fileExtension);
			FilePanel(callerScript, callbackMethod, fileExtension);
		}

		// Opens a file browser in load mode
		// Requires a caller script and a method for the callback result 
		// Also a file extension used to filter the loadable files
		public void OpenFilePanel(MonoBehaviour callerScript, string callbackMethod, string fileExtension) {
			// Make sure the file extension is not invalid, else set it to * (no filter for load)
			if (String.IsNullOrEmpty(fileExtension)) {
				fileExtension = "*";
			}
			_mode = FileBrowserMode.Load;
			_uiScript.SetLoadMode();
			FilePanel(callerScript, callbackMethod, fileExtension);
		}

		// Generic file browser panel to remove duplicate code
		private void FilePanel(MonoBehaviour callerScript, string callbackMethod, string fileExtension) {
			// Set values
			_fileExtension = fileExtension;
			_callerScript = callerScript;
			_callbackMethod = callbackMethod;
			// Call update once to set all files for initial directory
			UpdateFileBrowser();
		}

		// Destroy this file browser (the UI and the GameObject)
		private static void Destroy() {
			Destroy(GameObject.Find("FileBrowserUI"));
			Destroy(GameObject.Find("FileBrowser"));
		}
	}
}