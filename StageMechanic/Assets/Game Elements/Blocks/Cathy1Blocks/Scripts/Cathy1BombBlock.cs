/*  
 * Copyright (C) Catherine. All rights reserved.  
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

    private const float SMALL_BOMB_DEFAULT_FUSE_TIME = 1.5f;
    private const float LARGE_BOMB_DEFAULT_FUSE_TIME = 1.5f;
    private const int SMALL_BOMB_DEFAULT_RADIUS = 1;
    private const int LARGE_BOMB_DEFAULT_RADIUS = 3;
    private const float SMALL_BOMB_DEFAULT_ANIMATION_SCALE = 2f;
    private const float LARGE_BOMB_DEFAULT_ANIMATION_SCALE = 3f;

    public enum BombSize
    {
        Small,
        Large
    }

    public BombSize Size = BombSize.Small;

    private enum State
    {
        NoPlayer = 0,
        PlayerEnter,
        PlayerStand,
        PlayerLeave,
        Disarmed
    }

    private State CurrentState = State.NoPlayer;

    /// <summary>
    /// This class is used for both Bomb1 and Bomb2 types
    /// Setting to a different type should throw an exception
    /// for now just set it to the small one by default
    /// TODO
    /// </summary>
    public sealed override BlockType Type
    {
        get
        {
            if (Size == BombSize.Small)
                return Cathy1Block.BlockType.Bomb1;
            return Cathy1Block.BlockType.Bomb2;
        }
    }


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

    /// <summary>
    /// Sets the trigger time of the spike trap
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        TriggerTime = 1.5f;
        if (Type == BlockType.Bomb1)
            DamageRadius = new Vector3(SMALL_BOMB_DEFAULT_RADIUS, SMALL_BOMB_DEFAULT_RADIUS, SMALL_BOMB_DEFAULT_RADIUS);
        else
            DamageRadius = new Vector3(LARGE_BOMB_DEFAULT_RADIUS, LARGE_BOMB_DEFAULT_RADIUS, LARGE_BOMB_DEFAULT_RADIUS);

    }

    bool hasPlayer()
    {
        Vector3 player = PlayerManager.Player1Location();
        return (player == transform.position + Vector3.up && (PlayerManager.PlayerStateName() == "Idle" || PlayerManager.PlayerStateName() == "Walk" || PlayerManager.PlayerStateName() == "Center"));
    }

    private IEnumerator HandleStep()
    {
        CurrentState = State.PlayerEnter;
        GetComponent<AudioSource>()?.PlayOneShot(FuseSound);
        BlockManager.PlayEffect(this, ExplosionAnimation, ExplosionAnimationScale, TriggerTime);
        yield return new WaitForSeconds(TriggerTime);
        BlockManager.PlaySound(this, ExplosionSound);
        gameObject.SetActive(false);
        if (hasPlayer())
        {
            CurrentState = State.PlayerStand;
            PlayerManager.Player(0).TakeDamage(float.PositiveInfinity);

        }
        List<IBlock> localBlocks = BlockManager.GetBlocskAt(Position, DamageRadius.x);
        foreach(IBlock block in localBlocks)
        {
            //any immobile block will not be affected
            if(block.WeightFactor!=0f && block.GameObject != GameObject)
                BlockManager.CreateBlockAt(block.Position, "Cathy1 Internal", "Cracked (1 Step)");
        }
        CurrentState = State.Disarmed;
        BlockManager.DestroyBlock(this);
    }

    private void Update()
    {
        if (!BlockManager.PlayMode)
            return;
        if (CurrentState == State.PlayerEnter || CurrentState == State.PlayerStand)
            return;
        if (!hasPlayer())
            return;
        if (CurrentState == State.Disarmed)
            return;
        StartCoroutine(HandleStep());
    }

    public override Dictionary<string, KeyValuePair<string, string>> DefaultProperties
    {
        get
        {
            Dictionary<string, KeyValuePair<string, string>> ret = base.DefaultProperties;
            ret.Add("Trigger Time (seconds)", new KeyValuePair<string, string>("float", (Type==BlockType.Bomb1?SMALL_BOMB_DEFAULT_FUSE_TIME:LARGE_BOMB_DEFAULT_FUSE_TIME).ToString()));
            ret.Add("Damage Radius", new KeyValuePair<string, string>("int", (Type == BlockType.Bomb1 ? SMALL_BOMB_DEFAULT_RADIUS : LARGE_BOMB_DEFAULT_RADIUS).ToString()));
            ret.Add("Animation Scale", new KeyValuePair<string, string>("float", (Type == BlockType.Bomb1 ? SMALL_BOMB_DEFAULT_ANIMATION_SCALE : LARGE_BOMB_DEFAULT_ANIMATION_SCALE).ToString()));
            return ret;
        }
    }

    public override Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = base.Properties;
            if (TriggerTime != SMALL_BOMB_DEFAULT_FUSE_TIME && Type==BlockType.Bomb1)
                ret.Add("Trigger Time (seconds)", TriggerTime.ToString());
            if (TriggerTime != LARGE_BOMB_DEFAULT_FUSE_TIME && Type == BlockType.Bomb2)
                ret.Add("Trigger Time (seconds)", TriggerTime.ToString());
            if (Type == BlockType.Bomb1 && DamageRadius != new Vector3(SMALL_BOMB_DEFAULT_RADIUS, SMALL_BOMB_DEFAULT_RADIUS, SMALL_BOMB_DEFAULT_RADIUS))
                ret.Add("Damage Radius", DamageRadius.x.ToString());
            if (Type == BlockType.Bomb2 && DamageRadius != new Vector3(SMALL_BOMB_DEFAULT_RADIUS, LARGE_BOMB_DEFAULT_RADIUS, LARGE_BOMB_DEFAULT_RADIUS))
                ret.Add("Damage Radius", DamageRadius.x.ToString());
            if (Type == BlockType.Bomb1 && ExplosionAnimationScale != SMALL_BOMB_DEFAULT_ANIMATION_SCALE)
                ret.Add("Animation Scale", ExplosionAnimationScale.ToString());
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
        }
    }
}
