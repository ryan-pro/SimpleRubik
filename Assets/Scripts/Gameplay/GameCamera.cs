using Cysharp.Threading.Tasks;
using System.Linq;
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
    [SerializeField]
    private float zoomSpeed = 8f;
    [SerializeField, Range(0f, 1f)]
    private float dampener = 0.15f;

    private Transform camTransform;
    private IInput inputEvents;

    private Vector2 inputVector;
    private Vector2 rotVector;

    private bool canRotate;

    public Transform ParentTransform => camParent.transform;

    public float Distance
    {
        get => distance;
        set => distance = value;
    }

    private void Awake() => camTransform = cam.transform;

    private async UniTaskVoid Start()
    {
        camTransform.position = target.position - (Vector3.forward * distance);
        camParent.rotation = Quaternion.Euler(new Vector3(20f, -40f, camParent.rotation.eulerAngles.z));
        camTransform.LookAt(target);

        while (inputController.InputEvents == null)
            await UniTask.Yield();

        inputEvents = inputController.InputEvents;

        inputEvents.ButtonChanged -= OnButtonUpdated;
        inputEvents.ButtonChanged += OnButtonUpdated;

        inputEvents.ScrollChanged -= OnScrollUpdated;
        inputEvents.ScrollChanged += OnScrollUpdated;
    }

    private void OnEnable()
    {
        if (inputEvents == null)
            return;

        inputEvents.ButtonChanged -= OnButtonUpdated;
        inputEvents.ButtonChanged += OnButtonUpdated;

        inputEvents.ScrollChanged -= OnScrollUpdated;
        inputEvents.ScrollChanged += OnScrollUpdated;
    }

    private void OnDisable()
    {
        if (inputEvents == null)
            return;

        inputEvents.ButtonChanged -= OnButtonUpdated;
        inputEvents.ScrollChanged -= OnScrollUpdated;
    }

    private void LateUpdate()
    {
        camTransform.localPosition = Vector3.MoveTowards(camTransform.localPosition, camParent.localPosition - (Vector3.forward * distance), zoomSpeed * Time.deltaTime);

        if (canRotate)
        {
            rotVector += inputVector;

            rotVector = Vector2.Lerp(rotVector, Vector2.zero, dampener);
            camParent.eulerAngles += new Vector3(-rotVector.y, rotVector.x, 0f);
        }
    }

    private void OnButtonUpdated(object sender, ButtonEventArgs args)
    {
        if (args.PressedDown && args.Hits.All(a => a.collider == null))
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
        var newDistance = distance - (args.ScrollValue * zoomSpeed);
        distance = Mathf.Clamp(newDistance, 10f, 30f);
    }

    private void OnCursorMoved(object sender, PositionEventArgs args)
    {
        inputVector = args.Delta;
    }
}
