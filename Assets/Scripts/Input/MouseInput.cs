using UnityEngine;

public class MouseInput : IInput
{
    private Camera cam;
    private Vector2 previousMousePos;

    public event System.EventHandler<ButtonEventArgs> ButtonChanged;
    public event System.EventHandler<ScrollEventArgs> ScrollChanged;
    public event System.EventHandler<PositionEventArgs> CursorMoved;
    public MouseInput(Camera cam) => this.cam = cam;

    public void UpdateInputEvents()
    {
        Vector2 curMousePos = Input.mousePosition;

        if(Input.GetMouseButtonDown(0))
        {
            Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit);

            ButtonChanged?.Invoke(this, new ButtonEventArgs
            {
                PressedDown = true,
                ScreenPosition = curMousePos,
                Hit = hit
            });
        }

        if(Input.GetMouseButtonUp(0))
        {
            Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit);

            ButtonChanged?.Invoke(this, new ButtonEventArgs
            {
                PressedDown = false,
                ScreenPosition = curMousePos,
                Hit = hit
            });
        }

        var scrollValue = Input.GetAxisRaw("Mouse ScrollWheel");

        if(scrollValue != 0)
            ScrollChanged?.Invoke(this, new ScrollEventArgs { ScrollValue = scrollValue });

        CursorMoved?.Invoke(this, new PositionEventArgs
        {
            Delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
        });

        //if (curMousePos != previousMousePos)
        //{
        //    previousMousePos = curMousePos;
        //    CursorMoved?.Invoke(this, new PositionEventArgs
        //    {
        //        Delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
        //    });
        //}
    }
}
