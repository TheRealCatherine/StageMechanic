using UnityEngine;

[CreateAssetMenu(fileName ="Cat5ItemTheme",menuName ="Cat5 Item Theme")]
public class Cat5ItemTheme : AbstractItemTheme
{
	[Header("Player Start")]
	public GameObject PlayerStartPlaceholder;
	public GameObject PlayerStartObject;
	public GameObject PlayerAvatar;
	public GameObject Player2StartPlaceholder;
	public GameObject Player2StartObject;
	public GameObject Player2Avatar;
	public GameObject Player3StartPlaceholder;
	public GameObject Player3StartObject;
	public GameObject Player3Avatar;
	public Sprite PlayerStartIcon;
	public AudioClip PlayerSpawnSound;

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
	public AudioClip CoinCollectionSound;

	[Header("Special Collectable")]
	public GameObject SpecialCollectableObject1;
	public GameObject SpecialCollectableObject2;
	public GameObject SpecialCollectableObject3;
	public GameObject SpecialCollectableObject4;
	public Sprite SpecialCollectableIcon1;
	public Sprite SpecialCollectableIcon2;
	public Sprite SpecialCollectableIcon3;
	public Sprite SpecialCollectableIcon4;
	public AudioClip SpecialCollectableSound;

	[Header("Create Basic Blocks")]
	public GameObject CreateBasicBlocksPlaceholder;
	public GameObject CreateBasicBlockObject;
	public ParticleSystem CreateBlockAnimation;
	public Sprite CreateBasicBlocksIcon;
	public AudioClip CreateBasicBlocksCollectSound;
	public AudioClip CreateBasicBlocksUseSound;

	[Header("Create Immobile Blocks")]
	public GameObject CreateImmobileBlocksPlaceholder;
	public GameObject CreateImmobileBlockObject;
	public Sprite CreateImmobileBlocksIcon;
	public AudioClip CreateImmobileBlocksCollectSound;
	public AudioClip CreateImmobileBlocksUseSound;

	[Header("Enemy Removal")]
	public GameObject EnemyRemovalPlaceholder;
	public GameObject EnemyRemovalObject;
	public ParticleSystem EnemyRemovalAnimation;
	public Sprite EnemyRemovalIcon;
	public AudioClip EnemyRemovalCollectSound;
	public AudioClip EnemyRemovalUseSound;

	[Header("1 Up")]
	public GameObject OneUpPlaceholder;
	public GameObject OneUpObject;
	public Sprite OneUpIcon;
	public AudioClip OneUpSound;

	[Header("X-Factor")]
	public GameObject XFactorPlaceholder;
	public GameObject XFactorObject;
	public AudioClip XFactorActiveAudio;
	public ParticleSystem XFactorActiveAnimation;
	public Sprite XFactorIcon;
	public AudioClip XFactorCollectSound;

	[Header("Special Block Remover")]
	public GameObject SpecialBlockRemoverPlaceholder;
	public GameObject SpecialBlockRemoverObject;
	public ParticleSystem SpecialBlockRemoverAnimation;
	public Sprite SpecialBlockRemoverIcon;
	public AudioClip SpecialBlockRemoverCollectSound;
	public AudioClip SpecialBlockRemoverUseSound;

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
