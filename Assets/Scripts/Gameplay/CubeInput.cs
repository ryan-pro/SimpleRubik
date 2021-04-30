using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

public class CubeInput : MonoBehaviour
{
    [SerializeField]
    private string cubeTag;
    [SerializeField]
    private Cube cubeObject;
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private Camera gameplayCam;

    private IInput inputEvents;

    private bool wasPressed;
    private Vector2 startPos;
    private Transform hitCublet;

    private async UniTaskVoid Start()
    {
        while (inputController.InputEvents == null)
            await UniTask.Yield();

        inputEvents = inputController.InputEvents;
        inputEvents.ButtonChanged += OnButtonUpdated;
    }

    private void OnEnable()
    {
        if (inputEvents != null)
            inputEvents.ButtonChanged += OnButtonUpdated;
    }

    private void OnDisable()
    {
        if (inputEvents != null)
            inputEvents.ButtonChanged -= OnButtonUpdated;
    }

    private void DetermineCubletGroup(Vector2 endPoint)
    {
        var firstWorldPoint = gameplayCam.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, 10f));
        var secondWorldPoint = gameplayCam.ScreenToWorldPoint(new Vector3(endPoint.x, endPoint.y, 10f));
        var direction = (secondWorldPoint - firstWorldPoint).normalized;
        Debug.Log(direction);

        Transform secondCublet = null;
        if (Physics.Raycast(hitCublet.transform.position, direction, out var hit, 1f) && hit.collider.gameObject.CompareTag(cubeTag))
            secondCublet = hit.transform;
        else if (Physics.Raycast(hitCublet.transform.position, direction * -1, out var reverseHit, 1f) && reverseHit.collider.gameObject.CompareTag(cubeTag))
            secondCublet = reverseHit.transform;
        else
            throw new System.ArgumentNullException("No second cublet found!");

        var spinner = System.Array.Find(cubeObject.Spinners, a => a.Cublets.Contains(hitCublet.gameObject) && a.Cublets.Contains(secondCublet.gameObject));

        if (spinner == null)
            throw new System.ArgumentNullException("No appropriate spinner found!");

        //cubeObject.RotateSpinner(spinner, IsPositive(direction));
    }

    private bool IsPositive(Vector3 dir)
    {
        if(dir.x > 0.6f || dir.y > 0.6f || dir.z > 0.6f)
        {
            Debug.Log("Positive");
            return true;
        }
        else
        {
            Debug.Log("Negative");
            return false;
        }
    }

    private void OnButtonUpdated(object sender, ButtonEventArgs args)
    {
        if (args.PressedDown && args.Hit.collider != null && args.Hit.collider.CompareTag(cubeTag))
        {
            startPos = args.ScreenPosition;
            hitCublet = args.Hit.transform;
            //Debug.Log("Touched cube!");

            wasPressed = true;
            return;
        }
        else if (!args.PressedDown && wasPressed)
        {
            //Debug.Log("Released.");
            DetermineCubletGroup(args.ScreenPosition);

            wasPressed = false;
            return;
        }

        wasPressed = false;
    }
}
