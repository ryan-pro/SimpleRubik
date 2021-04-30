using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private Cublet cubletPrefab;
    [SerializeField]
    private LoadedCubeData loadedCubeData;
    [SerializeField]
    private bool spawnOnStart = true;

    private int size;
    private Cublet[] cublets = new Cublet[0];
    private Spinner[] spinners = new Spinner[0];

    public Spinner[] Spinners => spinners;

    //private void Start()
    //{
    //    if (spawnOnStart && Application.isEditor)
    //        CreateNewCube(3);
    //}

    public void CreateNewCube(int size)
    {
        CleanCube();

        var spawnPoint = transform.localPosition - new Vector3(size / 2f, size / 2f, size / 2f);

        cublets = new Cublet[size * size * size];
        this.size = size;

        int index = 0;
        for (int z = 0; z < size; z++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    cublets[index] = Instantiate(cubletPrefab, transform);
                    cublets[index].transform.localPosition = spawnPoint + new Vector3(x, y, z);

                    index++;
                }
            }
        }

        //Spinners
        spinners = new Spinner[size * 3];
        index = 0;

        for (int i = 0; i < size; i++)
        {
            var spinner = new GameObject("Spinner A", typeof(Spinner)).GetComponent<Spinner>();
            spinner.transform.parent = transform;
            spinner.transform.localPosition = spawnPoint + new Vector3(i + 0.5f, size / 2f, size / 2f);
            spinner.transform.Rotate(Vector3.up, 90f, Space.Self);
            spinners[index++] = spinner;

            spinner = new GameObject("Spinner B", typeof(Spinner)).GetComponent<Spinner>();
            spinner.transform.parent = transform;
            spinner.transform.localPosition = spawnPoint + new Vector3(size / 2f, i + 0.5f, size / 2f);
            spinner.transform.Rotate(Vector3.right, 90f, Space.Self);
            spinners[index++] = spinner;

            spinner = new GameObject("Spinner C", typeof(Spinner)).GetComponent<Spinner>();
            spinner.transform.parent = transform;
            spinner.transform.localPosition = spawnPoint + new Vector3(size / 2f, size / 2f, i + 0.5f);
            spinner.transform.Rotate(Vector3.forward, 90f, Space.Self);
            spinners[index++] = spinner;
        }
    }

    public void CreateCubeFromData(LoadedCubeData data)
    {
        CreateNewCube(data.Size);

        for (int i = 0; i < cublets.Length; i++)
            cublets[i].SetTransformFromData(data.Cublets[i]);
    }

    public void RotateSpinner(Spinner chosenSpinner, bool forward)
    {
        chosenSpinner.Spin(forward);

        foreach (var spin in spinners)
            spin.UpdateCollection();

        //TODO: Update cublet position data
        //TODO: Check positions for win condition
    }

    private void BackUpCurrentData()
    {
        if (cublets.Length == 0 || cublets[0] == null)
            return;

        var dataList = new CubletData[cublets.Length];

        for(int i = 0; i < cublets.Length; i++)
            dataList[i] = cublets[i].BackUpData();

        //TODO: Pass array to SO
    }

    public void CleanCube()
    {
        if (cublets.Length > 0)
        {
            foreach (var c in cublets)
                Destroy(c.gameObject);
        }

        if (spinners.Length > 0)
        {
            foreach (var s in spinners)
                Destroy(s.gameObject);
        }
    }
}
