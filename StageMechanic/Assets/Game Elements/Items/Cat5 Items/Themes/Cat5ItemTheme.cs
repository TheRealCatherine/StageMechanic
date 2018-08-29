using UnityEngine;

[CreateAssetMenu(fileName ="Cat5ItemTheme",menuName ="Cat5 Item Theme")]
public class Cat5ItemTheme : AbstractItemTheme
{
	[Header("Player Start")]
	public GameObject PlayerStartPlaceholder;
	public GameObject PlayerStartObject;
	public GameObject PlayerAvatar;
	public Sprite PlayerStartIcon;

	[Header("Goal")]
	public GameObject GoalPlaceholder;
	public GameObject GoalObject;
	public Sprite GoalIcon;

	[Header("Checkpoint")]
	public GameObject CheckpointPlaceholder;
	public GameObject CheckpointObject;
	public Sprite CheckpointIcon;

	[Header("Enemy Spawn")]
	public GameObject EnemySpawnPlaceholder;
	public GameObject EnemySpawnObject;
	public Sprite EnemySpawnIcon;

	[Header("Section Spawn")]
	public GameObject SectionSpawnPlaceholder;
	public GameObject SectionSpawnObject;
	public Sprite SectionSpawnIcon;

	[Header("Platform Move")]
	public GameObject PlatformMovePlaceholder;
	public GameObject PlatformMoveObject;
	public Sprite PlatformMoveIcon;

	[Header("Story")]
	public GameObject StoryPlaceholder;
	public GameObject StoryObject;
	public Sprite StoryIcon;

	[Header("Boss State")]
	public GameObject BossStatePlaceholder;
	public GameObject BossStateObject;
	public Sprite BossStateIcon;

	[Header("Coin")]
	public GameObject CoinPlaceholder;
	public GameObject CoinObject;
	public Sprite CoinIcon;

	[Header("Special Collectable")]
	public GameObject SpecialCollectableObject1;
	public GameObject SpecialCollectableObject2;
	public GameObject SpecialCollectableObject3;
	public GameObject SpecialCollectableObject4;
	public Sprite SpecialCollectableIcon1;
	public Sprite SpecialCollectableIcon2;
	public Sprite SpecialCollectableIcon3;
	public Sprite SpecialCollectableIcon4;

	[Header("Create Basic Blocks")]
	public GameObject CreateBasicBlocksPlaceholder;
	public GameObject CreateBasicBlockObject;
	public Sprite CreateBasicBlocksIcon;

	[Header("Create Immobile Blocks")]
	public GameObject CreateImmobileBlocksPlaceholder;
	public GameObject CreateImmobileBlockObject;
	public Sprite CreateImmobileBlocksIcon;

	[Header("Enemy Removal")]
	public GameObject EnemyRemovalPlaceholder;
	public GameObject EnemyRemovalObject;
	public Sprite EnemyRemovalIcon;

	[Header("1 Up")]
	public GameObject OneUpPlaceholder;
	public GameObject OneUpObject;
	public Sprite OneUpIcon;

	[Header("X-Factor")]
	public GameObject XFactorPlaceholder;
	public GameObject XFactorObject;
	public Sprite XFactorIcon;

	[Header("Special Block Remover")]
	public GameObject SpecialBlockRemoverPlaceholder;
	public GameObject SpecialBlockRemoverObject;
	public Sprite SpecialBlockRemoverIcon;

	[Header("Stopwatch")]
	public GameObject StopwatchPlaceholder;
	public GameObject StopwatchObject;
	public Sprite StopwatchIcon;

	[Header("Item Steal")]
	public GameObject ItemStealPlaceholder;
	public GameObject ItemStelObject;
	public Sprite ItemStealIcon;

	[Header("Item Randomizer")]
	public GameObject ItemRandomizerPlaceholder;
	public GameObject ItemRandomizerObject;
	public Sprite ItemRandomizerIcon;

	[Header("Frisbee")]
	public GameObject FrisbeePlaceholder;
	public GameObject FrisbeeObject;
	public Sprite FrisbeeIcon;
}
