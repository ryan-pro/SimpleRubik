using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private Camera gameplayCam;

    private IInput inputEvents;
    public IInput InputEvents => inputEvents;

    private void Awake()
    {
        if (Input.touchSupported)
            inputEvents = new MouseInput(gameplayCam);  //TODO: Replace with touch version
        else
            inputEvents = new MouseInput(gameplayCam);
    }

    private void Update() => inputEvents.UpdateInputEvents();
}
