using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("SimpleMagicCube/Game Initializer")]
public class GameInitializer : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField]
    private string menuSceneName;
    [SerializeField]
    private string gameplaySceneName;

    [Header("Data")]
    [SerializeField]
    private GameFlowManager flowManager;
    [SerializeField]
    private LoadedCubeData cubeData;

    private void Awake() => OnGameLaunch().Forget();

    private async UniTaskVoid OnGameLaunch()
    {
        var sceneLoader = LoadScenes();
        cubeData.LoadSavedData();

        await sceneLoader;
        await flowManager.LaunchMenu();
    }

    private UniTask LoadScenes()
    {
        var menuLoader = SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
        var gameplayLoader = SceneManager.LoadSceneAsync(gameplaySceneName, LoadSceneMode.Additive);

        return UniTask.WhenAll(menuLoader.ToUniTask(), gameplayLoader.ToUniTask());
    }
}
