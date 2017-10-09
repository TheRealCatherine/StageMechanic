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

   Block CreateBlock(Vector3 globalPosition, Quaternion globalRotation, int blockTypeIndex);
   Block CreateBlock(Vector3 globalPosition, Quaternion globalRotation, string blockTypeName);
}
