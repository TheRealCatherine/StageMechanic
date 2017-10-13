using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockFactory  {
    
   int BlockTypeCount
    {
        get;
    }

   List<string> BlockTypeNames
    {
        get;
    }

   IBlock CreateBlock(Vector3 globalPosition, Quaternion globalRotation, int blockTypeIndex, GameObject parent = null);
   IBlock CreateBlock(Vector3 globalPosition, Quaternion globalRotation, string blockTypeName, GameObject parent = null);
}
