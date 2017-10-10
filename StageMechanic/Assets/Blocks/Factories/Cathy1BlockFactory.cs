using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cathy1BlockFactory : MonoBehaviour, IBlockFactory
{

    public GameObject BlockPrefab;
    public GameObject Trap1BlockPrefab;
    public GameObject Trap2BlockPrefab;
    public GameObject SpringBlockPrefab;
    public GameObject MonsterBlockPrefab;
    public GameObject IceBlockPrefab;
    public GameObject VortexBlockPrefab;
    public GameObject Bomb1BlockPrefab;
    public GameObject Bomb2BlockPrefab;
    public GameObject Crack1BlockPrefab;
    public GameObject Crack2BlockPrefab;
    public GameObject TeleportBlockPrefab;
    public GameObject Heavy1BlockPrefab;
    public GameObject Heavy2BlockPrefab;
    public GameObject ImmobleBlockPrefab;
    public GameObject FixedBlockPrefab;
    public GameObject RandomBlockPrefab;
    public GameObject GoalBlockPrefab;



    public int BlockTypeCount
    {
        get
        {
            return BlockTypeNames.Count();
        }
    }

    public static readonly List<string> _blockTypeNames = new List<string>{
        "Custom",
        "Basic",			//Typical block
		"Trap1",			//Spike Trap
		"Trap2",			//??? Trap
		"Spring",			//Teleport variation, moves character up along edge
		"Monster",		    //Teleport variation, moves character down from edge
		"Ice",			    //Teleport variation, moves charcter along top of block
		"Vortex",			//Vortex trap
		"Bomb1",			//Bomb with short timing
		"Bomb2",			//Bomb with long timing
		"Crack1",			//Can step on once
		"Crack2",			//Can step on twice
		"Teleport",		    //Moves character from one block to another
		"Heavy1",			//Similar to Basic but slower to move
		"Heavy2",			//Even slower to move than Heavy1
		"Immobile",		    //Basic blocks that cannot normally be moved by the player
		"Fixed",			//Basic blocks that are fixed in space, cannot be moved no matter what
		"Random",			//Not a fixed type, one of a selectable subset
		"Goal"			    //Level completion zone
	};

    public List<string> BlockTypeNames
    {
        get
        {
            return _blockTypeNames;
        }
    }

    //TODO fix support for changing block types at a location
    public IBlock CreateBlock(Vector3 pos, Quaternion rotation, Cathy1Block.BlockType type = Cathy1Block.BlockType.Basic, GameObject parent = null)
    {
        string oldName = null;
        List<GameObject> oldItems = new List<GameObject>();

        GameObject[] collidedGameObjects =
            Physics.OverlapSphere(pos, 0.1f)
                .Except(new[] { GetComponent<Collider>() })
                .Select(c => c.gameObject)
                .ToArray();

        foreach (GameObject obj in collidedGameObjects)
        {
           IBlock bl = obj.GetComponent<IBlock>();
            if (bl != null)
            {
                oldName = bl.Name;
                foreach(GameObject item in ((IBlock)bl).Items)
                {
                    if (item != null)
                    {
                        item.transform.parent = null;
                        //TODO don't copy list
                        oldItems.Add(item);
                    }
                }
                if (bl.TypeName != type.ToString())
                {
                    //TODO make this not shitty hacky type styff
                    bl.TypeName = type.ToString();
                    Cathy1Block tmp = CreateBlock(new Vector3(-20, -20, -20), rotation, type) as Cathy1Block;
                    bl.GameObject.GetComponent<Renderer>().material = tmp.GetComponent<Renderer>().material;
                    bl.GameObject.transform.localScale = tmp.transform.localScale;
                    Destroy(tmp.GameObject);
                }
                bl.Parent = parent;
                return bl;
            }
        }

        GameObject newBlock = null;

        switch (type)
        {
            case Cathy1Block.BlockType.Basic:
                newBlock = Instantiate(BlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Ice:
                newBlock = Instantiate(IceBlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Heavy1:
                newBlock = Instantiate(Heavy1BlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Heavy2:
                newBlock = Instantiate(Heavy2BlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Bomb1:
                newBlock = Instantiate(Bomb1BlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Bomb2:
                newBlock = Instantiate(Bomb2BlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Immobile:
                newBlock = Instantiate(ImmobleBlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Fixed:
                newBlock = Instantiate(FixedBlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Spring:
                newBlock = Instantiate(SpringBlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Crack1:
                newBlock = Instantiate(Crack1BlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Crack2:
                newBlock = Instantiate(Crack2BlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Trap1:
                newBlock = Instantiate(Trap1BlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Trap2:
                newBlock = Instantiate(Trap2BlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Vortex:
                newBlock = Instantiate(VortexBlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Monster:
                newBlock = Instantiate(MonsterBlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Teleport:
                newBlock = Instantiate(TeleportBlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Random:
                newBlock = Instantiate(RandomBlockPrefab, pos, rotation) as GameObject;
                break;
            case Cathy1Block.BlockType.Goal:
                newBlock = Instantiate(GoalBlockPrefab, pos, rotation) as GameObject;
                break;
            default:
                //Create a new block at the cursor position and set it as the active game block
                newBlock = Instantiate(BlockPrefab, pos, rotation) as GameObject;
                break;
        }

        Debug.Assert(newBlock != null);

        Cathy1Block block = newBlock.GetComponent<Cathy1Block>();
        Debug.Assert(block != null);
        block.Type = type;
        block.Parent = parent;
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
            type = (Cathy1Block.BlockType)Enum.Parse(typeof(Cathy1Block.BlockType), blockTypeName);
            Debug.Assert(Enum.IsDefined(typeof(Cathy1Block.BlockType), type));
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.Message);
        }
        return CreateBlock(globalPosition, globalRotation, type, parent);
    }
}
