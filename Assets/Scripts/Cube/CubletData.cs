using UnityEngine;

[System.Serializable]
public class CubletData
{
    public Vector3 OriginalLocalPosition;
    public Vector3 MostRecentLocalPosition;

    public Quaternion MostRecentWorldRotation;
}
