using Cysharp.Threading.Tasks;
using UnityEngine;

public class CubePreview : MonoBehaviour
{
    [SerializeField]
    private CubeData loadedCubeData;
    [SerializeField]
    private Transform cubeParent, cubeRotator;
    [SerializeField]
    private Animation rotateAnimator;
    [SerializeField]
    private AnimationCurve resetAnimationCurve;

    private void OnEnable() => loadedCubeData.OnDataUpdated += UpdateCubeWithNewData;
    private void OnDisable() => loadedCubeData.OnDataUpdated -= UpdateCubeWithNewData;

    private void UpdateCubeWithNewData(object sender, System.EventArgs e)
    {
        //TODO: Recreate cube with new data
        throw new System.NotImplementedException();
    }

    public void DebugResetRotation()
    {
        var curCubeRotation = cubeRotator.localRotation;
        rotateAnimator.Stop();

        SmoothResetRotation(curCubeRotation).Forget();
    }

    private async UniTaskVoid SmoothResetRotation(Quaternion startRotation)
    {
        var elapsed = 0f;
        float testDuration = 0.5f;

        while(elapsed < testDuration)
        {
            elapsed += Time.deltaTime;
            cubeRotator.localRotation = Quaternion.Slerp(startRotation, Quaternion.identity, resetAnimationCurve.Evaluate(elapsed / testDuration));

            await UniTask.NextFrame();
        }
    }
}
