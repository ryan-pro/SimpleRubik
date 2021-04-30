using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cublet : MonoBehaviour
{
    private Vector3 originalPosition;

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
