using UnityEngine;

[AddComponentMenu("Simple Magic Cube/Cube/Cubelet")]
public class Cubelet : MonoBehaviour
{
    private Vector3 originalLocalPosition;

    public bool IsCenter { get; set; }
    public bool IsInInitialPosition => transform.localPosition == originalLocalPosition;

    public void ResetInitialPosition()
        => originalLocalPosition = transform.localPosition;

    public void SetTransformFromData(CubeletData savedData)
    {
        originalLocalPosition = savedData.OriginalLocalPosition;
        transform.localPosition = savedData.MostRecentLocalPosition;
        transform.localRotation = savedData.MostRecentWorldRotation;
    }

    public CubeletData BackUpData()
    {
        return new CubeletData()
        {
            OriginalLocalPosition = originalLocalPosition,
            MostRecentLocalPosition = transform.localPosition,
            MostRecentWorldRotation = transform.rotation
        };
    }
}
