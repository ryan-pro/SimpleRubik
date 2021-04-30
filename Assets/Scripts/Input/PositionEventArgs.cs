using UnityEngine;

public class PositionEventArgs : System.EventArgs
{
    public Vector2 Position { get; set; }
    public Vector2 Delta { get; set; }
}
