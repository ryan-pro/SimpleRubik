using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Transform camParent;
    [SerializeField]
    private Transform target;

    [Header("Input")]
    [SerializeField]
    private InputController inputController;

    [Header("Options")]
    [SerializeField, Range(10f, 30f)]
    private float distance = 10f;
    [SerializeField, Range(0f, 1f)]
    private float dampener = 0.15f;

    private Transform camTransform;
    private IInput inputEvents;

    private Vector2 inputVector;
    private Vector2 rotVector;

    private bool canRotate;

    private void Awake() => camTransform = cam.transform;

    private void Start()
    {
        camTransform.position = target.position - (Vector3.forward * distance);
        camParent.rotation = Quaternion.Euler(new Vector3(20f, -40f, camParent.rotation.eulerAngles.z));
        camTransform.LookAt(target);
    }

    private void OnEnable()
    {
        if (inputEvents == null)
            inputEvents = inputController.InputEvents;

        inputEvents.ButtonChanged += OnButtonUpdated;
        inputEvents.ScrollChanged += OnScrollUpdated;
    }

    private void OnDisable()
    {
        inputEvents.ButtonChanged -= OnButtonUpdated;
        inputEvents.ScrollChanged -= OnScrollUpdated;
    }

    private void LateUpdate()
    {
        camTransform.localPosition = camParent.localPosition - (Vector3.forward * distance);

        if (canRotate)
        {
            rotVector += inputVector;

            rotVector = new Vector2(Mathf.Lerp(rotVector.x, 0, dampener), Mathf.Lerp(rotVector.y, 0, dampener));
            camParent.eulerAngles += new Vector3(-rotVector.y, rotVector.x, 0f);
        }
    }

    private void OnButtonUpdated(object sender, ButtonEventArgs args)
    {
        if (args.PressedDown && args.Hit.collider == null)
        {
            canRotate = true;
            inputEvents.CursorMoved += OnCursorMoved;
        }
        else
        {
            inputEvents.CursorMoved -= OnCursorMoved;
            canRotate = false;
        }
    }

    private void OnScrollUpdated(object sender, ScrollEventArgs args)
    {

    }

    private void OnCursorMoved(object sender, PositionEventArgs args)
    {
        inputVector = args.Delta;
    }
}
