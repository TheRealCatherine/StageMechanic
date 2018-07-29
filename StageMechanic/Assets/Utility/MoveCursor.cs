using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveCursor : MonoBehaviour
{
	public void DoMoveCursor(Vector3 direction)
	{
		if (BlockManager.Cursor.transform.position.y < 60000f &&
			BlockManager.Cursor.transform.position.y > -60000f &&
			BlockManager.Cursor.transform.position.x < 60000f &&
			BlockManager.Cursor.transform.position.x > -60000f)

				BlockManager.Cursor.transform.position += direction;
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
