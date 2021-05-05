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

    private void Awake() => flowManager.SetLevelReference(this);

    public void InitializeNewGame()
    {
        if (loadedCubeData.IsDataLoaded)
            cubeObject.CreateCubeFromData(loadedCubeData);
        else
            cubeObject.CreateNewCube(loadedCubeData.DesiredNewSize);

        ResetGameplay();
    }

    public void OnCubeSolved()
    {
        inputController.enabled = false;
        undoController.Clear();
        timer.StopCounting();

        HandleWinCompletion().Forget();
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

    private async UniTaskVoid HandleWinCompletion()
    {
        await winAnimator.Animate(this.GetCancellationTokenOnDestroy());

        //TODO: Show congrats dialog, passing timer string
        Debug.Log("You solved the cube!");
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
