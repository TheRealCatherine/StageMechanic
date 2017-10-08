using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCollider : MonoBehaviour {

	private GameObject _objectUnderCursor;
	public GameObject ObjectUnderCursor {
		get {
			return _objectUnderCursor;
		}
		set {
			_objectUnderCursor = value;
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		_objectUnderCursor = other.gameObject;
	}

	void OnTriggerExit(Collider other)
	{
		_objectUnderCursor = null;
	}
}
