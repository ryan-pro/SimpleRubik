using UnityEngine;

[AddComponentMenu("SimpleMagicCube/UI/Submenu Transition Player")]
public class SubmenuTransitionPlayer : MonoBehaviour
{
    [SerializeField]
    private Animation submenuAnimation;
    [SerializeField]
    private AnimationClip clip;

    public void Play()
    {
        submenuAnimation[clip.name].normalizedTime = 0f;
        submenuAnimation[clip.name].speed = 1f;
        submenuAnimation.Play();
    }

    public void PlayReverse()
    {
        submenuAnimation[clip.name].normalizedTime = 1f;
        submenuAnimation[clip.name].speed = -1f;
        submenuAnimation.Play();
    }
}
