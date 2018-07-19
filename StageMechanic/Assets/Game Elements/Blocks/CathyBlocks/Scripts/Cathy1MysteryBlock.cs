﻿/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy1MysteryBlock : Cathy1Block
{
	public AudioClip RevealSound;
	public float Delay = DEFAULT_DELAY;
	public const float DEFAULT_DELAY = 0.05f;

	public override void ApplyTheme(Cathy1BlockTheme theme)
	{
		Debug.Assert(theme.IdleMystery != null);
		Model1 = theme.IdleMystery;
		Model2 = theme.RevealingMystery;
		RevealSound = theme.MysteryRevealSound;
	}

	public readonly string[] PossibleTypes = {
		"Basic",
		"Cracked (2 Steps)",
		"Heavy",
		"Small Bomb",
		"Spring",
		"Monster"};

	protected override void OnPlayerEnter(PlayerMovementEvent ev)
	{
		base.OnPlayerEnter(ev);
		StartCoroutine(HandlePlayer(ev));
	}

	//TODO OnPlayerEnter for some reason was not sufficient here, need to figure out why
	//and then probably remove this
	protected override void OnPlayerStay(PlayerMovementEvent ev)
	{
		base.OnPlayerStay(ev);
		StartCoroutine(HandlePlayer(ev));
	}


	bool _started = false;
	virtual internal IEnumerator HandlePlayer(PlayerMovementEvent ev)
	{
		if (ev.Location != PlayerMovementEvent.EventLocation.Top || _started)
			yield break;
		string statename = ev.Player.StateNames[ev.Player.CurrentStateIndex];
		if (statename == "Idle" || statename == "Walk" || statename == "Center")
		{
			_started = true;
			if (RevealSound != null)
				AudioEffectsManager.PlaySound(this, RevealSound);
			yield return new WaitForSeconds(Delay);
			System.Random rnd = new System.Random();
			int index = rnd.Next(PossibleTypes.Length);
			BlockManager.CreateBlockAt(Position, "Cathy1 Internal", PossibleTypes[index]);
		}
	}

	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add("Trigger Time (seconds)", new DefaultValue { TypeInfo = typeof(float), Value = DEFAULT_DELAY.ToString() });
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (Delay != DEFAULT_DELAY)
				ret.Add("Trigger Time (seconds)", Delay.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Trigger Time (seconds)"))
				Delay = float.Parse(value["Trigger Time (seconds)"]);
		}
	}
}
