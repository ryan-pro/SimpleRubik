using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    private float rotationDuration = 0.5f;
    [SerializeField]
    private AnimationCurve rotationAnimCurve;

    [SerializeField]
    private Cublet[] cublets;
    private int size;

    public Cublet[] Cublets => cublets;
    public bool ContainsCenter => cublets.Any(a => a.IsCenter);

    public void SetUpPhysics(int size)
    {
        this.size = size;
        GetComponent<BoxCollider>().size = new Vector3(size + 0.1f, size + 0.1f, 1f);
    }

    public void UpdateCollection()
    {
        var hits = Physics.BoxCastAll(transform.position, new Vector3(size, size, 0.2f) / 2f, transform.forward, transform.rotation, 0.1f);
        cublets = hits.Where(a => a.collider.CompareTag("Cube")).Select(b => b.collider.GetComponentInParent<Cublet>()).ToArray();
    }

    public async UniTask Spin(bool forward)
    {
        if(ContainsCenter)
        {
            Debug.Log("Can't rotate this spinner; center cube is unmovable.");
            return;
        }

        var trueParent = cublets[0].transform.parent;

        foreach(var c in cublets)
            c.transform.parent = transform;

        var startRot = transform.localRotation;
        var startEuler = startRot.eulerAngles;
        var endRot = Quaternion.Euler(startEuler.x, startEuler.y, startEuler.z + (forward ? 90f : -90f));

        var elapsed = 0f;
        while(elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;

            transform.localRotation = Quaternion.Lerp(startRot, endRot, rotationAnimCurve.Evaluate(elapsed / rotationDuration));
            await UniTask.NextFrame();
        }

        foreach (var c in cublets)
            c.transform.parent = trueParent;
    }
}
