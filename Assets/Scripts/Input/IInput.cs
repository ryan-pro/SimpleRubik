using System;

public interface IInput
{
    event EventHandler<ButtonEventArgs> ButtonChanged;
    event EventHandler<ScrollEventArgs> ScrollChanged;
    event EventHandler<PositionEventArgs> CursorMoved;

    void UpdateInputEvents();
}
