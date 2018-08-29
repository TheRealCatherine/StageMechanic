using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinZAxis : MonoBehaviour {

		public float speed = 10f;

		void Update()
		{
			transform.Rotate(Vector3.forward, speed * Time.deltaTime);
		}
}
