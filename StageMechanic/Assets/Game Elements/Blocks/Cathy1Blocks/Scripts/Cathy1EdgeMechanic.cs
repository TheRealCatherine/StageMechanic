/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1EdgeMechanic : MonoBehaviour
{

	public bool IsGrounded { get; set; } = false;

	public bool inspectorGrounded = false;
	public Vector3 dodododo;

	void Update ()
	{
		Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block> ();
		if (thisBlock == null)
			return;

		BlockManager bm = thisBlock.BlockManager;
		Debug.Assert (bm != null);
		if (bm.PlayMode) {

			IsGrounded = false;
			inspectorGrounded = false;
			dodododo = Vector3.zero;

			Vector3 down = transform.TransformDirection (Vector3.down);

			if (Physics.Raycast (transform.position, down, 1f) && (transform.position.y % 1) == 0) {
				IsGrounded = true;
				inspectorGrounded = true;
			}

			List<Collider> crossColiders = new List<Collider> (Physics.OverlapBox (transform.position - new Vector3 (0f, 0.75f, 0f), new Vector3 (0.1f, 0.1f, 0.75f)));
			crossColiders.AddRange (Physics.OverlapBox (transform.position - new Vector3 (0f, 0.75f, 0f), new Vector3 (0.75f, 0.1f, 0.1f)));

			foreach (Collider col in crossColiders) {
				if (col.gameObject == gameObject)
					continue;
				Cathy1EdgeMechanic otherBlock = col.gameObject.GetComponent<Cathy1EdgeMechanic> ();
				if (otherBlock == null)
					continue;
				if (!otherBlock.IsGrounded)
					continue;
				if ((transform.position.y % 1) == 0) {
					inspectorGrounded = false;
					dodododo = otherBlock.transform.position;
					IsGrounded = true;
					break;
				}
			}

		}
		ApplyGravity ();
	}

	public void ApplyGravity ()
	{
		if (IsGrounded)
			return;

		Cathy1Block thisBlock = gameObject.GetComponent<Cathy1Block> ();
		if (thisBlock == null)
			return;

		BlockManager bm = thisBlock.BlockManager;
		Debug.Assert (bm != null);
		if (bm.PlayMode)
			thisBlock.Position -= new Vector3 (0, 0.25f, 0);
	}

	public bool TestForSupportedBlock (int height)
	{
		foreach (Collider col in Physics.OverlapBox(transform.position + new Vector3(0f,0.7f*height,0f),new Vector3(0.01f,0.01f,0.01f))) {
			if (col.gameObject == gameObject)
				continue;
			Cathy1EdgeMechanic otherBlock = col.gameObject.GetComponent<Cathy1EdgeMechanic> ();
			if (otherBlock == null)
				continue;
			return true;
		}
		return false;
	}
}
