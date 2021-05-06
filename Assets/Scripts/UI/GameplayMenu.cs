using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameplayMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject inputCatch;
    [SerializeField]
    private Animation animator;
    [SerializeField]
    private AnimationClip clip;

    [SerializeField]
    private GameFlowManager flowManager;
    [SerializeField]
    private ModalControl quitPrompt;

    private bool isOpen;
    public void SwapMenuStatus()
    {
        SetMenuStatus(!isOpen);
    }

    public void SetMenuStatus(bool open)
    {
        isOpen = open;

        inputCatch.SetActive(isOpen);
        AnimateMenu(isOpen);
    }

    private void AnimateMenu(bool forward)
    {
        animator[clip.name].normalizedTime = forward ? 0f : 1f;
        animator[clip.name].speed = forward ? 1f : -1f;
        animator.Play();
    }

    public void RestartGame() => PrepareRestart().Forget();

    private async UniTaskVoid PrepareRestart()
    {
        if(await quitPrompt.ShowMessage("Are you sure you want to restart?\r\nProgress will be lost.") == ModalResult.Positive)
        {
            SetMenuStatus(false);
            flowManager.RestartGame();
        }
    }

    public void ExitToMenu() => PrepareToExit().Forget();

    private async UniTaskVoid PrepareToExit()
    {
        if(await quitPrompt.ShowMessage("Return to Main Menu?") == ModalResult.Positive)
        {
            SetMenuStatus(false);
            flowManager.EndGame();
        }
    }
}
