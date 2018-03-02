using UnityEngine;


[CreateAssetMenu(fileName = "PushPullBlockTheme", menuName = "PushPullBlockTheme")]
public class PushPullBlockTheme : AbstractBlockTheme {
	[Header("Grass")]
	public GameObject Grass;
	public Sprite GrassIcon;

	[Header("Checkerd Gameboard")]
	public GameObject Gameboard1;
	public GameObject Gameboard2;
	public Sprite GameboardIcon;

	public GameObject[] Blocks;
	public Sprite Block;
}
