using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("SimpleMagicCube/UI/Loaded Data Checker")]
public class LoadedDataChecker : MonoBehaviour
{
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [SerializeField]
    private ScriptableObject placeholderDataObject;
    [SerializeField]
    private BoolEvent onDataStatusChanged;

    private void OnEnable()
    {
        //TODO: Check if deserialized data is loaded, set Interactable value
        onDataStatusChanged.Invoke(false);  //Temp value
    }

    public void UpdateDataStatus(bool loaded)
    {
        //TODO: Set interactable value based on parameter
        //TODO: Subscribe this to serializer event for auto updates
        //Maybe interface?

        onDataStatusChanged.Invoke(loaded);
    }
}
