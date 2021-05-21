using UnityEngine;

public class TouchInput : IInput
{
    private Camera cam;
    private RaycastHit[] hits = new RaycastHit[20];

    private float fingerDistance;

    public event System.EventHandler<ButtonEventArgs> ButtonChanged;
    public event System.EventHandler<ScrollEventArgs> ScrollChanged;
    public event System.EventHandler<PositionEventArgs> CursorMoved;

    public TouchInput(Camera cam) => this.cam = cam;

    //Called in an Update loop
    public void UpdateInputEvents()
    {
        if (Input.touchCount == 1)
            UpdateSingleTouchEvents();
        else if (Input.touchCount == 2)
            UpdatePinchEvents();
    }

    private void UpdateSingleTouchEvents()
    {
        System.Array.Clear(hits, 0, hits.Length);
        var newTouch = Input.GetTouch(0);

        switch (newTouch.phase)
        {
            case TouchPhase.Began:
                //Debug.Log("Single touch started.");

                Physics.RaycastNonAlloc(cam.ScreenPointToRay(newTouch.position), hits);
                ButtonChanged?.Invoke(this, new ButtonEventArgs
                {
                    PressedDown = true,
                    ScreenPosition = newTouch.position,
                    Hits = hits
                });
                break;
            case TouchPhase.Stationary:
            case TouchPhase.Moved:
                //Debug.Log($"Single touch moving, delta: {newTouch.deltaPosition * newTouch.deltaTime}");

                CursorMoved?.Invoke(this, new PositionEventArgs
                {
                    Position = newTouch.position,
                    Delta = newTouch.deltaPosition * newTouch.deltaTime
                });
                break;
            case TouchPhase.Ended:
                //Debug.Log("Single touch ended.");

                Physics.RaycastNonAlloc(cam.ScreenPointToRay(newTouch.position), hits);
                ButtonChanged?.Invoke(this, new ButtonEventArgs
                {
                    PressedDown = false,
                    ScreenPosition = newTouch.position,
                    Hits = hits
                });
                break;
        }
    }

    private void UpdatePinchEvents()
    {
        var firstFinger = Input.GetTouch(0);
        var secondFinger = Input.GetTouch(1);

        if (firstFinger.phase == TouchPhase.Ended || secondFinger.phase == TouchPhase.Ended)
        {
            //Debug.Log($"Pinch ended, final distance: {fingerDistance}");
            fingerDistance = 0f;
        }
        else if (firstFinger.phase == TouchPhase.Began || secondFinger.phase == TouchPhase.Began)
        {
            fingerDistance = (secondFinger.position - firstFinger.position).magnitude;
            //Debug.Log($"Pinch detected, starting distance: {fingerDistance}");
        }
        else if (firstFinger.phase == TouchPhase.Moved || secondFinger.phase == TouchPhase.Moved)
        {
            var curDistance = (secondFinger.position - firstFinger.position).magnitude;
            var difference = curDistance - fingerDistance;
            fingerDistance = curDistance;

            ScrollChanged?.Invoke(this, new ScrollEventArgs { ScrollValue = difference });
            //Debug.Log($"Pinch changed, difference in distance: {difference}");
        }
    }
}
