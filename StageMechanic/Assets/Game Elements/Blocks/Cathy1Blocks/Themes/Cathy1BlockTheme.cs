using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName ="Cathy1BlockTheme",menuName ="Cathy1BlockTheme")]
public class Cathy1BlockTheme : AbstractBlockTheme
{
	[Header("Normal Block")]
	public MeshRenderer BasicBlock1;
	public MeshRenderer BasicBlock2; //Optional
	public MeshRenderer BasicBlock3; //Optional
	public MeshRenderer BasicBlock4; //Optional
	public Sprite BasicBlockIcon;  //Optional

	[Header("Small Bomb")]
	public MeshRenderer SmallBombIdle;
	public MeshRenderer SmallBombTriggered; //Optional
	public ParticleSystem SmallBombExplosion; //Optional
	public Vector3 SmallBombExplosionOffset; //Optional
	public AudioClip SmallBombFuseSound; //Optional
	public AudioClip SmallBombExplosionSound; //Optional
	public Sprite SmallBombIcon; //Optional

	[Header("Large Bomb")]
	public MeshRenderer LargeBombIdle;
	public MeshRenderer LargeBombTriggered; //Optional
	public ParticleSystem LargeBombExplosion; //Optional
	public Vector3 LargeBombExplosionOffset; //Optional
	public AudioClip LargeBombFuseSound; //Optional
	public AudioClip LargeBombExplosionSound; //Optional
	public Sprite LargeBombIcon; //Optional

	[Header("Cracked")]
	public MeshRenderer LightCracks;
	public MeshRenderer HeavyCracks;
	public ParticleSystem DisintigrationDust; //Optional
	public Vector3 DustOffset; //Optional
	public AudioClip CrackSound; //Optional
	public AudioClip DisintigrateSound; //Optional
	public Sprite LightCracksIcon; //Optional
	public Sprite HeavyCracksIcon; //Optional

	[Header("Grouped Blocks")]
	//TODO
	public Sprite GroupedBlockIcon; //Optional

	[Header("Goal")]
	public MeshRenderer IdleGoal;
	public MeshRenderer ActiveGoal; //Optional
	public ParticleSystem IdleGoalEffect; //Optional
	public ParticleSystem ActiveGoalEffect; //Optional
	public Vector3 GoalEffectsOffset; //Optional
	public Sprite GoalIcon; //Optional

	[Header("Heavy")]
	public MeshRenderer Heavy;
	public Sprite HeavyIcon; //Optional

	[Header("Ice")]
	public MeshRenderer Ice;
	public ParticleSystem RandomIceEffect;  //Optional
	public Vector3 IceEffectOffset;
	public Sprite IceIcon; //Optional

	[Header("Immobile")]
	public MeshRenderer Immobile;
	public Sprite ImmobileIcon; //Optional

	[Header("Laser")]
	public MeshRenderer IdleLaser;
	public MeshRenderer ActiveLaser; //Optional
	public MeshRenderer DisabledLaser; //Optional
	public ParticleSystem LaserEffect; //Optional
	public Vector3 LaserEffectOffset; //Optional
	public AudioClip LaserWarningSound; //Optional
	public AudioClip LaserFireSound; //Optional (note: I'm afirin' ma layzar)
	public AudioClip DisablingLaserSound; //Optional
	public Sprite LaserIcon; //Optional

	[Header("Monster")]
	public MeshRenderer ActiveMonster;
	public MeshRenderer MovingMonster; //Optional
	public MeshRenderer DisarmedMonster; //Optional (default: BasicBlock1)
	public AudioClip RandomMonsterSound; //Optional
	public AudioClip MoveMonsterSound; //Optional
	public AudioClip DisarmMonsterSound; //Optional
	public ParticleSystem RandomMonsterEffect; //Optional
	public ParticleSystem MoveMonsterEffect; //Optional
	public ParticleSystem DisarmMonsterEffect; //Optional
	public Vector3 MonsterEffectOffset; //Optional
	public Sprite MonsterIcon; //Optional

	[Header("Mystery")]
	public MeshRenderer IdleMystery;
	public MeshRenderer RevealingMystery; //Optional
	public AudioClip MysteryRevealSound; //Optional
	public Sprite MysteryIcon; //Optional

	[Header("spike Trap")]
	public MeshRenderer TrapArmed;
	public MeshRenderer TrapDisarmed;
	public MeshRenderer TrapWarning; //Optional
	public MeshRenderer TrapActive; //Optional
	public ParticleSystem TrapRandomEffect; //Optional
	public ParticleSystem TrapActiveEffect; //Optional
	public Vector3 TrapEffectOffset; //Optional
	public AudioClip TrapWarningSound; //Optional
	public AudioClip TrapActivatedSound; //Optional
	public Sprite TrapIcon; //Optional

	[Header("Spring")]
	public MeshRenderer IdleSpring;
	public MeshRenderer ActiveSpring; //Optional
	public AudioClip SpringSound; //Optional
	public ParticleSystem SpringEffect; //Optional
	public Vector3 SpringEffectOffset; //Optional
	public Sprite SpringIcon; //Optional

	[Header("Vortex")]
	public MeshRenderer IdleVortex;
	public MeshRenderer ActiveVortex; //Optional
	public AudioClip IdleVortexSound; //Optional
	public AudioClip ActiveVortexSound; //Optional
	public ParticleSystem RandomVortexEffect; //Optional
	public ParticleSystem ActiveVortexEffect; //Optional
	public Vector3 VortexEffectOffset; //Optional
	public Sprite VortexIcon; //Optional

}
