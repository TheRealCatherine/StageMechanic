/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Rendering.PostProcessing;

public class VisualEffectsManager : MonoBehaviour {

    public static VisualEffectsManager Instance;
    public PostProcessingBehaviour PostProcessor;
    public PostProcessingProfile DefaultProfile;
    public PostProcessingProfile MotionDebutProfile;
    public PostProcessLayer MainStageProcessLayer;
    public ParticleSystem Fog;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnablePostProcessing(PlayerPrefs.GetInt("PostProcessing", 1) == 1);
        EnableMotionDebug(PlayerPrefs.GetInt("MotionDebug", 0) == 1);
    }

    private IEnumerator _particleAnimationHelper(Vector3 position, ParticleSystem animationPrefab, float scale, float duration, Quaternion rotation)
    {
        ParticleSystem system = Instantiate(animationPrefab, position, rotation, BlockManager.Instance.Stage.transform);
        ParticleSystem.MainModule module = system.main;
        if (scale != 1.0f)
        {
            module.scalingMode = ParticleSystemScalingMode.Hierarchy;
            system.transform.localScale = new Vector3(scale, scale, scale);
        }
        if (duration > 0)
            module.simulationSpeed = (module.duration / duration);

        system.Play();
        yield return new WaitForSeconds(system.main.duration);
        system.Stop();
        Destroy(system.gameObject);
    }

    public static void PlayEffect(IBlock block, ParticleSystem animationPrefab, float scale = 1f, float duration = -1f, Vector3 offset = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        Debug.Assert(animationPrefab != null);
        if (duration == 0 || scale == 0)
            return;
        Quaternion rot = rotation;
        if (rotation != Quaternion.identity)
        {
            Vector3 lookDirection = (block.Position + offset) - block.Position;
            rot = Quaternion.LookRotation(lookDirection);
            rot *= rotation;
        }
        Instance.StartCoroutine(Instance._particleAnimationHelper(block.Position + offset, animationPrefab, scale, duration, rot));
    }
    
    public static void EnableFog(bool show)
    {
        Instance?.Fog?.gameObject.SetActive(show);
    }

    public static void EnablePostProcessing(bool process)
    {
        Instance.MainStageProcessLayer.enabled = process;
        Instance.PostProcessor.enabled = process;
    }

    public static void EnableMotionDebug(bool show)
    {
        if (show)
            Instance.PostProcessor.profile = Instance.MotionDebutProfile;
        else
            Instance.PostProcessor.profile = Instance.DefaultProfile;
    }
}
