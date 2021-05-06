using TMPro;
using UnityEngine;

public class TimerObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private Timer timerLogic;

    private void OnEnable() => timerText.text = "00:00";

    private void Update()
    {
        if (!timerLogic.ShouldCount)
            return;

        timerLogic.UpdateTimer();
        timerText.text = timerLogic.GetTimeByMinute();
    }
}
