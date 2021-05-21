using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private bool defaultToTouchControls;
    [SerializeField]
    private Camera gameplayCam;

    private IInput inputEvents;
    public IInput InputEvents => inputEvents;

    private void Awake()
    {
        if (Input.touchSupported || defaultToTouchControls)
            inputEvents = new TouchInput(gameplayCam);
        else
            inputEvents = new MouseInput(gameplayCam);
    }

    private void Update() => inputEvents.UpdateInputEvents();
}
