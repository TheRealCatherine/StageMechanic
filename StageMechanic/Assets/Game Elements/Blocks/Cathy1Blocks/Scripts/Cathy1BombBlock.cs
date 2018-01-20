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

    private const float SmallBombFuseTime = 1.5f;
    private const float LargeBombFuseTime = 1.5f;
    private const int SmallBombRadius = 1;
    private const int LargeBombRadius = 3;

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
            DamageRadius = new Vector3(SmallBombRadius, SmallBombRadius, SmallBombRadius);
        else
            DamageRadius = new Vector3(LargeBombRadius, LargeBombRadius, LargeBombRadius);
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
        yield return new WaitForSeconds(TriggerTime);
        GetComponent<AudioSource>()?.PlayOneShot(ExplosionSound);
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
        BlockManager.DestoryBlock(this);
    }

    private void Update()
    {
        if (!BlockManager.PlayMode)
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
            ret.Add("Trigger Time (seconds)", new KeyValuePair<string, string>("float", (Type==BlockType.Bomb1?SmallBombFuseTime:LargeBombFuseTime).ToString()));
            ret.Add("Damage Radius", new KeyValuePair<string, string>("int", (Type == BlockType.Bomb1 ? SmallBombRadius : LargeBombRadius).ToString()));
            return ret;
        }
    }

    public override Dictionary<string, string> Properties
    {
        get
        {
            Dictionary<string, string> ret = base.Properties;
            if (TriggerTime != SmallBombFuseTime && Type==BlockType.Bomb1)
                ret.Add("Trigger Time (seconds)", TriggerTime.ToString());
            if (TriggerTime != LargeBombFuseTime && Type == BlockType.Bomb2)
                ret.Add("Trigger Time (seconds)", TriggerTime.ToString());
            if (Type == BlockType.Bomb1 && DamageRadius != new Vector3(SmallBombRadius, SmallBombRadius, SmallBombRadius))
                ret.Add("Damage Radius", DamageRadius.x.ToString());
            if (Type == BlockType.Bomb2 && DamageRadius != new Vector3(SmallBombRadius, LargeBombRadius, LargeBombRadius))
                ret.Add("Damage Radius", DamageRadius.x.ToString());
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
        }
    }
}
