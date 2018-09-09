using System.Collections;
using GameJolt.API;
using UnityEngine;
using UnityEngine.UI;

namespace GameJolt.Demo.UI {
	public class UserInfoBox : MonoBehaviour {
		public Image Avatar;
		public Text Name;
		public Text Id;
		public Text UserType;

		private void Start() {
			StartCoroutine(UpdateRoutine());
		}

		private IEnumerator UpdateRoutine() {
			var wait = new WaitForSeconds(1f);
			while(enabled) {
				UpdateInfos();
				yield return wait;
			}
		}

		private void UpdateInfos() {
			var user = GameJoltAPI.Instance.CurrentUser;
			Avatar.sprite = user != null ? user.Avatar : null;
			Name.text = user != null ? user.Name : "<UserName>";
			Id.text = user != null ? user.ID.ToString() : "<ID>";
			UserType.text = user != null ? user.Type.ToString() : "<UserType>";
		}
	}
}
