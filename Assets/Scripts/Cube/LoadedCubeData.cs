using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Simple Magic Cube/Cube Data")]
public class LoadedCubeData : ScriptableObject
{
    public event System.EventHandler<DataChangedArgs> OnDataUpdated;

    [SerializeField]
    private string serializedFile = "cubeprogress.save";

    private int size;
    private CubeletData[] cubelets;
    private SpinAction[] undoList;

    //public int Size { get; private set; }
    //public CubeletData[] Cubelets { get; private set; }
    //public SpinAction[] UndoList { get; private set; }

    private int desiredNewSize = 3;
    public int DesiredNewSize
    {
        get => desiredNewSize;
        set
        {
            desiredNewSize = value;
            OnDataUpdated?.Invoke(this, new DataChangedArgs { SizeOnlyChanged = true });
        }
    }
    public float DesiredNewSizeFloat
    {
        get => desiredNewSize;
        set
        {
            desiredNewSize = Mathf.RoundToInt(value);
            OnDataUpdated?.Invoke(this, new DataChangedArgs { SizeOnlyChanged = true });
        }
    }

    private bool isDataLoaded = false;
    public bool IsDataLoaded
    {
        get => isDataLoaded;
        set => isDataLoaded = value;
    }

    public CubeData ToData() => new CubeData()
    {
        Size = size,
        Cubelets = cubelets,
        UndoList = undoList
    };

    public void LoadSavedData()
    {
        var loadedData = DeserializeSaveFile();
        if (loadedData == null)
            return;

        size = loadedData.Size;
        cubelets = loadedData.Cubelets;
        undoList = loadedData.UndoList;

        IsDataLoaded = true;
        OnDataUpdated?.Invoke(this, new DataChangedArgs { SizeOnlyChanged = false });
    }

    public void UpdateCurrentData(int newSize, CubeletData[] newCubelets, SpinAction[] newUndos)
    {
        size = newSize;
        cubelets = newCubelets;
        undoList = newUndos;


        SerializeCurrentData(new CubeData()
        {
            Size = size,
            Cubelets = cubelets,
            UndoList = undoList
        });

        IsDataLoaded = true;
        OnDataUpdated?.Invoke(this, new DataChangedArgs { SizeOnlyChanged = false });
    }

    private CubeData DeserializeSaveFile()
    {
        var filePath = Path.Combine(Application.persistentDataPath, serializedFile);

        if (!File.Exists(filePath))
            return null;

        return JsonUtility.FromJson<CubeData>(File.ReadAllText(filePath));
    }

    private void SerializeCurrentData(CubeData toSave)
    {
        var filePath = Path.Combine(Application.persistentDataPath, serializedFile);

        var json = JsonUtility.ToJson(toSave, true);
        File.WriteAllText(filePath, json);
    }
}
