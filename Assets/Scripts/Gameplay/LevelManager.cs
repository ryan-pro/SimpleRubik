using Cysharp.Threading.Tasks;
using UnityEngine;

[AddComponentMenu("Simple Magic Cube/Game/Level Manager")]
public class LevelManager : MonoBehaviour
{
    [Header("Scene Objects")]
    [SerializeField]
    private Cube cubeObject;
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private CubeShuffler shuffler;
    [SerializeField]
    private GameWinAnimator winAnimator;

    [Header("Utilities")]
    [SerializeField]
    private GameFlowManager flowManager;
    [SerializeField]
    private LoadedCubeData loadedCubeData;
    [SerializeField]
    private UndoController undoController;
    [SerializeField]
    private Timer timer;

    private CubeData gameStartData;     //Used to persist cube state at game start; unserialized

    private void Awake() => flowManager.SetLevelReference(this);

    public void InitializeNewGame()
    {
        if (loadedCubeData.IsDataLoaded)
        {
            var data = loadedCubeData.ToData();
            cubeObject.CreateCubeFromData(data);
            undoController.SetUndoStack(data.UndoList);
        }
        else
        {
            cubeObject.CreateNewCube(loadedCubeData.DesiredNewSize);
            cubeObject.SetAsInitialState();
            undoController.Clear();
        }

        ResetGameplay();
    }

    public void SetCubeFromStartData()
    {
        if (gameStartData != null)
        {
            cubeObject.CreateCubeFromData(gameStartData);
            undoController.SetUndoStack(gameStartData.UndoList);
        }
    }

    public async UniTask StartGameplay(bool newGame)
    {
        cubeObject.RefreshSpinners();

        if (newGame)
            await shuffler.ShuffleCube(cubeObject);

        gameStartData = cubeObject.GetCurrentCubeState();
        //TODO: Serialize new data

        timer.StartCounting();
        inputController.enabled = true;
    }

    public void OnCubeSolved()
    {
        inputController.enabled = false;
        undoController.Clear();
        timer.StopCounting();

        HandleWinCompletion().Forget();
    }

    private async UniTaskVoid HandleWinCompletion()
    {
        await winAnimator.Animate();
        flowManager.HandleGameWin("Completion Time: " + timer.GetTimeByMinute());
    }

    public void CleanUpLevel()
    {
        StopGameplay();
        cubeObject.CleanCube();
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
