using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cathy1BlockFactory : MonoBehaviour {

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

    public Block CreateBlock(Vector3 pos, Quaternion rotation, Block.BlockType type = Block.BlockType.Basic)
    {
        string oldName = null;
        GameObject oldItem = null;

        GameObject[] collidedGameObjects =
            Physics.OverlapSphere(pos, 0.1f)
                .Except(new[] { GetComponent<Collider>() })
                .Select(c => c.gameObject)
                .ToArray();

        foreach (GameObject obj in collidedGameObjects)
        {
            Block bl = obj.GetComponent(typeof(Block)) as Block;
            if (bl != null)
            {
                oldName = bl.Name;
                if (bl.Item != null)
                {
                    bl.Item.transform.parent = null;
                    oldItem = bl.Item;
                }
                if (bl.Type != type)
                {
                    //TODO make this not shitty hacky type styff
                    bl.Type = type;
                    Block tmp = CreateBlock(new Vector3(-20, -20, -20), rotation, type);
                    bl.GetComponent<Renderer>().material = tmp.GetComponent<Renderer>().material;
                    bl.transform.localScale = tmp.transform.localScale;
                    Destroy(tmp);
                }
                return bl;
            }
        }

        GameObject newBlock = null;

        switch (type)
        {
            case Block.BlockType.Basic:
                newBlock = Instantiate(BlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Ice:
                newBlock = Instantiate(IceBlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Heavy1:
                newBlock = Instantiate(Heavy1BlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Heavy2:
                newBlock = Instantiate(Heavy2BlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Bomb1:
                newBlock = Instantiate(Bomb1BlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Bomb2:
                newBlock = Instantiate(Bomb2BlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Immobile:
                newBlock = Instantiate(ImmobleBlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Fixed:
                newBlock = Instantiate(FixedBlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Spring:
                newBlock = Instantiate(SpringBlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Crack1:
                newBlock = Instantiate(Crack1BlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Crack2:
                newBlock = Instantiate(Crack2BlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Trap1:
                newBlock = Instantiate(Trap1BlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Trap2:
                newBlock = Instantiate(Trap2BlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Vortex:
                newBlock = Instantiate(VortexBlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Monster:
                newBlock = Instantiate(MonsterBlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Teleport:
                newBlock = Instantiate(TeleportBlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Random:
                newBlock = Instantiate(RandomBlockPrefab, pos, rotation) as GameObject;
                break;
            case Block.BlockType.Goal:
                newBlock = Instantiate(GoalBlockPrefab, pos, rotation) as GameObject;
                break;
            default:
                //Create a new block at the cursor position and set it as the active game block
                newBlock = Instantiate(BlockPrefab, pos, rotation) as GameObject;
                break;
        }

        Debug.Assert(newBlock != null);

        Block block = newBlock.GetComponent<Block>();
        Debug.Assert(block != null);
        block.Type = type;
        return block;
    }
}
