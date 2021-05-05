using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Simple Magic Cube/Undo Controller")]
public class UndoController : ScriptableObject
{
    private readonly Stack<SpinAction> undoStack = new Stack<SpinAction>();             //Actions that can be undone
    private readonly Queue<SpinAction> requestedUndoQueue = new Queue<SpinAction>();    //Actions requested to be undone (in case the player rapidly presses the Undo button)
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
                await targetCube.ExecuteRotation(toUndo.UsedSpinner, !toUndo.SpunForward, true, false);
            }

            await UniTask.Yield();
        }
    }

    public void Clear() => undoStack.Clear();

    private void OnDisable()
    {
        backgroundCancellationSource?.Cancel();
        backgroundCancellationSource?.Dispose();
    }
}
