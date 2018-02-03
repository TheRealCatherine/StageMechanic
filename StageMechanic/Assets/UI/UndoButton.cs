using UnityEngine;

public class UndoButton : MonoBehaviour {

	public void Undo()
    {
        Serializer.Undo();
    }
}
