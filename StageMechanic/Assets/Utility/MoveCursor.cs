using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveCursor : MonoBehaviour
{
	public float MoveDuration = 0.15f;

	public void DoMoveCursor(Vector3 direction)
	{
		Vector3 newpos = BlockManager.Cursor.transform.position + direction;

		if (newpos.y < 60000f &&
			newpos.y > 0f &&
			newpos.x < 60000f &&
			newpos.x > -60000f)
			//BlockManager.Cursor.transform.DOMove(newpos, MoveDuration);
			BlockManager.Cursor.transform.position = newpos;
	}

	public void MoveUp()
	{
		DoMoveCursor(Vector3.up);
	}

	public void MoveDown()
	{
		DoMoveCursor(Vector3.down);
	}

	public void MoveLeft()
	{
		DoMoveCursor(Vector3.left);
	}

	public void MoveRight()
	{
		DoMoveCursor(Vector3.right);
	}

	public void MoveIn()
	{
		DoMoveCursor(Vector3.back);
	}

	public void MoveOut()
	{
		DoMoveCursor(Vector3.forward);
	}
}
