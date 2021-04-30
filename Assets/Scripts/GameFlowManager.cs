using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Simple Magic Cube/Game Flow Manager")]
public class GameFlowManager : ScriptableObject
{
    private MenuController menuController;
    private LevelManager levelManager;

    public void SetMenuReference(MenuController menu) => menuController = menu;
    public void SetLevelReference(LevelManager level) => levelManager = level;

    public async UniTask LaunchMenu() => await menuController.DisplayIntroTransition(true);

    public void InitiateGame(bool newGame) => TransitionToGame(newGame).Forget();
    private async UniTaskVoid TransitionToGame(bool newGame)
    {
        await menuController.DisplayOutroTransition();

        menuController.SetGameUI(true);
        levelManager.InitializeNewGame();

        await menuController.DisplayIntroTransition(false);
        await levelManager.StartGameplay(newGame);
    }

    public void EndGame() => TransitionToMenu().Forget();

    public async UniTaskVoid TransitionToMenu()
    {
        await menuController.DisplayOutroTransition();

        levelManager.CleanUpLevel();
        menuController.SetGameUI(false);

        await menuController.DisplayIntroTransition(true);
    }
}
