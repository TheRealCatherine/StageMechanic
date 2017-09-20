using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {

	public GameObject BasicBlockPrefab;
	private GameObject activeObject;


	void Start() {

	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.C)) {
			GameObject newBlock = Instantiate (BasicBlockPrefab, transform.position, transform.rotation) as GameObject;
			newBlock.transform.SetParent (transform, false);
			activeObject = newBlock;
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			activeObject.transform.position += new Vector3 (0, 1, 0);
		}
	}

	static List<GameObject> GetChildren(GameObject obj) {
		List<GameObject> list = new List<GameObject>();
		foreach (Transform child in obj.transform)
			list.Add (child.gameObject);
		return list;
	}
}
