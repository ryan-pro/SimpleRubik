using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cublets;

    public GameObject[] Cublets => cublets;

    private void Start()
    {
        UpdateCollection();
    }

    public void Spin(bool forward)
    {
        var trueParent = cublets[0].transform.parent;

        foreach(var c in cublets)
            c.transform.parent = transform;

        transform.Rotate(Vector3.forward, forward ? 90f : -90f, Space.Self);

        foreach (var c in cublets)
            c.transform.parent = trueParent;
    }

    public void UpdateCollection()
    {
        var hits = Physics.BoxCastAll(transform.position, new Vector3(2, 2, 0.1f), transform.forward, transform.rotation, 0.1f);
        cublets = hits.Select(a => a.collider.gameObject).Where(b => b.CompareTag("Cube")).ToArray();
    }
}
