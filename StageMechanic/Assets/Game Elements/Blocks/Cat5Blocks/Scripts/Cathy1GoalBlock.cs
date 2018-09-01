/*  
 * Copyright (C) 2018 You're Perfect Studio. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Cathy1GoalBlock : Cathy1Block
{
	public ParticleSystem IdleEffect;
	public ParticleSystem ActiveEffect;
	public Vector3 EffectOffset;
	public AudioClip Applause;
	public string NextStageFilename;
	public string NextStageStartPos;
	public bool MustNotFall = false;

	public override void ApplyTheme(Cathy1BlockTheme theme)
	{
		Debug.Assert(theme.IdleGoal != null);
		Model1 = theme.IdleGoal;
		Model2 = theme.ActiveGoal;
		IdleEffect = theme.IdleGoalEffect;
		ActiveEffect = theme.ActiveGoalEffect;
		EffectOffset = theme.GoalEffectsOffset;
		Applause = theme.GoalSound;
	}

	public override void Awake()
	{
		base.Awake();
		//Make block immobile
		WeightFactor = 0f;
	}

	bool _hasPlayedSound = false;
	virtual internal void HandlePlayer(PlayerMovementEvent ev)
	{
		if (ev.Location != PlayerMovementEvent.EventLocation.Top)
			return;
		string statename = ev.Player.StateNames[ev.Player.CurrentStateIndex];
		if (statename == "Idle" || statename == "Walk" || statename == "Center")
		{
			if (!_hasPlayedSound)
			{
				if (Applause != null)
					AudioEffectsManager.PlaySound(this, Applause);
				_hasPlayedSound = true;
			}

			if (Input.anyKey)
			{
				if (!string.IsNullOrWhiteSpace(NextStageFilename) && PlayerPrefs.HasKey("LastLoadDir"))
				{
					Uri location = new Uri(PlayerPrefs.GetString("LastLoadDir") + "/" + NextStageFilename);
					Debug.Log("loading " + location.ToString());
					BlockManager.Instance.TogglePlayMode();
					if (Serializer.UseBinaryFiles)
					{
						string loc = PlayerPrefs.GetString("LastLoadDir") + "/" + NextStageFilename;
						loc = loc.Replace(".json", ".bin");
						if (Application.platform == RuntimePlatform.Android)
						{
							BlockManager.Instance.StartCoroutine(BlockManager.DelayTogglePlayMode(0.5f));
							Serializer.BlocksFromBinaryStream(BetterStreamingAssets.ReadAllBytes(loc), true);
						}
						else
							Serializer.BlocksFromBinaryStream(File.ReadAllBytes(loc), true);
					}
					Serializer.BlocksFromJson(location,startPlayMode:true);
				}
				else if(string.IsNullOrWhiteSpace(NextStageFilename)) {
					Debug.Log("No next level specified");
				}
				else if(!PlayerPrefs.HasKey("LastLoadDir"))
				{
					Debug.Log("Unknown file location");
				}
			}
		}
	}

	protected override void OnPlayerEnter(PlayerMovementEvent ev)
	{
		base.OnPlayerEnter(ev);
		ShowModel(2);
		HandlePlayer(ev);
	}

	protected override void OnPlayerStay(PlayerMovementEvent ev)
	{
		base.OnPlayerStay(ev);
		ShowModel(2);
		HandlePlayer(ev);
	}

	protected override void OnPlayerLeave(PlayerMovementEvent ev)
	{
		base.OnPlayerLeave(ev);
		ShowModel(1);
		_hasPlayedSound = false;
	}

	protected override void OnMotionStateChanged(BlockMotionState newState, BlockMotionState oldState)
	{
		base.OnMotionStateChanged(newState, oldState);
		if (MustNotFall && newState == BlockMotionState.Falling)
		{
			PlayerManager.DestroyAllPlayers();
			UIManager.ShowSinglePlayerDeathDialog();
		}

	}

	public override Dictionary<string, DefaultValue> DefaultProperties
	{
		get
		{
			Dictionary<string, DefaultValue> ret = base.DefaultProperties;
			ret.Add("Next Stage Filename",                  new DefaultValue { TypeInfo = typeof(string),   Value = string.Empty });
			ret.Add("Next Stage Start Block Override",      new DefaultValue { TypeInfo = typeof(string),   Value = string.Empty });
			ret.Add("Must Not Fall",                        new DefaultValue { TypeInfo = typeof(bool),     Value = "False" });
			return ret;
		}
	}

	public override Dictionary<string, string> Properties
	{
		get
		{
			Dictionary<string, string> ret = base.Properties;
			if (!string.IsNullOrEmpty(NextStageFilename) && !string.IsNullOrWhiteSpace(NextStageFilename))
				ret.Add("Next Stage Filename", NextStageFilename);
			if (!string.IsNullOrEmpty(NextStageStartPos) && !string.IsNullOrWhiteSpace(NextStageStartPos))
				ret.Add("Next Stage Start Block Override", NextStageStartPos);
			if (MustNotFall)
				ret.Add("Must Not Fall", true.ToString());
			return ret;
		}
		set
		{
			base.Properties = value;
			if (value.ContainsKey("Next Stage Filename"))
				NextStageFilename = value["Next Stage Filename"];
			if (value.ContainsKey("Next Stage Start Block Override"))
				NextStageStartPos = value["Next Stage Start Block Override"];
			if (value.ContainsKey("Must Not Fall"))
				MustNotFall = bool.Parse(value["Must Not Fall"]);
		}
	}
}
