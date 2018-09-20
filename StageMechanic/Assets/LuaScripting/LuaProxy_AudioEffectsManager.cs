using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaProxy_AudioEffectsManager 
{
	public void play(AudioClip sound, float volume = 1f)
	{
		AudioEffectsManager.PlaySound(sound, volume);
	}

	public void play(AbstractBlock block, AudioClip sound, float volume = 1f)
	{
		AudioEffectsManager.PlaySound(block, sound, volume);
	}
}
