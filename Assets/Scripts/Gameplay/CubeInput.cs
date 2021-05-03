using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Cube cubeObject;
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private Camera gameplayCam;

    [Header("Options")]
    [SerializeField]
    private string spinnerTag = "Spinner";
    [SerializeField, Range(0.01f, 1f)]
    private float dragDistanceThreshold = 1f;

    private IInput inputEvents;

    private bool wasPressed;
    private Vector2 startPos;
    private readonly RaycastHit[] secondHits = new RaycastHit[20];
    private List<Spinner> potentialSpinners = new List<Spinner>(2);

    private async UniTaskVoid Start()
    {
        while (inputController.InputEvents == null)
            await UniTask.Yield();

        inputEvents = inputController.InputEvents;
        inputEvents.ButtonChanged -= OnButtonUpdated;
        inputEvents.ButtonChanged += OnButtonUpdated;
    }

    private void OnEnable()
    {
        if (inputEvents == null)
            return;

        inputEvents.ButtonChanged -= OnButtonUpdated;
        inputEvents.ButtonChanged += OnButtonUpdated;
    }

    private void OnDisable()
    {
        if (inputEvents != null)
            inputEvents.ButtonChanged -= OnButtonUpdated;
    }

    private void OnButtonUpdated(object sender, ButtonEventArgs args)
    {
        var validHits = args.Hits.Where(a => a.collider != null && a.collider.CompareTag(spinnerTag));

        if (args.PressedDown && validHits.Any())
        {
            var cursorWorldPos = gameplayCam.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, 1f));

            potentialSpinners = validHits
                .OrderBy(a => (a.point - cursorWorldPos).sqrMagnitude)
                .Select(b => b.collider.GetComponent<Spinner>())
                .Take(2).ToList();

            startPos = args.ScreenPosition;
            inputEvents.CursorMoved += OnCursorMoved;

            wasPressed = true;
            return;
        }
        else if (!args.PressedDown && wasPressed)
        {
            DetermineCubletGroup(args.ScreenPosition);

            inputEvents.CursorMoved -= OnCursorMoved;
            wasPressed = false;
            return;
        }

        wasPressed = false;
    }

    private void OnCursorMoved(object sender, PositionEventArgs args)
    {
        if (!wasPressed || potentialSpinners.Count < 2)
            return;

        System.Array.Clear(secondHits, 0, secondHits.Length);

        if(Physics.RaycastNonAlloc(gameplayCam.ScreenPointToRay(args.Position), secondHits) > 0)
        {
            var cursorWorldPos = gameplayCam.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, 1f));

            var validHits = secondHits
                .Where(a => a.collider != null && a.collider.CompareTag(spinnerTag))
                .OrderBy(b => (b.point - cursorWorldPos).sqrMagnitude).Take(2);

            foreach(var spin in potentialSpinners)
            {
                if (validHits.All(a => a.transform != spin.transform))
                {
                    potentialSpinners.Remove(spin);
                    break;
                }
            }
        }
    }

    private void DetermineCubletGroup(Vector2 endPoint)
    {
        var startWorldPoint = gameplayCam.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, 10f));
        var endWorldPoint = gameplayCam.ScreenToWorldPoint(new Vector3(endPoint.x, endPoint.y, 10f));
        var heading = endWorldPoint - startWorldPoint;

        if (heading.sqrMagnitude < dragDistanceThreshold)
            return;

        var direction = heading.normalized;

        if (potentialSpinners.Count == 1)
            cubeObject.RotateSpinner(potentialSpinners[0], IsPositive(direction)).Forget();
        else
            Debug.Log("Couldn't determine spinner.");
    }

    private bool IsPositive(Vector3 dir) => dir.x > 0.6f || dir.y > 0.6f || dir.z > 0.6f;
}
