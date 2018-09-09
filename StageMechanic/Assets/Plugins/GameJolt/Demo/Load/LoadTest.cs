using GameJolt.API;
using GameJolt.UI;
using UnityEngine;

namespace GameJolt.Demo.Load {
	public class LoadTest : MonoBehaviour {
		public void SignInButtonClicked() {
			GameJoltUI.Instance.ShowSignIn(success => {
				GameJoltUI.Instance.QueueNotification(success ? "Welcome" : "Closed the window :(");
			});
		}

		public void SignOutButtonClicked() {
			if(!GameJoltAPI.Instance.HasUser) {
				GameJoltUI.Instance.QueueNotification("You're not signed in");
			} else {
				GameJoltAPI.Instance.CurrentUser.SignOut();
				GameJoltUI.Instance.QueueNotification("Signed out :(");
			}
		}

		public void IsSignedInButtonClicked() {
			if(GameJoltAPI.Instance.HasUser) {
				GameJoltUI.Instance.QueueNotification(
					"Signed in as " + GameJoltAPI.Instance.CurrentUser.Name);
			} else {
				GameJoltUI.Instance.QueueNotification("Not Signed In :(");
			}
		}

		public void LoadSceneButtonClicked(string sceneName) {
			Debug.Log("Loading Scene " + sceneName);
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			Application.LoadLevel(sceneName);
#else
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
#endif
		}
	}
}
