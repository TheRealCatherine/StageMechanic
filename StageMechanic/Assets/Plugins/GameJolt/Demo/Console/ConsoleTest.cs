using System;
using System.Linq;
using GameJolt.API;
using GameJolt.API.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace GameJolt.Demo.Console {
	public class ConsoleTest : MonoBehaviour {
		#region Inspector Fields
		// Console
		public RectTransform ConsoleTransform;
		public GameObject LinePrefab;

		// Users
		public InputField UserNameField;
		public InputField UserTokenField;
		public InputField UserIdsField;

		// Scores
		public InputField ScoreValueField;
		public InputField ScoreTextField;
		public Toggle GuestScoreToggle;
		public InputField GuestNameField;
		public InputField TableField;
		public InputField LimitField;
		public Toggle UserScoresToggle;
		public InputField CompareValue;
		public Dropdown ScoreOptions;
		public Toggle UserScoresToggle2;

		// Trophies
		public InputField TrophyIdField;
		public InputField TrophyIdsField;
		public Toggle UnlockedTrophiesOnlyToggle;

		// DataStore
		public InputField KeyField;
		public InputField ValueField;
		public Dropdown ModeField;
		public InputField PatternField;
		public Toggle GlobalToggle;
		#endregion Inspector Fields

		#region User
		public void SignIn() {
			Debug.Log("Sign In. Click to see source.");

			var user = new User(UserNameField.text, UserTokenField.text);
			user.SignIn(
				signInSuccess => {
					AddConsoleLine("Sign In {0}.", signInSuccess ? "Successful" : "Failed");
				},
				userFetchSuccess => {
					AddConsoleLine("User's Information Fetch {0}.", userFetchSuccess ? "Successful" : "Failed");
					if(userFetchSuccess)
						PrintUserInfo(user);
				});
		}

		public void SignOut() {
			Debug.Log("Sign Out. Click to see source.");

			var isSignedIn = GameJoltAPI.Instance.HasUser;
			if(isSignedIn) {
				GameJoltAPI.Instance.CurrentUser.SignOut();
			}

			AddConsoleLine("Sign Out {0}.", isSignedIn ? "Successful" : "Failed");
		}

		public void GetFriends() {
			Debug.Log("GetFriends. Click to see source.");
			
			Friends.Get(friends => {
				if(friends == null) {
					AddConsoleLine("GetFriends failed (are you signed in?)");
				} else {
					AddConsoleLine("You have {0} friends.", friends.Length);
					foreach(var friend in friends) {
						PrintUserInfo(friend);
					}
				}
			});
		}

		public void GetUsersById() {
			Debug.Log("Get Users By Id. Click to see source.");

			var ids = ParseIds(UserIdsField.text);
			Users.Get(ids, users => {
				if(users != null) {
					foreach(var user in users) {
						PrintUserInfo(user);
					}
					AddConsoleLine("Found {0} user(s).", users.Length);
				}
			});
		}
		#endregion

		#region Session
		public void SessionOpen() {
			Debug.Log("Session Open. Click to see source.");

			Sessions.Open(success => {
				AddConsoleLine("Session Open {0}.", success ? "Successful" : "Failed");
			});
		}

		public void SessionPingActive() {
			Debug.Log("Session Ping Active. Click to see source.");

			Sessions.Ping(SessionStatus.Active, success => {
				AddConsoleLine("Session Ping Active {0}.", success ? "Successful" : "Failed");
			});
		}

		public void SessionPingIdle() {
			Debug.Log("Session Ping Idle. Click to see source.");

			Sessions.Ping(SessionStatus.Idle, success => {
				AddConsoleLine("Session Ping Idle {0}.", success ? "Successful" : "Failed");
			});
		}

		public void SessionCheck() {
			Debug.Log("Session Check. Click to see source.");

			Sessions.Check(success => {
				if(success == null)
					AddConsoleLine("Session Check Failed");
				else
					AddConsoleLine("Session {0}.", success.Value ? "Active" : "Inactive");
			});
		}

		public void SessionClose() {
			Debug.Log("Session Close. Click to see source.");

			Sessions.Close(success => {
				AddConsoleLine("Session Close {0}.", success ? "Successful" : "Failed");
			});
		}
		#endregion

		#region Scores
		public void GetTables() {
			Debug.Log("Get Tables. Click to see source.");

			Scores.GetTables(tables => {
				if(tables != null) {
					foreach(var table in tables.Reverse()) {
						AddConsoleLine("> {0} - {1}", table.Name, table.ID);
					}
					AddConsoleLine("Found {0} table(s).", tables.Length);
				}
			});
		}

		public void AddScore() {
			if(GuestScoreToggle.isOn) {
				Debug.Log("Add Score (for Guest). Click to see source.");

				var scoreValue = ScoreValueField.text != string.Empty ? int.Parse(ScoreValueField.text) : 0;
				var tableId = TableField.text != string.Empty ? int.Parse(TableField.text) : 0;

				Scores.Add(scoreValue, ScoreTextField.text, GuestNameField.text, tableId, "", success => {
					AddConsoleLine("Score Add (for Guest) {0}.", success ? "Successful" : "Failed");
				});
			} else {
				Debug.Log("Add Score (for Guest). Click to see source.");

				var scoreValue = ScoreValueField.text != string.Empty ? int.Parse(ScoreValueField.text) : 0;
				var tableId = TableField.text != string.Empty ? int.Parse(TableField.text) : 0;

				Scores.Add(scoreValue, ScoreTextField.text, tableId, "", success => {
					AddConsoleLine("Score Add {0}.", success ? "Successful" : "Failed");
				});
			}
		}

		private void GetScoresCallback(Score[] scores) {
			if(scores == null) return;
			foreach(var score in scores.Reverse())
				AddConsoleLine("> {0} - {1}", score.PlayerName, score.Value);
			AddConsoleLine("Found {0} scores(s).", scores.Length);
		}

		public void GetScores() {
			Debug.Log("Get Scores. Click to see source.");

			var tableId = TableField.text != string.Empty ? int.Parse(TableField.text) : 0;
			var limit = LimitField.text != string.Empty ? int.Parse(LimitField.text) : 10;
			Scores.Get(GetScoresCallback, tableId, limit, UserScoresToggle.isOn);
		}

		public void GetScoresWithOptions() {
			var tableId = TableField.text != string.Empty ? int.Parse(TableField.text) : 0;
			var limit = LimitField.text != string.Empty ? int.Parse(LimitField.text) : 10;
			var score = CompareValue.text != string.Empty ? int.Parse(CompareValue.text) : 0;
			if(ScoreOptions.value == 0)
				Scores.GetBetterThan(score, GetScoresCallback, tableId, limit, UserScoresToggle2.isOn);
			else
				Scores.GetWorseThan(score, GetScoresCallback, tableId, limit, UserScoresToggle2.isOn);
		}

		public void GetRank() {
			Debug.Log("Get Rank. Click to see source.");

			var scoreValue = ScoreValueField.text != string.Empty ? int.Parse(ScoreValueField.text) : 0;
			var tableId = TableField.text != string.Empty ? int.Parse(TableField.text) : 0;

			Scores.GetRank(scoreValue, tableId, rank => {
				AddConsoleLine("Rank {0}", rank);
			});
		}
		#endregion

		#region Trophies
		public void UnlockTrophy() {
			Debug.Log("Unlock Trophy. Click to see source.");

			var trophyId = TrophyIdField.text != string.Empty ? int.Parse(TrophyIdField.text) : 0;
			Trophies.Unlock(trophyId, success => {
				AddConsoleLine("Unlock Trophy {0}.", success ? "Successful" : "Failed");
			});
		}

		public void RemoveTrophy() {
			Debug.Log("Remove Trophy. Click to see source.");

			var trophyId = TrophyIdField.text != string.Empty ? int.Parse(TrophyIdField.text) : 0;
			Trophies.Remove(trophyId, success => {
				AddConsoleLine("Remove Trophy {0}.", success ? "Successful" : "Failed");
			});
		}

		public void GetTrophies() {
			if(TrophyIdsField.text.IndexOf(',') == -1) {
				Debug.Log("Get Single Trophy. Click to see source.");

				var trophyId = TrophyIdsField.text != string.Empty ? int.Parse(TrophyIdsField.text) : 0;
				Trophies.Get(trophyId, PrintTrophy);
			} else {
				Debug.Log("Get Multiple Trophies. Click to see source.");

				var trophyIDs = ParseIds(TrophyIdsField.text);

				Trophies.Get(trophyIDs, trophies => {
					if(trophies != null) {
						foreach(var trophy in trophies.Reverse()) {
							PrintTrophy(trophy);
						}
						AddConsoleLine("Found {0} trophies.", trophies.Length);
					}
				});
			}
		}

		public void GetAllTrophies() {
			Debug.Log("Get All Trophies. Click to see source.");

			Trophies.Get(trophies => {
				if(trophies != null) {
					foreach(var trophy in trophies.Reverse()) {
						PrintTrophy(trophy);
					}
					AddConsoleLine("Found {0} trophies.", trophies.Length);
				}
			});
		}

		public void GetTrophiesByStatus() {
			Debug.Log("Get Trophies by Status (Unlocked or not). Click to see source.");

			Trophies.Get(UnlockedTrophiesOnlyToggle.isOn, trophies => {
				if(trophies != null) {
					foreach(var trophy in trophies.Reverse()) {
						PrintTrophy(trophy);
					}
					AddConsoleLine("Found {0} trophies.", trophies.Length);
				}
			});
		}

		private void PrintTrophy(Trophy trophy) {
			if(trophy != null) {
				AddConsoleLine("> {0} - {1} - {2} - {3}Unlocked - {4}Secret", trophy.Title, trophy.ID,
					trophy.Difficulty, trophy.Unlocked ? "" : "Not ", trophy.IsSecret ? "" : "Not ");
			}
		}
		#endregion

		#region DataStore
		public void GetDataStoreKey() {
			Debug.Log("Get DataStore Key. Click to see source.");

			DataStore.Get(KeyField.text, GlobalToggle.isOn, value => {
				if(value != null) {
					AddConsoleLine("> {0}", value);
				}
			});
		}

		public void GetDataStoreKeys() {
			Debug.Log("Get DataStore Keys. Click to see source.");

			DataStore.GetKeys(GlobalToggle.isOn, PatternField.text, keys => {
				if(keys != null) {
					foreach(var key in keys) {
						AddConsoleLine("> {0}", key);
					}
					AddConsoleLine("Found {0} keys.", keys.Length);
				} else {
					AddConsoleLine("No keys found.");
				}
			});
		}

		public void RemoveDataStoreKey() {
			Debug.Log("Remove DataStore Key. Click to see source.");

			DataStore.Delete(KeyField.text, GlobalToggle.isOn, success => {
				AddConsoleLine("Remove DataStore Key {0}.", success ? "Successful" : "Failed");
			});
		}

		public void SetDataStoreKey() {
			Debug.Log("Set DataStore Key. Click to see source.");
			var data = ValueField.text;
			
			// for testing: limit the upload size
			// if the data is larger than 50 bytes, it will be split into 5 packages
			int uploadSize = 50;
			if(data.Length > uploadSize) uploadSize = data.Length / 5 + 1;
			DataStore.SetSegmented(KeyField.text, data, GlobalToggle.isOn, success => {
				AddConsoleLine("Set DataStore Key {0}.", success ? "Successful" : "Failed");
			}, progress => AddConsoleLine("uploaded {0}% ({1}/{2} bytes)",
				progress * 100 / data.Length, progress, data.Length), uploadSize);
		}

		public void UpdateDataStoreKey() {
			DataStoreOperation mode;
			try {
				mode = (DataStoreOperation)System.Enum.Parse(typeof(DataStoreOperation), ModeField.captionText.text);
			} catch {
				Debug.LogWarning("Wrong Mode. Should be Add, Subtract, Multiply, Divide, Append or Prepend.");
				return;
			}

			Debug.Log("Update DataStore Key. Click to see source.");

			DataStore.Update(KeyField.text, ValueField.text, mode, GlobalToggle.isOn, value => {
				if(value != null) {
					AddConsoleLine("> {0}", value);
				}
			});
		}
		#endregion

		#region Misc
		public void GetTime() {
			Debug.Log("Get Time. Click to see source.");

			Misc.GetTime(time => {
				AddConsoleLine("Server Time: {0}", time);
			});
		}
		#endregion

		#region Internal
		private void Start() {
			// Do not try this at home! Seriously, you shouldn't.
			var settings = GameJoltAPI.Instance.Settings;
			if(settings != null) {
				UserNameField.text = settings.DebugUser;
				UserTokenField.text = settings.DebugToken;
			}
			UserIdsField.onValidateInput += ValidateIdList;
			TrophyIdsField.onValidateInput += ValidateIdList;
		}

		private void AddConsoleLine(string format, params object[] args) {
			var tr = Instantiate(LinePrefab).transform;
			tr.GetComponent<Text>().text = string.Format(format, args);
			tr.SetParent(ConsoleTransform);
			tr.SetAsFirstSibling();
		}

		private void PrintUserInfo(User user) {
			AddConsoleLine("> {0} ({1}) - {2}\n   SignedUp: {3}, LastLoggedIn: {4}",
				user.Name, user.ID, user.Type, user.SignedUp, user.LastLoggedIn);
		}

		private char ValidateIdList(string text, int index, char addedChar) {
			if(addedChar >= '0' && addedChar <= '9' || addedChar == ',') return addedChar;
			return '\0';
		}

		private int[] ParseIds(string text) {
			return text.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
		}
		#endregion Internal
	}
}
