using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaProxy_PlayerManager
{
	public int count
	{
		get
		{
			return PlayerManager.PlayerCount;
		}
	}

	public AbstractPlayerCharacter player(int number=0)
	{
		return (PlayerManager.Player(number) as AbstractPlayerCharacter);
	}

	public void clear()
	{
		PlayerManager.Clear();
	}

	public void showall(bool show = true)
	{
		PlayerManager.ShowAllPlayers(show);
	}

	public void hideall()
	{
		showall(false);
	}

	public AbstractPlayerCharacter at(float x, float y, float z)
	{
		return PlayerManager.GetPlayerNear(new Vector3(x, y, z), 0.1f);
	}

	public AbstractPlayerCharacter at(Vector3 position)
	{
		return PlayerManager.GetPlayerNear(position, 0.1f);
	}
}
