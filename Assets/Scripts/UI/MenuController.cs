using UnityEngine;

[AddComponentMenu("SimpleMagicCube/UI/Menu Controller")]
public class MenuController : MonoBehaviour
{
    [SerializeField]
    private ModalControl quitPrompt;

    private void DisplayOutroTransition()
    {
        //TODO: Make awaitable
        //Fade-outs, animations, etc. for showing a fade-to-game
    }

    public async void PrepareQuit()
    {
        if (await quitPrompt.ShowMessage("Are you sure you want to quit?"))
        {
            Application.Quit();
            Debug.Log("Game has ended!");
        }
    }
}
