using UnityEngine;

public class MouseInput : IInput
{
    private Camera cam;
    private RaycastHit[] hits = new RaycastHit[20];

    public event System.EventHandler<ButtonEventArgs> ButtonChanged;
    public event System.EventHandler<ScrollEventArgs> ScrollChanged;
    public event System.EventHandler<PositionEventArgs> CursorMoved;

    public MouseInput(Camera cam) => this.cam = cam;

    public void UpdateInputEvents()
    {
        Vector2 curMousePos = Input.mousePosition;
        System.Array.Clear(hits, 0, hits.Length);

        if(Input.GetMouseButtonDown(0))
        {
            Physics.RaycastNonAlloc(cam.ScreenPointToRay(curMousePos), hits);

            ButtonChanged?.Invoke(this, new ButtonEventArgs
            {
                PressedDown = true,
                ScreenPosition = curMousePos,
                Hits = hits
            });
        }

        if(Input.GetMouseButtonUp(0))
        {
            Physics.RaycastNonAlloc(cam.ScreenPointToRay(curMousePos), hits);

            ButtonChanged?.Invoke(this, new ButtonEventArgs
            {
                PressedDown = false,
                ScreenPosition = curMousePos,
                Hits = hits
            });
        }

        var scrollValue = Input.GetAxisRaw("Mouse ScrollWheel");

        if(scrollValue != 0)
            ScrollChanged?.Invoke(this, new ScrollEventArgs { ScrollValue = scrollValue });

        CursorMoved?.Invoke(this, new PositionEventArgs
        {
            Position = curMousePos,
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
