/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
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
