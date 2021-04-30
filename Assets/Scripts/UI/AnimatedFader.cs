using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedFader : MonoBehaviour
{
    [SerializeField]
    private Image faderImage;

    public UniTask FadeIn(float duration)
        => Fade(Color.black, Color.clear, Mathf.Max(0.001f, duration));

    public UniTask FadeOut(float duration)
        => Fade(Color.clear, Color.black, Mathf.Max(0.001f, duration), true);

    private async UniTask Fade(Color startColor, Color endColor, float duration, bool leaveOn = false)
    {
        faderImage.gameObject.SetActive(true);

        var elapsed = 0f;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            faderImage.color = Color.Lerp(startColor, endColor, elapsed / duration);
            await UniTask.NextFrame();
        }

        faderImage.gameObject.SetActive(leaveOn);
    }
}
