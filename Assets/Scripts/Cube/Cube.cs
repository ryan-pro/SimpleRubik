using UnityEngine;
using Cysharp.Threading.Tasks;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private Cublet cubletPrefab;
    [SerializeField]
    private Spinner spinnerPrefab;
    [SerializeField]
    private LoadedCubeData loadedCubeData;

    private int size;
    private Cublet[] cublets = new Cublet[0];
    private Spinner[] spinners = new Spinner[0];

    public Spinner[] Spinners => spinners;

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
                    var newCublet = Instantiate(cubletPrefab, transform);
                    newCublet.transform.localPosition = spawnPoint + new Vector3(x, y, z);

                    if (cublets.Length % 2 > 0 && Mathf.FloorToInt(cublets.Length / 2f) == index)
                        newCublet.IsCenter = true;

                    cublets[index] = newCublet;
                    index++;
                }
            }
        }

        //Spinners
        if (spinnerPrefab == null)
            return;

        spinners = new Spinner[size * 3];
        index = 0;

        for (int i = 0; i < size; i++)
        {
            var spinner = Instantiate(spinnerPrefab, transform);
            spinner.transform.localPosition = spawnPoint + new Vector3(i + 0.5f, size / 2f, size / 2f);
            spinner.transform.Rotate(Vector3.up, 90f, Space.Self);
            spinners[index++] = spinner;

            spinner = Instantiate(spinnerPrefab, transform);
            spinner.transform.localPosition = spawnPoint + new Vector3(size / 2f, i + 0.5f, size / 2f);
            spinner.transform.Rotate(Vector3.right, 90f, Space.Self);
            spinners[index++] = spinner;

            spinner = Instantiate(spinnerPrefab, transform);
            spinner.transform.localPosition = spawnPoint + new Vector3(size / 2f, size / 2f, i + 0.5f);
            spinners[index++] = spinner;
        }

        foreach(var spin in spinners)
            spin.SetUpPhysics(size);
    }

    public void CreateCubeFromData(LoadedCubeData data)
    {
        CreateNewCube(data.Size);

        for (int i = 0; i < cublets.Length; i++)
            cublets[i].SetTransformFromData(data.Cublets[i]);
    }

    public void RefreshSpinners()
    {
        foreach (var spin in spinners)
            spin.UpdateCollection();
    }

    public async UniTaskVoid RotateSpinner(Spinner chosenSpinner, bool forward)
    {
        await chosenSpinner.Spin(forward);

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
