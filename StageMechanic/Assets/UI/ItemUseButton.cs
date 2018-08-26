using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseButton : MonoBehaviour {

	public void OnClicked()
	{
		PlayerManager.Player(0).UseItem();
	}
}
