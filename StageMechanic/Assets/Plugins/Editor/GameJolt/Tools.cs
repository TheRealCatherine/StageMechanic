using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using GameJolt.API;

// ReSharper disable once CheckNamespace
namespace GameJolt.Editor {
	public class Tools : MonoBehaviour {
		private const string DefaultSettingsPath = "Assets/Plugins/GameJolt/GJAPISettings.asset";
		private const string ManagerPrefabPath = "Assets/Plugins/GameJolt/Prefabs/GameJoltAPI.prefab";

		[MenuItem("Edit/Project Settings/Game Jolt API")]
		public static void Settings() {
			ScriptableObject asset;
			var assets = AssetDatabase.FindAssets("t:GameJolt.API.Settings");
			if(assets.Length == 0) {
				asset = ScriptableObject.CreateInstance<Settings>();
				AssetDatabase.CreateAsset(asset, DefaultSettingsPath);
				AssetDatabase.SaveAssets();
			} else {
				asset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0]), typeof(Settings)) as Settings;
			}

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

		[MenuItem("GameObject/Game Jolt API Manager")]
		public static void Manager() {
			var manager = FindObjectOfType<GameJoltAPI>();
			if(manager != null) {
				Selection.activeObject = manager;
			} else {
				var prefab = AssetDatabase.LoadAssetAtPath(ManagerPrefabPath, typeof(GameObject)) as GameObject;
				if(prefab == null) {
					Debug.LogError("Unable to locate Game Jolt API prefab.");
				} else {
					var clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
					Selection.activeObject = clone;

					if(FindObjectOfType<EventSystem>() == null) {
						// ReSharper disable once ObjectCreationAsStatement
						new GameObject(
							"EventSystem",
							typeof(EventSystem),
							typeof(StandaloneInputModule),
							typeof(StandaloneInputModule)
						);
					}
				}
			}
		}
	}
}