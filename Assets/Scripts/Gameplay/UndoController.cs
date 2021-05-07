using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Simple Magic Cube/Undo Controller")]
public class UndoController : ScriptableObject
{
    //Actions that can be undone
    private Stack<SpinAction> undoStack = new Stack<SpinAction>();

    //Actions requested to be undone (in case the player rapidly presses the Undo button)
    private readonly Queue<SpinAction> requestedUndoQueue = new Queue<SpinAction>();

    private Cube targetCube;

    private UniTask backgroundTask;
    private CancellationTokenSource backgroundCancellationSource;

    public int UndoCount => undoStack.Count;

    public void SetCube(Cube cube) => targetCube = cube;
    public void RegisterAction(SpinAction newAction) => undoStack.Push(newAction);

    public void Undo()
    {
        if (undoStack.Count == 0)
            return;

        requestedUndoQueue.Enqueue(undoStack.Pop());

        if (backgroundCancellationSource == null)
        {
            backgroundCancellationSource = new CancellationTokenSource();
            backgroundTask = HandleUndoQueue(backgroundCancellationSource.Token);
        }
    }

    private async UniTask HandleUndoQueue(CancellationToken token)
    {
        while(Application.isPlaying && !token.IsCancellationRequested)
        {
            while(requestedUndoQueue.Count > 0)
            {
                var toUndo = requestedUndoQueue.Dequeue();
                await targetCube.Rotate(toUndo.SpinnerObject, !toUndo.Forward, true, false);
            }

            await UniTask.Yield();
        }
    }

    public void SetUndoStack(SpinAction[] undoActions)
    {
        foreach(var action in undoActions)
        {
            if (action.SpinnerObject == null)
                action.SpinnerObject = System.Array.Find(FindObjectsOfType<Spinner>(), a => a.name == action.SpinnerName);
        }

        undoStack = new Stack<SpinAction>(undoActions);
    }

    public SpinAction[] ToArray()
    {
        var result = undoStack.ToArray();

        foreach(var action in result)
        {
            if(action.SpinnerObject != null)
                action.SpinnerName = action.SpinnerObject.name;
        }

        return result;
    }

    public void Clear() => undoStack.Clear();

    private void OnDisable()
    {
        backgroundCancellationSource?.Cancel();
        backgroundCancellationSource?.Dispose();
    }
}
