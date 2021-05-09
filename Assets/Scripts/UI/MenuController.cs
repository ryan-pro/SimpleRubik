using Cysharp.Threading.Tasks;
using UnityEngine;

[AddComponentMenu("SimpleMagicCube/UI/Menu Controller")]
public class MenuController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField]
    private GameObject menuObject;
    [SerializeField]
    private GameObject gameUIObject;
    [SerializeField]
    private ModalControl gameWinPrompt;
    [SerializeField]
    private ModalControl quitPrompt;

    [Header("Utilities")]
    [SerializeField]
    private GameFlowManager flowManager;
    [SerializeField]
    private AnimatedFader fader;

    [Header("Options")]
    [SerializeField]
    private float fadeDuration = 1f;

    private void Awake() => flowManager.SetMenuReference(this);

    public async UniTask DisplayIntroTransition(bool showMenu)
    {
        menuObject.SetActive(showMenu);
        await fader.FadeIn(fadeDuration);
    }

    public async UniTask DisplayOutroTransition()
    {
        await fader.FadeOut(fadeDuration);
        menuObject.SetActive(false);
    }

    public void SetGameUI(bool active) => gameUIObject.SetActive(active);

    public UniTask<ModalResult> PresentWinPrompt(string message)
        => gameWinPrompt.ShowMessage(message, "Play Again", "Return to Menu");

    public void Quit() => PrepareQuit().Forget();

    private async UniTaskVoid PrepareQuit()
    {
        if (await quitPrompt.ShowMessage("Are you sure you want to quit?") == ModalResult.Positive)
        {
            await fader.FadeOut(fadeDuration);
            Application.Quit();

            if (Application.isEditor)
                Debug.Log("Game has ended!");
        }
    }
}
