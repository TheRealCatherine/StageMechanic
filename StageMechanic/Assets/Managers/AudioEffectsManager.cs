﻿/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
 using System.Collections;
using UnityEngine;

public class AudioEffectsManager : MonoBehaviour {

    public static AudioEffectsManager Instance;
    public AudioSource MainCamera;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator _soundHelper(AudioClip clip, Vector3 position, float volume)
    {
        //AudioSource.PlayClipAtPoint(clip, position, volume);
        //TODO figure out why the above is OMFG quiet AF
        MainCamera.GetComponent<AudioSource>().PlayOneShot(clip, volume);
        yield return new WaitForSeconds(clip.length);
    }

    public static void PlaySound(IBlock block, AudioClip sound, float volume = 1f)
    {
        Debug.Assert(block != null);
        Debug.Assert(sound != null);
        Instance.StartCoroutine(Instance._soundHelper(sound, block.Position, volume));
    }
}