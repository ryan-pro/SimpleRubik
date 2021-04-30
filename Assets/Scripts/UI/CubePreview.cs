using Cysharp.Threading.Tasks;
using UnityEngine;

public class CubePreview : MonoBehaviour
{
    [SerializeField]
    private LoadedCubeData loadedCubeData;

    [SerializeField]
    private Cube cubeObject;

    [SerializeField]
    private Transform cubeRotator;
    [SerializeField]
    private Animation rotateAnimator;
    [SerializeField]
    private AnimationCurve resetAnimationCurve;

    private void Start() => GenerateNewCube();
    private void OnEnable() => loadedCubeData.OnDataUpdated += UpdateCubeWithNewData;
    private void OnDisable() => loadedCubeData.OnDataUpdated -= UpdateCubeWithNewData;

    public void GenerateNewCube(bool forceNew = false)
    {
        if (loadedCubeData.IsDataLoaded && !forceNew)
            cubeObject.CreateCubeFromData(loadedCubeData);
        else
            cubeObject.CreateNewCube(loadedCubeData.DesiredNewSize);

        foreach (var comp in GetComponentsInChildren<Component>())
            comp.gameObject.layer = gameObject.layer;
    }

    private void UpdateCubeWithNewData(object sender, System.EventArgs e)
        => GenerateNewCube();

    public void ResetRotation()
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
