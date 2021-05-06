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

    public void HandleGameWin(string message) => PresentWinScreen(message).Forget();

    private async UniTaskVoid PresentWinScreen(string message)
    {
        //Positive = restart, negative = to main menu, cancelled = hide menu
        var result = await menuController.PresentWinPrompt(message);

        if (result == ModalResult.Positive)
            await ResetLevel();
        else if (result == ModalResult.Negative)
            await TransitionToMenu();
    }

    public void RestartGame() => ResetLevel().Forget();

    private async UniTask ResetLevel()
    {
        await menuController.DisplayOutroTransition();

        levelManager.CleanUpLevel();
        levelManager.InitializeNewGame();
        levelManager.SetCubeFromStartData();

        await menuController.DisplayIntroTransition(false);
        await levelManager.StartGameplay(false);
    }

    public void EndGame() => TransitionToMenu().Forget();

    public async UniTask TransitionToMenu()
    {
        await menuController.DisplayOutroTransition();

        levelManager.CleanUpLevel();
        menuController.SetGameUI(false);

        await menuController.DisplayIntroTransition(true);
    }
}
