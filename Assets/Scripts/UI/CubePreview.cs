﻿using Cysharp.Threading.Tasks;
using UnityEngine;

[AddComponentMenu("Smart Magic Cube/UI/Cube Preview")]
public class CubePreview : MonoBehaviour
{
    [Header("Animation Options")]
    [SerializeField]
    private float rotationDuration = 0.5f;
    [SerializeField]
    private AnimationCurve resetAnimationCurve;
    [SerializeField]
    private Animation rotateAnimator;

    [Header("References")]
    [SerializeField]
    private Cube cubeObject;
    [SerializeField]
    private Transform cubeRotator;
    [SerializeField]
    private LoadedCubeData loadedCubeData;

    private void OnEnable()
    {
        loadedCubeData.OnDataUpdated += UpdateCubeWithNewData;
        GenerateNewCube();
    }

    private void OnDisable()
    {
        loadedCubeData.OnDataUpdated -= UpdateCubeWithNewData;
        cubeObject.CleanCube();
    }

    public void GenerateNewCube(bool dontUseLoadedData = false)
    {
        if (!dontUseLoadedData && loadedCubeData.IsDataLoaded)
            cubeObject.CreateCubeFromData(loadedCubeData.ToData());
        else
            cubeObject.CreateNewCube(loadedCubeData.DesiredNewSize);

        foreach (var comp in GetComponentsInChildren<Component>())
            comp.gameObject.layer = gameObject.layer;
    }

    private void UpdateCubeWithNewData(object sender, DataChangedArgs args)
        => GenerateNewCube(args.SizeOnlyChanged);

    public void ResetRotation()
    {
        var curCubeRotation = cubeRotator.localRotation;
        rotateAnimator.Stop();

        SmoothResetRotation(curCubeRotation).Forget();
    }

    private async UniTaskVoid SmoothResetRotation(Quaternion startRotation)
    {
        var elapsed = 0f;

        while(elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            cubeRotator.localRotation = Quaternion.Slerp(startRotation, Quaternion.identity, resetAnimationCurve.Evaluate(elapsed / rotationDuration));

            await UniTask.NextFrame();
        }
    }
}
