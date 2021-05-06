using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("SimpleMagicCube/UI/Modal Control")]
public class ModalControl : MonoBehaviour
{
    [SerializeField]
    private GameObject parentObject;

    [Header("Text Objects")]
    [SerializeField]
    private TextMeshProUGUI messageText;
    [SerializeField]
    private TextMeshProUGUI positiveText;
    [SerializeField]
    private TextMeshProUGUI negativeText;

    [Header("Buttons")]
    [SerializeField]
    private Button positiveButton;
    [SerializeField]
    private Button negativeButton;
    [SerializeField]
    private Button backgroundButton;

    public UniTask<ModalResult> ShowMessage(string message) => ShowMessage(message, "Yes", "No");

    public UniTask<ModalResult> ShowMessage(string message, string positiveMessage, string negativeMessage)
    {
        parentObject.SetActive(true);
        messageText.text = message;
        positiveText.text = positiveMessage;
        negativeText.text = negativeMessage;

        var tcs = new UniTaskCompletionSource<ModalResult>();
        positiveButton.onClick.AddListener(() => OnChoiceSelected(tcs, ModalResult.Positive));
        negativeButton.onClick.AddListener(() => OnChoiceSelected(tcs, ModalResult.Negative));
        backgroundButton.onClick.AddListener(() => OnChoiceSelected(tcs, ModalResult.Cancelled));

        return tcs.Task;
    }

    private void OnChoiceSelected(UniTaskCompletionSource<ModalResult> tcs, ModalResult choice)
    {
        positiveButton.onClick.RemoveAllListeners();
        negativeButton.onClick.RemoveAllListeners();
        backgroundButton.onClick.RemoveAllListeners();

        parentObject.SetActive(false);
        tcs.TrySetResult(choice);
    }
}
