using UnityEngine;

[System.Serializable]
public class CubeletData
{
    public Vector3 OriginalLocalPosition;
    public Vector3 MostRecentLocalPosition;

    public Quaternion MostRecentWorldRotation;
}
