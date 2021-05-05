using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Simple Magic Cube/Cube Data")]
public class LoadedCubeData : ScriptableObject
{
    public event System.EventHandler OnDataUpdated;

    [SerializeField]
    private string serializedFile = "cubeprogress.save";

    public int Size { get; private set; }
    public CubeletData[] Cubelets { get; private set; }
    public SpinAction[] UndoList { get; private set; }

    private int desiredNewSize = 3;
    public int DesiredNewSize
    {
        get => desiredNewSize;
        set
        {
            desiredNewSize = value;
            OnDataUpdated?.Invoke(this, System.EventArgs.Empty);
        }
    }
    public float DesiredNewSizeFloat
    {
        get => desiredNewSize;
        set
        {
            desiredNewSize = Mathf.RoundToInt(value);
            OnDataUpdated?.Invoke(this, System.EventArgs.Empty);
        }
    }

    private bool isDataLoaded = false;
    public bool IsDataLoaded
    {
        get => isDataLoaded;
        set => isDataLoaded = value;
    }

    public void LoadSavedData()
    {
        var loadedData = DeserializeSaveFile();
        if (loadedData == null)
            return;

        Size = loadedData.Size;
        Cubelets = loadedData.Cubelets;
        //TODO: Undos

        IsDataLoaded = true;
        OnDataUpdated?.Invoke(this, System.EventArgs.Empty);
    }

    public void UpdateCurrentData(int size, CubeletData[] newData)
    {
        Size = size;
        Cubelets = newData;
        //TODO: Get undos


        SerializeCurrentData(new CubeData
        {
            Size = Size,
            Cubelets = Cubelets
            //TODO: undos
        });

        IsDataLoaded = true;
        OnDataUpdated?.Invoke(this, System.EventArgs.Empty);
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
        //File.WriteAllText(filePath, json);
    }
}
