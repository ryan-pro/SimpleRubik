using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("SimpleMagicCube/UI/Modal Control")]
public class ModalControl : MonoBehaviour
{
    [SerializeField]
    private GameObject parentObject;
    [SerializeField]
    private TextMeshProUGUI messageText;
    [SerializeField]
    private Button yesButton, noButton, backgroundButton;

    public UniTask<bool> ShowMessage(string message)
    {
        parentObject.SetActive(true);
        messageText.text = message;

        var tcs = new UniTaskCompletionSource<bool>();
        yesButton.onClick.AddListener(() => OnChoiceSelected(tcs, true));
        noButton.onClick.AddListener(() => OnChoiceSelected(tcs, false));
        backgroundButton.onClick.AddListener(() => OnChoiceSelected(tcs, false));

        return tcs.Task;
    }

    private void OnChoiceSelected(UniTaskCompletionSource<bool> tcs, bool choice)
    {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        backgroundButton.onClick.RemoveAllListeners();

        parentObject.SetActive(false);
        tcs.TrySetResult(choice);
    }
}
