﻿/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1BombBlock : Cathy1AbstractTrapBlock
{
	public Material ArmedStateMaterial;
	public Material TriggeredStateMaterial;
	public Material ActiveStateMaterial;
	public Material DisarmedStateMaterial;

	public AudioClip FuseSound;
	public AudioClip ExplosionSound;

	public ParticleSystem ExplosionAnimation;
	public float ExplosionAnimationScale = 2f;
	public Vector3 ExplosionAnimationOffset;

	private const float SMALL_BOMB_DEFAULT_FUSE_TIME = 1.5f;
	private const float LARGE_BOMB_DEFAULT_FUSE_TIME = 1.5f;
	private const int SMALL_BOMB_DEFAULT_RADIUS = 1;
	private const int LARGE_BOMB_DEFAULT_RADIUS = 2;
	private const float SMALL_BOMB_DEFAULT_ANIMATION_SCALE = 2f;
	private const float LARGE_BOMB_DEFAULT_ANIMATION_SCALE = 3f;

	public enum BombSize
	{
		Small,
		Large
	}

	public BombSize Size = BombSize.Small;

	public override void ApplyTheme(Cathy1BlockTheme theme)
	{
		if (Size == BombSize.Small)
		{
			Debug.Assert(theme.SmallBombIdle != null);
			Model1 = theme.SmallBombIdle;
			Model2 = theme.SmallBombTriggered;
			ExplosionAnimation = theme.SmallBombExplosion;
			ExplosionAnimationOffset = theme.SmallBombExplosionOffset;
			FuseSound = theme.SmallBombFuseSound;
			ExplosionSound = theme.SmallBombExplosionSound;
		}
		else
		{
			Debug.Assert(theme.LargeBombIdle != null);
			Model1 = theme.LargeBombIdle;
			Model2 = theme.LargeBombTriggered;
			ExplosionAnimation = theme.LargeBombExplosion;
			ExplosionAnimationOffset = theme.LargeBombExplosionOffset;
			FuseSound = theme.LargeBombFuseSound;
			ExplosionSound = theme.LargeBombExplosionSound;
		}
	}

	/// <summary>
	/// This class is used for both Bomb1 and Bomb2 types
	/// Setting to a different type should throw an exception
	/// for now just set it to the small one by default
	/// TODO
	/// </summary>

	/// <summary>
	/// Indicate to the Cathy1 game rules that this is a spike trap
	/// </summary>
	public sealed override TrapBlockType TrapType
	{
		get
		{
			if (Size == BombSize.Small)
				return Cathy1AbstractTrapBlock.TrapBlockType.SmallBomb;
			return Cathy1AbstractTrapBlock.TrapBlockType.BigBomb;
		}
	}

	public sealed override float TriggerTime { get; set; } = SMALL_BOMB_DEFAULT_FUSE_TIME;

	/// <summary>
	/// Sets the trigger time of the spike trap
	/// </summary>
	public override void Awake()
	{
		base.Awake();
		if (Size == BombSize.Small)
		{
			DamageRadius = new Vector3(SMALL_BOMB_DEFAULT_RADIUS, SMALL_BOMB_DEFAULT_RADIUS, SMALL_BOMB_DEFAULT_RADIUS);
			TriggerTime = SMALL_BOMB_DEFAULT_FUSE_TIME;
			ExplosionAnimationScale = SMALL_BOMB_DEFAULT_ANIMATION_SCALE;
		}
		else
		{
			DamageRadius = new Vector3(LARGE_BOMB_DEFAULT_RADIUS, LARGE_BOMB_DEFAULT_RADIUS, LARGE_BOMB_DEFAULT_RADIUS);
			TriggerTime = LARGE_BOMB_DEFAULT_FUSE_TIME;
			ExplosionAnimationScale = LARGE_BOMB_DEFAULT_ANIMATION_SCALE;
		}

	}

	[SerializeField]
	private Vector3 _epicenterOffset;
	public override Vector3 Epicenter
	{
		get
		{
			return base.Epicenter+_epicenterOffset;
		}

		set
		{
			base.Epicenter = value - _epicenterOffset;
		}
	}

	private void DoExplosion()
	{
		if(ExplosionSound != null)
			AudioEffectsManager.PlaySound(this, ExplosionSound);
		foreach (AbstractPlayerCharacter player in PlayerManager.GetPlayersNear(Epicenter + Vector3.up, radius: 0.25f))
		{
			player.TakeDamage(float.PositiveInfinity);
		}

		if (_epicenterOffset == Vector3.zero)
			gameObject.SetActive(false);
		List<AbstractBlock> localBlocks = BlockManager.GetBlocksNear(Epicenter, DamageRadius.x);
		foreach (AbstractBlock block in localBlocks)
		{
			Cathy1Block c1b = block as Cathy1Block;
			if (c1b != null && c1b != this && c1b.gameObject.activeInHierarchy)
			{
				//TODO do this by having the blocks take damage
				if (c1b.TypeName == "Basic")
					BlockManager.CreateBlockAt(block.Position, "Cathy1 Internal", "Cracked (2 Steps)");
				else if (c1b.TypeName == "Cracked (2 Steps)")
					BlockManager.CreateBlockAt(block.Position, "Cathy1 Internal", "Cracked (1 Step)");
				else if (c1b.TypeName == "Cracked (1 Step)")
					BlockManager.DestroyBlock(c1b);
				else if (c1b.TypeName == "Small Bomb" || c1b.TypeName == "Large Bomb")
					(c1b as Cathy1BombBlock)?.InstantDetonate();
			}
		}
		IsArmed = false;
		if (_epicenterOffset == Vector3.zero)
			BlockManager.DestroyBlock(this);

	}

	internal override IEnumerator HandleStep()
	{
		ShowModel(2);
		AudioEffectsManager.PlaySound(this, FuseSound);
		if(ExplosionAnimation != null && ExplosionAnimationScale>0)
			VisualEffectsManager.PlayEffect(this, ExplosionAnimation, ExplosionAnimationScale, TriggerTime, _epicenterOffset);
		yield return new WaitForSeconds(TriggerTime);
		DoExplosion();
	}

	public void InstantDetonate()
	{
		if (IsTriggered)
			return;
		IsTriggered = true;
		if (ExplosionAnimation != null)
			VisualEffectsManager.PlayEffect(this, ExplosionAnimation, ExplosionAnimationScale, 0.1f, _epicenterOffset);
		DoExplosion();
	}

	public override Dictionary<string,DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add("Trigger Time (seconds)",   new DefaultValue { TypeInfo = typeof(float),    Value = (Size == BombSize.Small ? SMALL_BOMB_DEFAULT_FUSE_TIME : LARGE_BOMB_DEFAULT_FUSE_TIME).ToString() });
			ret.Add("Damage Radius",            new DefaultValue { TypeInfo = typeof(int),      Value = (Size == BombSize.Small ? SMALL_BOMB_DEFAULT_RADIUS : LARGE_BOMB_DEFAULT_RADIUS).ToString() });
			ret.Add("Animation Scale",          new DefaultValue { TypeInfo = typeof(float),    Value = (Size == BombSize.Small ? SMALL_BOMB_DEFAULT_ANIMATION_SCALE : LARGE_BOMB_DEFAULT_ANIMATION_SCALE).ToString() });
			ret.Add("Epicenter Offset",         new DefaultValue { TypeInfo = typeof(Vector3),  Value = Vector3.zero.ToString() });
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (TriggerTime != SMALL_BOMB_DEFAULT_FUSE_TIME && Size == BombSize.Small)
				ret.Add("Trigger Time (seconds)", TriggerTime.ToString());
			if (TriggerTime != LARGE_BOMB_DEFAULT_FUSE_TIME && Size == BombSize.Large)
				ret.Add("Trigger Time (seconds)", TriggerTime.ToString());
			if (Size == BombSize.Small && DamageRadius != new Vector3(SMALL_BOMB_DEFAULT_RADIUS, SMALL_BOMB_DEFAULT_RADIUS, SMALL_BOMB_DEFAULT_RADIUS))
				ret.Add("Damage Radius", DamageRadius.x.ToString());
			if (Size == BombSize.Large && DamageRadius != new Vector3(SMALL_BOMB_DEFAULT_RADIUS, LARGE_BOMB_DEFAULT_RADIUS, LARGE_BOMB_DEFAULT_RADIUS))
				ret.Add("Damage Radius", DamageRadius.x.ToString());
			if (Size == BombSize.Small && ExplosionAnimationScale != SMALL_BOMB_DEFAULT_ANIMATION_SCALE)
				ret.Add("Animation Scale", ExplosionAnimationScale.ToString());
			if (Size == BombSize.Large && ExplosionAnimationScale != LARGE_BOMB_DEFAULT_ANIMATION_SCALE)
				ret.Add("Animation Scale", ExplosionAnimationScale.ToString());
			if (_epicenterOffset != Vector3.zero)
				ret.Add("Epicenter Offset", _epicenterOffset.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Trigger Time (seconds)"))
				TriggerTime = float.Parse(value["Trigger Time (seconds)"]);
			if (value.ContainsKey("Damage Radius"))
			{
				int radius = int.Parse(value["Damage Radius"]);
				DamageRadius = new Vector3(radius, radius, radius);
			}
			if (value.ContainsKey("Animation Scale"))
				ExplosionAnimationScale = float.Parse(value["Animation Scale"]);
			if (value.ContainsKey("Epicenter Offset"))
				_epicenterOffset = Utility.StringToVector3(value["Epicenter Offset"]);
		}
	}
}
