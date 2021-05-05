using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    private float rotationDuration = 0.5f;
    [SerializeField]
    private float fastRotationDuration = 0.2f;
    [SerializeField]
    private AnimationCurve rotationAnimCurve;

    [SerializeField]
    private Cubelet[] cubelets;
    private int size;

    public Cubelet[] Cubelets => cubelets;
    public bool ContainsCenter => cubelets.Any(a => a.IsCenter);

    public void SetUpPhysics(int size)
    {
        this.size = size;
        GetComponent<BoxCollider>().size = new Vector3(size + 0.1f, 1f, size + 0.1f);
    }

    public void UpdateCollection()
    {
        var hits = Physics.BoxCastAll(transform.position, new Vector3(size, 0.2f, size) / 2f, transform.forward, transform.rotation, 0.1f);
        cubelets = hits.Where(a => a.collider.CompareTag("Cube")).Select(b => b.collider.GetComponentInParent<Cubelet>()).ToArray();
    }

    public async UniTask<SpinAction> Spin(bool forward, bool fastSpin)
    {
        if(ContainsCenter)
        {
            Debug.Log("Can't rotate this spinner; center cube is unmovable.");
            return null;
        }

        var trueParent = cubelets[0].transform.parent;

        foreach (var c in cubelets)
            c.transform.parent = transform;

        var startRot = transform.localRotation;
        var endRot = startRot * Quaternion.Euler(new Vector3(0f, forward ? 90f : -90f, 0f));

        var duration = fastSpin ? fastRotationDuration : rotationDuration;
        var elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            transform.localRotation = Quaternion.Lerp(startRot, endRot, rotationAnimCurve.Evaluate(elapsed / duration));
            await UniTask.NextFrame();
        }

        foreach (var c in cubelets)
            c.transform.parent = trueParent;


        return new SpinAction()
        {
            UsedSpinner = this,
            SpunForward = forward
        };
    }
}
