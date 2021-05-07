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
        var validHits = args.Hits.Where(a => a.collider != null);

        if (args.PressedDown && validHits.Any())
        {
            var cursorWorldPos = gameplayCam.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, 1f));

            potentialSpinners = validHits
                .Where(a => a.collider.CompareTag(spinnerTag))
                .OrderBy(b => (b.point - cursorWorldPos).sqrMagnitude)
                .Select(c => c.collider.GetComponent<Spinner>())
                .Take(2).ToList();

            startPos = args.ScreenPosition;
            inputEvents.CursorMoved += OnCursorMoved;

            wasPressed = true;
            return;
        }
        else if (!args.PressedDown && wasPressed)
        {
            DetermineCubeletGroup(args.ScreenPosition);

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

            var spinnerHits = secondHits
                .Where(a => a.collider != null && a.collider.CompareTag(spinnerTag))
                .OrderBy(b => (b.point - cursorWorldPos).sqrMagnitude).Take(2);

            foreach (var spin in potentialSpinners)
            {
                if (spinnerHits.All(a => a.transform != spin.transform))
                {
                    potentialSpinners.Remove(spin);
                    break;
                }
            }
        }
    }

    private void DetermineCubeletGroup(Vector2 endPoint)
    {
        var heading = startPos - endPoint;

        if (heading.sqrMagnitude < dragDistanceThreshold)
            return;

        var direction = heading.normalized;

        if (potentialSpinners.Count == 1)
            cubeObject.Rotate(potentialSpinners[0], IsPositive(direction), false, true).Forget();
        else
            Debug.Log("Couldn't determine spinner.");

        CleanUpInputData();
    }

    private bool IsPositive(Vector3 dir) => dir.x > 0.6f || dir.y > 0.6f || dir.z > 0.6f;

    private void CleanUpInputData()
    {
        startPos = Vector2.zero;
        potentialSpinners.Clear();
    }
}
