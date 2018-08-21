/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using UnityEngine;

public class Cathy1IceBlock : Cathy1Block
{

	public ParticleSystem RandomEffect;
	public Vector3 EffectOffset;

	public override void ApplyTheme(Cathy1BlockTheme theme)
	{
		Debug.Assert(theme.Ice != null);
		Model1 = theme.Ice;
		RandomEffect = theme.RandomIceEffect;
		EffectOffset = theme.IceEffectOffset;
	}

	protected override void OnPlayerEnter(PlayerMovementEvent ev)
	{
		base.OnPlayerEnter(ev);
		StartCoroutine(HandlePlayer(ev));
	}

	protected override void OnPlayerStay(PlayerMovementEvent ev)
	{
		base.OnPlayerStay(ev);
		StartCoroutine(HandlePlayer(ev));
	}

	protected override void OnPlayerLeave(PlayerMovementEvent ev)
	{
		base.OnPlayerLeave(ev);
		_started = false;
		_shouldSlide = false;
	}

	bool _started = false;
	bool _shouldSlide = false;
	virtual internal IEnumerator HandlePlayer(PlayerMovementEvent ev)
	{
		if (ev.Location != PlayerMovementEvent.EventLocation.Top || _started)
			yield break;
		string statename = ev.Player.StateNames[ev.Player.CurrentStateIndex];
		if (statename == "Walk" || statename == "Slide")
		{
			_shouldSlide = true;
		}
		else if(statename == "Idle" && _shouldSlide)
		{
			_started = true;
			PlayerManager.Player1SlideForward();
		}
	}
}
