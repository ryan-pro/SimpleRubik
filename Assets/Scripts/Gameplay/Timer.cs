using UnityEngine;

[CreateAssetMenu(menuName = "Simple Magic Cube/Timer")]
public class Timer : ScriptableObject
{
    private float countedTime = 0f;
    private bool shouldCount;

    public float CountedTime => countedTime;
    public bool ShouldCount
    {
        get => shouldCount;
        set => shouldCount = value;
    }

    public void StartCounting() => ShouldCount = true;
    public void StopCounting() => ShouldCount = false;
    public void Restart() => countedTime = 0f;

    public void UpdateTimer() => countedTime += Time.deltaTime;

    public string GetTimeByMinute()
    {
        var minutes = Mathf.FloorToInt(countedTime / 60f).ToString().PadLeft(2, '0');
        var seconds = Mathf.RoundToInt(countedTime % 60f).ToString().PadLeft(2, '0');
        return $"{minutes}:{seconds}";
    }
}
