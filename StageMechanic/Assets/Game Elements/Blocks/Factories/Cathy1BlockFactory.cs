/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Cathy1BlockFactory : MonoBehaviour, IBlockFactory
{

    public GameObject BlockPrefab;
    public GameObject SpikeTrapPrefab;
    public GameObject SpringBlockPrefab;
    public GameObject MonsterBlockPrefab;
    public GameObject IceBlockPrefab;
    public GameObject VortexBlockPrefab;
    public GameObject Bomb1BlockPrefab;
    public GameObject Bomb2BlockPrefab;
    public GameObject Crack1BlockPrefab;
    public GameObject Crack2BlockPrefab;
    public GameObject Heavy1BlockPrefab;
    public GameObject ImmobleBlockPrefab;
    public GameObject RandomBlockPrefab;
    public GameObject GoalBlockPrefab;

    public Sprite BasicIcon;
    public Sprite TrapIcon;
    public Sprite SpringIcon;
    public Sprite MonsterIcon;
    public Sprite IceIcon;
    public Sprite VortexIcon;
    public Sprite Bomb1Icon;
    public Sprite Bomb2Icon;
    public Sprite Crack1Icon;
    public Sprite Crack2Icon;
    public Sprite HeavyIcon;
    public Sprite ImmobileIcon;
    public Sprite MysterIcon;
    public Sprite GoalIcon;

    public int BlockTypeCount
    {
        get
        {
            return BlockTypeNames.Count();
        }
    }

    public static readonly List<string> _blockTypeNames = new List<string>{
        "Custom",
        "Basic",
        "Immobile",
        "Cracked (2 Steps)",
        "Cracked (1 Step)",
        "Heavy",
        "Spike Trap",
        "Ice",
        "Small Bomb",
        "Large Bomb",
        "Spring",
        "Mystery",
        "Monster",
        "Vortex",
        "Goal"
    };

    public List<string> BlockTypeNames
    {
        get
        {
            return _blockTypeNames;
        }
    }

    public Sprite IconForType(string name)
    {
        switch(name)
        {
            case "Basic":
                return BasicIcon;
            case "Spike Trap":
                return TrapIcon;
            case "Spring":
                return SpringIcon;
            case "Monster":
                return MonsterIcon;
            case "Ice":
                return IceIcon;
            case "Vortex":
                return VortexIcon;
            case "Small Bomb":
                return Bomb1Icon;
            case "Large Bomb":
                return Bomb2Icon;
            case "Cracked (1 Step)":
                return Crack1Icon;
            case "Cracked (2 Steps)":
                return Crack2Icon;
            case "Heavy":
                return HeavyIcon;
            case "Immobile":
                return ImmobileIcon;
            case "Mystery":
                return MysterIcon;
            case "Goal":
                return GoalIcon;
            default:
                return null;
        }
    }

    public static Cathy1Block.BlockType TypeForName(string name)
    {
        switch (name)
        {

            case "Basic":
                return Cathy1Block.BlockType.Basic;
            case "Spike Trap":
                return Cathy1Block.BlockType.SpikeTrap;
            case "Spring":
                return Cathy1Block.BlockType.Spring;
            case "Monster":
                return Cathy1Block.BlockType.Monster;
            case "Ice":
                return Cathy1Block.BlockType.Ice;
            case "Vortex":
                return Cathy1Block.BlockType.Vortex;
            case "Small Bomb":
                return Cathy1Block.BlockType.Bomb1;
            case "Large Bomb":
                return Cathy1Block.BlockType.Bomb2;
            case "Cracked (1 Step)":
                return Cathy1Block.BlockType.Crack1;
            case "Cracked (2 Steps)":
                return Cathy1Block.BlockType.Crack2;
            case "Heavy":
                return Cathy1Block.BlockType.Heavy;
            case "Immobile":
                return Cathy1Block.BlockType.Immobile;
            case "Mystery":
                return Cathy1Block.BlockType.Random;
            case "Goal":
                return Cathy1Block.BlockType.Goal;
            case "Custom":
            default:
                return Cathy1Block.BlockType.Custom;
        }
    }

    public static string NameForType(Cathy1Block.BlockType type)
    {
        switch (type)
        {

            case Cathy1Block.BlockType.Basic:
                return "Basic";
            case Cathy1Block.BlockType.SpikeTrap:
                return "Spike Trap";
            case Cathy1Block.BlockType.Spring:
                return "Spring";
            case Cathy1Block.BlockType.Monster:
                return "Monster";
            case Cathy1Block.BlockType.Ice:
                return "Ice";
            case Cathy1Block.BlockType.Vortex:
                return "Vortex";
            case Cathy1Block.BlockType.Bomb1:
                return "Small Bomb";
            case Cathy1Block.BlockType.Bomb2:
                return "Large Bomb";
            case Cathy1Block.BlockType.Crack1:
                return "Cracked (1 Step)";
            case Cathy1Block.BlockType.Crack2:
                return "Cracked (2 Steps)";
            case Cathy1Block.BlockType.Heavy:
                return "Heavy";
            case Cathy1Block.BlockType.Immobile:
                return "Immobile";
            case Cathy1Block.BlockType.Random:
                return "Mystery";
            case Cathy1Block.BlockType.Goal:
                return "Goal";
            case Cathy1Block.BlockType.Custom:
            default:
                return "Custom";
        }
    }
    //TODO fix support for changing block types at a location
    public IBlock CreateBlock(Vector3 pos, Quaternion rotation, Cathy1Block.BlockType type = Cathy1Block.BlockType.Basic, GameObject parent = null)
    {
        string oldName = String.Empty;
        GameObject oldItem = null;

        GameObject[] collidedGameObjects =
            Physics.OverlapSphere(pos, 0.1f)
                .Except(new[] { GetComponent<BoxCollider>() })
                .Select(c => c.gameObject)
                .ToArray();


        foreach (GameObject obj in collidedGameObjects)
        {
            Cathy1Block bl = obj.GetComponent<Cathy1Block>();
            if (bl != null)
            {
                if (parent == null)
                    parent = bl.Parent.gameObject;
                oldName = bl.Name;
                oldItem = bl.FirstItem;
                if (oldItem != null)
                    oldItem.transform.parent = null;
                BlockManager.DestroyBlock(bl);
            }

        }

        GameObject newBlock = null;

        switch (type)
        {
            case Cathy1Block.BlockType.Basic:
                newBlock = Instantiate(BlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Ice:
                newBlock = Instantiate(IceBlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Heavy:
                newBlock = Instantiate(Heavy1BlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Bomb1:
                newBlock = Instantiate(Bomb1BlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Bomb2:
                newBlock = Instantiate(Bomb2BlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Immobile:
                newBlock = Instantiate(ImmobleBlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Spring:
                newBlock = Instantiate(SpringBlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Crack1:
                newBlock = Instantiate(Crack1BlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Crack2:
                newBlock = Instantiate(Crack2BlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.SpikeTrap:
                newBlock = Instantiate(SpikeTrapPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Vortex:
                newBlock = Instantiate(VortexBlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Monster:
                newBlock = Instantiate(MonsterBlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Random:
                newBlock = Instantiate(RandomBlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            case Cathy1Block.BlockType.Goal:
                newBlock = Instantiate(GoalBlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
            default:
                //Create a new block at the cursor position and set it as the active game block
                newBlock = Instantiate(BlockPrefab, pos, rotation, parent.transform) as GameObject;
                break;
        }

        Debug.Assert(newBlock != null);
        Cathy1Block block = newBlock.GetComponent<Cathy1Block>();
        Debug.Assert(block != null);
        block.Parent = parent;
        if (oldName != String.Empty)
            block.Name = oldName;
        block.FirstItem = oldItem;
        block.BlockManager = GetComponent<BlockManager>();
        return block;
    }

    public IBlock CreateBlock(Vector3 globalPosition, Quaternion globalRotation, int blockTypeIndex, GameObject parent)
    {
        return CreateBlock(globalPosition, globalRotation, BlockTypeNames.ElementAtOrDefault(blockTypeIndex), parent);
    }

    public IBlock CreateBlock(Vector3 globalPosition, Quaternion globalRotation, string blockTypeName, GameObject parent)
    {
        Cathy1Block.BlockType type = Cathy1Block.BlockType.Custom;
        try
        {
            type = TypeForName(blockTypeName);
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.Message);
        }
        return CreateBlock(globalPosition, globalRotation, type, parent);
    }
}
