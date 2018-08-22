using UnityEngine;

[CreateAssetMenu(fileName ="Cat5ItemTheme",menuName ="Cat5ItemTheme")]
public class Cat5ItemTheme : AbstractBlockTheme
{
	[Header("Player Start")]
	public GameObject PlayerStartPlaceholder;
	public GameObject PlayerStartObject;
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
	public GameObject SpecialCollectablePlaceholder;
	public GameObject SpecialCollectableObject1;
	public GameObject SpecialCollectableObject2;
	public GameObject SpecialCollectableObject3;
	public Sprite SepcialCollectableIcon;

	[Header("Create Blocks")]
	public GameObject CreateBlocksPlaceholder;
	public GameObject CreateBasicBlockObject;
	public GameObject CreateImmobileBlockObject;
	public GameObject CreateFloorBlocksObject;
	public Sprite CreateBlocksIcon;

	[Header("Enemy Removal")]
	public GameObject EnemyRemovalPlaceholder;
	public GameObject EnemyRemovalObject;
	public GameObject EnemyRemovalIcon;

	[Header("1 Up")]
	public GameObject OneUpPlaceholder;
	public GameObject OneUpObject;
	public GameObject OneUpIcon;

	[Header("X-Factor")]
	public GameObject XFactorPlaceholder;
	public GameObject XFactorObject;
	public GameObject XFactorIcon;

	[Header("Special Block Remover")]
	public GameObject SpecialBlockRemoverPlaceholder;
	public GameObject SpecialBlockRemoverObject;
	public GameObject specialBlockRemoverIcon;

	[Header("Stopwatch")]
	public GameObject StopwatchPlaceholder;
	public GameObject StopwatchObject;
	public GameObject StopwatchIcon;

	[Header("Item Steal")]
	public GameObject ItemStealPlaceholder;
	public GameObject ItemStelObject;
	public GameObject ItemStealIcon;

	[Header("Item Randomizer")]
	public GameObject ItemRandomizerPlaceholder;
	public GameObject ItemRandomizerObject;
	public GameObject ItemRandomizerIcon;

	[Header("Frisbee")]
	public GameObject FrisbeePlaceholder;
	public GameObject FrisbeeObject;
	public GameObject FrisbeeIcon;
}
