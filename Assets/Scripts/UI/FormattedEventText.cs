using TMPro;
using UnityEngine;

[AddComponentMenu("SimpleMagicCube/UI/Formatted Event Text")]
public class FormattedEventText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textObject;

    [SerializeField, TextArea(1, 5)]
    [Tooltip("Areas marked \"{0}\" will be replaced with specified input.")]
    private string formattableText = "{0}x{0}x{0}";

    public void SetText(string input)
        => textObject.text = formattableText.Replace("{0}", input);

    public void SetText(float input)
        => textObject.text = formattableText.Replace("{0}", Mathf.RoundToInt(input).ToString());
}
