/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using UnityEngine;

public class Cathy1VortexBlock : Cathy1AbstractTrapBlock
{
	public AudioClip IdleSound;
	public AudioClip ActiveSound;
	public ParticleSystem RandomEffect;
	public ParticleSystem ActiveEffect;
	public Vector3 EffectOffset;

	public sealed override TrapBlockType TrapType { get; } = TrapBlockType.Vortex;
    public sealed override float TriggerTime { get; set; } = 0f;

	public override void ApplyTheme(Cathy1BlockTheme theme)
	{
		Debug.Assert(theme.IdleVortex != null);
		Model1 = theme.IdleVortex;
		Model2 = theme.ActiveVortex;

		IdleSound = theme.IdleVortexSound;
		ActiveSound = theme.ActiveVortexSound;
		RandomEffect = theme.RandomVortexEffect;
		ActiveEffect = theme.ActiveVortexEffect;
		EffectOffset = theme.VortexEffectOffset;
	}

	internal override IEnumerator HandleStep()
    {
        IsTriggered = true;
        if(TriggerTime>0)
            yield return new WaitForSeconds(TriggerTime);
		if (ActiveSound != null)
			AudioEffectsManager.PlaySound(this, ActiveSound);
        foreach (AbstractPlayerCharacter player in PlayerManager.GetPlayersNear(Position + Vector3.up, radius: 0.25f))
        {
            player.TakeDamage(float.PositiveInfinity);
        }
        IsArmed = true;
        IsTriggered = false;
    }

    public override void Awake()
    {
        base.Awake();
        TriggerTime = 0f;
    }

    internal override void Update()
    {
        base.Update();
        if (!BlockManager.PlayMode)
            return;
        IBlock onTop = BlockManager.GetBlockNear(transform.position + Vector3.up);
        if (onTop != null) {
            GetComponent<AudioSource>()?.Play();
            BlockManager.DestroyBlock(onTop);
        }

    }
}
