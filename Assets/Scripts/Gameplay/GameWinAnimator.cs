using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[AddComponentMenu("Simple Magic Cube/Game/Game Win Animator")]
public class GameWinAnimator : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField]
    private float zoomDistance = 20f;
    [SerializeField]
    private float zoomDuration = 1f;
    [SerializeField]
    private AnimationCurve zoomCurve;

    [Header("Rotation")]
    [SerializeField]
    private float rotation = 720f;
    [SerializeField]
    private float rotateDuration = 2.5f;
    [SerializeField]
    private AnimationCurve rotationCurve;

    [SerializeField]
    private GameCamera cam;

    public async UniTask Animate(CancellationToken token)
    {
        var startZoom = cam.Distance;

        var elapsed = 0f;
        while (elapsed < zoomDuration && !token.IsCancellationRequested)
        {
            elapsed += Time.deltaTime;
            cam.Distance = Mathf.Lerp(startZoom, zoomDistance, zoomCurve.Evaluate(elapsed / zoomDuration));

            await UniTask.NextFrame();
        }

        if (token.IsCancellationRequested)
            return;

        var startRot = cam.ParentTransform.eulerAngles;
        //var endRot = startRot * Quaternion.Euler(new Vector3(0f, rotation, 0f));

        elapsed = 0f;
        while(elapsed < rotateDuration && !token.IsCancellationRequested)
        {
            elapsed += Time.deltaTime;

            var rot = Mathf.Lerp(startRot.y, rotation + startRot.y, rotationCurve.Evaluate(elapsed / rotateDuration));
            cam.ParentTransform.rotation = Quaternion.Euler(startRot.x, rot, 0f);

            await UniTask.NextFrame();
        }
    }
}
