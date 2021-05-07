using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Simple Magic Cube/Cube/Cube")]
public class Cube : MonoBehaviour
{
    [SerializeField]
    private Cubelet cubeletPrefab;
    [SerializeField]
    private Spinner spinnerPrefab;
    [SerializeField]
    private LoadedCubeData loadedCubeData;
    [SerializeField]
    private UndoController undoController;

    [SerializeField]
    private UnityEvent cubeSolved;

    private int size;
    private Cubelet[] cubelets = new Cubelet[0];
    private Spinner[] spinners = new Spinner[0];

    public Spinner[] Spinners => spinners;

    private void Awake()
    {
        if (undoController != null)
            undoController.SetCube(this);
    }

    public void CreateNewCube(int size)
    {
        InstantiateCubeContents(size);

        //foreach (var c in cubelets)
        //    c.ResetInitialPosition();
    }

    public void CreateCubeFromData(CubeData data)
    {
        CreateNewCube(data.Size);

        for (int i = 0; i < cubelets.Length; i++)
            cubelets[i].SetTransformFromData(data.Cubelets[i]);
    }

    private void InstantiateCubeContents(int size)
    {
        CleanCube();

        var spawnPoint = transform.localPosition - new Vector3(size / 2f, size / 2f, size / 2f);

        cubelets = new Cubelet[size * size * size];
        this.size = size;

        int index = 0;
        for (int z = 0; z < size; z++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    var newCubelet = Instantiate(cubeletPrefab, transform);
                    newCubelet.transform.localPosition = spawnPoint + new Vector3(x, y, z);

                    if (cubelets.Length % 2 > 0 && Mathf.FloorToInt(cubelets.Length / 2f) == index)
                        newCubelet.IsCenter = true;

                    cubelets[index] = newCubelet;
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
            spinner.name = $"{spinner.name} X ({index})";
            spinner.transform.localPosition = spawnPoint + new Vector3(i + 0.5f, size / 2f, size / 2f);
            spinner.transform.Rotate(Vector3.forward, 90f);
            spinners[index++] = spinner;

            spinner = Instantiate(spinnerPrefab, transform);
            spinner.name = $"{spinner.name} Y ({index})";
            spinner.transform.localPosition = spawnPoint + new Vector3(size / 2f, i + 0.5f, size / 2f);
            spinners[index++] = spinner;

            spinner = Instantiate(spinnerPrefab, transform);
            spinner.name = $"{spinner.name} Z ({index})";
            spinner.transform.localPosition = spawnPoint + new Vector3(size / 2f, size / 2f, i + 0.5f);
            spinner.transform.Rotate(Vector3.right, 90f);
            spinners[index++] = spinner;
        }

        foreach(var spin in spinners)
            spin.SetUpPhysics(size);
    }

    public void SetAsInitialState()
    {
        foreach (var c in cubelets)
            c.ResetInitialPosition();
    }

    public CubeData GetCurrentCubeState() => new CubeData()
    {
        Size = size,
        Cubelets = cubelets.Select(a => a.BackUpData()).ToArray(),
        UndoList = undoController.ToArray()
    };

    public void RefreshSpinners()
    {
        foreach (var spin in spinners)
            spin.UpdateCollection();
    }

    public async UniTask Rotate(Spinner chosenSpinner, bool forward, bool fastSpin, bool playerAction)
    {
        var action = await chosenSpinner.Spin(forward, fastSpin);

        foreach (var spin in spinners)
            spin.UpdateCollection();

        if (!playerAction || action == null)
            return;

        undoController.RegisterAction(action);

        if (cubelets.All(a => a.IsInInitialPosition))
            cubeSolved.Invoke();
        else
            BackUpCurrentData();
    }

    public void BackUpCurrentData()
    {
        if (cubelets.Length == 0 || cubelets[0] == null)
            return;

        var dataList = new CubeletData[cubelets.Length];

        for(int i = 0; i < cubelets.Length; i++)
            dataList[i] = cubelets[i].BackUpData();

        loadedCubeData.UpdateCurrentData(size, dataList, undoController.ToArray());
    }

    public void CleanCube()
    {
        if(undoController != null)
            undoController.Clear();

        if (cubelets.Length > 0)
        {
            foreach (var c in cubelets.Where(a => a != null))
                Destroy(c.gameObject);
        }

        if (spinners.Length > 0)
        {
            foreach (var s in spinners.Where(a => a != null))
                Destroy(s.gameObject);
        }
    }
}
