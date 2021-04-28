using UnityEngine;

[AddComponentMenu("SimpleMagicCube/UI/Menu Controller")]
public class MenuController : MonoBehaviour
{
    public void StartNewGame(int cubeSize)
    {
        DisplayOutroTransition();

        //Either find a way to communicate with the game (manager) or reconsider separate scenes
        //Alternatively, ScriptableObject game/scene manager
    }

    public void ContinueGame()
    {
        DisplayOutroTransition();

        //Same comments as above
    }

    private void DisplayOutroTransition()
    {
        //TODO: Make awaitable
        //Fade-outs, animations, etc. for showing a fade-to-game
    }

    public void PrepareQuit()
    {
        //Modal confirmation here
        Application.Quit();
    }
}
