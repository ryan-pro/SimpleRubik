using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class CubeShuffler : MonoBehaviour
{
    [SerializeField]
    private int shuffleCount = 10;

    private Queue<SpinAction> shuffleQueue;

    private void Awake() => shuffleQueue = new Queue<SpinAction>(shuffleCount);

    public UniTask ShuffleCube(Cube cubeObject)
    {
        FillStack(cubeObject);
        return ProcessStack(cubeObject, this.GetCancellationTokenOnDestroy());
    }

    private async UniTask ProcessStack(Cube cubeObject, CancellationToken token)
    {
        while (shuffleQueue.Count > 0)
        {
            if (!Application.isPlaying || token.IsCancellationRequested)
                return;

            var curAction = shuffleQueue.Dequeue();
            await cubeObject.ExecuteRotation(curAction.UsedSpinner, curAction.SpunForward, true, false);
        }
    }

    private void FillStack(Cube cubeObject)
    {
        shuffleQueue.Clear();

        var spinnerList = cubeObject.Spinners.Where(a => a.Cubelets.All(b => !b.IsCenter)).ToList();

        while(shuffleQueue.Count < shuffleCount)
        {
            var curSpinner = spinnerList[Random.Range(0, spinnerList.Count - 1)];
            spinnerList.Remove(curSpinner);
            spinnerList.Add(curSpinner);

            shuffleQueue.Enqueue(new SpinAction
            {
                UsedSpinner = curSpinner,
                SpunForward = System.Convert.ToBoolean(Random.Range(0, 2))
            });
        }
    }
}
