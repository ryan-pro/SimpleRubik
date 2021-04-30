using UnityEngine;

public class ButtonEventArgs : System.EventArgs
{
    public bool PressedDown { get; set; }
    public Vector2 ScreenPosition { get; set; }
    public RaycastHit Hit { get; set; }
}
