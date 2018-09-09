﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameJolt.UI.Behaviours {
	public class KeyboardNavigableForm : StateMachineBehaviour {
		public string FirstFieldPath;
		public string SubmitButtonPath;

		private InputField firstField;
		private Button submitButton;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
			if(firstField == null) {
				var firstFieldTransform = animator.transform.Find(FirstFieldPath);
				if(firstFieldTransform != null) {
					firstField = firstFieldTransform.GetComponent<InputField>();
				}
			}

			if(submitButton == null) {
				var submitButtonTransform = animator.transform.Find(SubmitButtonPath);
				if(submitButtonTransform != null) {
					submitButton = submitButtonTransform.GetComponent<Button>();
				}
			}

			if(firstField != null) {
				firstField.Select();
			}
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
			if(Input.GetKeyDown(KeyCode.Tab)) {
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
					NavigateUp();
				} else {
					NavigateDown();
				}
			}

			if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				Submmit();
			}
		}

		protected virtual void NavigateUp() {
			if(EventSystem.current.currentSelectedGameObject == null) {
				return;
			}

			var next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
			if(next != null) {
				next.Select();
			}
		}

		protected virtual void NavigateDown() {
			if(EventSystem.current.currentSelectedGameObject == null) {
				return;
			}

			var next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			if(next != null) {
				next.Select();
			}
		}

		protected virtual void Submmit() {
			if(submitButton != null) {
				var pointer = new PointerEventData(EventSystem.current);
				ExecuteEvents.Execute(submitButton.gameObject, pointer, ExecuteEvents.submitHandler);
			}
		}
	}
}
