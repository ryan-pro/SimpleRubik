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

    public void ExitToMenu() => PrepareToExit().Forget();

    private async UniTaskVoid PrepareToExit()
    {
        if(await quitPrompt.ShowMessage("Return to Main Menu?"))
        {
            flowManager.EndGame();
            Debug.Log("Returning to menu.");
        }
    }
}
