using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private UndoController controller;

    private void Update() => button.interactable = controller.UndoCount > 0;
}
