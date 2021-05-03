using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Scene Objects")]
    [SerializeField]
    private Cube cubeObject;
    [SerializeField]
    private InputController inputController;

    [Header("Utilities")]
    [SerializeField]
    private GameFlowManager flowManager;
    [SerializeField]
    private LoadedCubeData loadedCubeData;
    [SerializeField]
    private Timer timer;

    private void Awake() => flowManager.SetLevelReference(this);

    public void InitializeNewGame()
    {
        if (loadedCubeData.IsDataLoaded)
            cubeObject.CreateCubeFromData(loadedCubeData);
        else
            cubeObject.CreateNewCube(loadedCubeData.DesiredNewSize);

        ResetGameplay();
    }

    public void CleanUpLevel()
    {
        StopGameplay();
        cubeObject.CleanCube();
    }

    private UniTask TriggerNewGameShuffle()
    {

        //TODO: Shuffle the cube
        return UniTask.Delay(1000);
    }

    public async UniTask StartGameplay(bool newGame)
    {
        cubeObject.RefreshSpinners();

        if (newGame)
            await TriggerNewGameShuffle();

        timer.StartCounting();
        inputController.enabled = true;
    }

    public void StopGameplay()
    {
        inputController.enabled = false;
        timer.StartCounting();
    }

    private void ResetGameplay()
    {
        inputController.enabled = false;

        timer.StopCounting();
        timer.Restart();
    }
}
