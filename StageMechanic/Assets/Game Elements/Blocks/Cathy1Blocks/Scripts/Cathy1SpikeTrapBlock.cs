/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements a normal Cath1-style spike trap. Upon the player or enemy stepping on this
/// block it will trigger after the specified trigger time, allowing the player or enemy
/// a certain amount of time to move before the trap enters the IsTriggerd state before
/// moving into the disarned state.
/// </summary>
public class Cathy1SpikeTrapBlock : Cathy1AbstractTrapBlock
{
	public ParticleSystem RandomEffect;
	public ParticleSystem ActiveEffect;
	public Vector3 EffectOffset;
	public AudioClip WarningSound;
	public AudioClip ActivatedSound;
	public float AnimationScale = DEFAULT_ANIMATION_SCALE;

	private const float DEFAULT_TRIGGER_TIME = 1.2f;
	private const float DEFAULT_ANIMATION_SCALE = 2;

	public sealed override TrapBlockType TrapType { get; } = TrapBlockType.Spike;
	public sealed override float TriggerTime { get; set; } = DEFAULT_TRIGGER_TIME;

	public override void ApplyTheme(Cathy1BlockTheme theme)
	{
		Debug.Assert(theme.TrapArmed != null);
		Debug.Assert(theme.TrapDisarmed != null);
		Model1 = theme.TrapArmed;
		Model2 = theme.TrapDisarmed;
		Model3 = theme.TrapWarning;
		Model4 = theme.TrapActive;

		RandomEffect = theme.TrapRandomEffect;
		ActiveEffect = theme.TrapActiveEffect;
		EffectOffset = theme.TrapEffectOffset;
		WarningSound = theme.TrapWarningSound;
		ActivatedSound = theme.TrapActivatedSound;
	}

	[SerializeField]
	private Vector3 _epicenterOffset;
	public override Vector3 Epicenter
	{
		get
		{
			return base.Epicenter + _epicenterOffset;
		}

		set
		{
			base.Epicenter = value + _epicenterOffset;
		}
	}

	internal override IEnumerator HandleStep()
	{
		yield return new WaitForSeconds(TriggerTime);
		if(ActiveEffect != null)
			VisualEffectsManager.PlayEffect(this, ActiveEffect, 7f, TriggerTime, new Vector3(0f, 3f, 0f), Quaternion.Euler(0, 180, 90));
		if(ActivatedSound != null)
			AudioEffectsManager.PlaySound(this,ActivatedSound);
		foreach(AbstractPlayerCharacter player in PlayerManager.GetPlayersNear(Position+Vector3.up, radius:0.25f)) {
			player.TakeDamage(float.PositiveInfinity);
		}
		//Renderer rend = GetComponent<Renderer>();
		//rend.material = DisarmedStateMaterial;
		IsArmed = false;
	}

	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add("Trigger Time (seconds)",   new DefaultValue { TypeInfo = typeof(float), Value = DEFAULT_TRIGGER_TIME.ToString() } );
			ret.Add("Animation Scale",          new DefaultValue { TypeInfo = typeof(float), Value = DEFAULT_ANIMATION_SCALE.ToString() } );
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (TriggerTime != DEFAULT_TRIGGER_TIME)
				ret.Add("Trigger Time (seconds)", TriggerTime.ToString());
			if (AnimationScale != DEFAULT_ANIMATION_SCALE)
				ret.Add("Animation Scale", AnimationScale.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Trigger Time (seconds)"))
				TriggerTime = float.Parse(value["Trigger Time (seconds)"]);
			if (value.ContainsKey("Animation Scale"))
				AnimationScale = float.Parse(value["Animation Scale"]);
		}
	}
}
