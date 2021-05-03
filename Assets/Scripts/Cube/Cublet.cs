using UnityEngine;

public class Cublet : MonoBehaviour
{
    private Vector3 originalPosition;

    [SerializeField]
    private bool isCenter;

    public bool IsCenter
    {
        get => isCenter;
        set => isCenter = value;
    }

    public void SetTransformFromData(CubletData savedData)
    {
        originalPosition = savedData.OriginalLocalPosition;
        transform.localPosition = savedData.MostRecentLocalPosition;
        transform.localRotation = savedData.MostRecentWorldRotation;
    }

    public CubletData BackUpData()
    {
        return new CubletData()
        {
            OriginalLocalPosition = originalPosition,
            MostRecentLocalPosition = transform.localPosition,
            MostRecentWorldRotation = transform.rotation
        };
    }
}
