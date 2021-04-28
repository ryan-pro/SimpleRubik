using UnityEngine;

[AddComponentMenu("SimpleMagicCube/Hide on Mobile")]
public class HideOnMobile : MonoBehaviour
{
    private void Awake()
        => gameObject.SetActive(!Application.isMobilePlatform);
}
