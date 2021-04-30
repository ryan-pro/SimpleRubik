using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Simple Magic Cube/Undo Controller")]
public class UndoController : ScriptableObject
{
    private Stack<UndoAction> undoStack = new Stack<UndoAction>();

    public int UndoCount => undoStack.Count;

    public void Undo()
    {
        if (undoStack.Count == 0)
            return;
    }
}
