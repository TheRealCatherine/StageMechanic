using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BoxGirlBlockTheme", menuName = "BoxGirlBlockTheme")]
public class BoxGirlBlockTheme : AbstractBlockTheme {
	[Header("Door")]
	public GameObject DoorBlock;
	public GameObject Door;
	public GameObject DoorOpenAnimation;
	public GameObject DoorCloseAnimation;
	public Vector3 DoorFrameOffset;
	public AudioClip DoorOpenSound;
	public AudioClip DoorCloseSound;
	public Sprite DoorIcon;
}
