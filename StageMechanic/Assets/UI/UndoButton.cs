using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoButton : MonoBehaviour {

	public void Undo()
    {
        BlockManager.Undo();
    }
}
