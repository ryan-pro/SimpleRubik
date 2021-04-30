using TMPro;
using UnityEngine;

public class TimerObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private Timer timerLogic;

    private void OnEnable() => timerText.text = timerLogic.GetTimeByMinute();

    private void Update()
    {
        if (!timerLogic.ShouldCount)
            return;

        timerLogic.UpdateTimer();
        timerText.text = timerLogic.GetTimeByMinute();
    }
}
