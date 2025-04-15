using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;

public class MouseMover : MonoBehaviour
{
    private Vector2 virtualMousePos;
    private Camera cam;
    public float speed = 1000f; // Speed to move the cursor
    private Player player;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
        cam = GetFirstActiveCamera();
        // Initialize virtual cursor to current position at start
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            virtualMousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        }
    }

    public Camera GetFirstActiveCamera()
    {
        if (cam != null && cam.isActiveAndEnabled)
            return cam;
        
        if (Camera.main != null && Camera.main.isActiveAndEnabled)
        {
            return Camera.main;
        }

        foreach (Camera cam in Camera.allCameras)
        {
            if (cam != null && cam.isActiveAndEnabled)
            {
                return cam;
            }
        }

        return null;
    }

    void Update()
    {
        if (player.GetButtonDown("Select Object Under Cursor"))
        {
            SimulateLeftClick();
        }

        if (UnityEngine.InputSystem.Mouse.current == null)
            return;

        //if (player.GetAxis)

        float horizontal = player.GetAxis("Move Cursor Horizontal");
        float vertical = player.GetAxis("Move Cursor Vertical");

        if (Mathf.Abs(horizontal) < 0.001f && Mathf.Abs(vertical) < 0.001f)
        {
            return;
        }

        Vector2 delta = new Vector2(horizontal, vertical) * speed * Mathf.Max(0.01f, Time.deltaTime);
        virtualMousePos += delta;

        // Clamp to screen bounds
        virtualMousePos.x = Mathf.Clamp(virtualMousePos.x, 0, Screen.width);
        virtualMousePos.y = Mathf.Clamp(virtualMousePos.y, 0, Screen.height);

        UnityEngine.InputSystem.Mouse.current.WarpCursorPosition(virtualMousePos);
    }


    //void SimulateLeftClick()
    //{
    //    if (UnityEngine.InputSystem.Mouse.current == null)
    //        return;

    //    Vector2 mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

    //    // Do a raycast into the scene
    //    Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        Debug.Log($"Simulated click hit {hit.collider.name}");

    //        // Optionally: call something on the object
    //        // hit.collider.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
    //    }

    //    // Optionally: trigger UI click if over UI
    //    if (EventSystem.current != null)
    //    {
    //        PointerEventData pointerData = new PointerEventData(EventSystem.current)
    //        {
    //            position = mousePosition
    //        };

    //        var results = new System.Collections.Generic.List<RaycastResult>();
    //        EventSystem.current.RaycastAll(pointerData, results);

    //        foreach (var result in results)
    //        {
    //            Debug.Log($"Simulated UI click on {result.gameObject.name}");

    //            ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
    //        }
    //    }
    //}

    void SimulateLeftClick()
    {
        //if (UnityEngine.InputSystem.Mouse.current == null)
        //    return;

        Vector2 mousePosition = virtualMousePos;

        if (cam == null || !cam.isActiveAndEnabled)
            cam = GetFirstActiveCamera();

        Ray ray = cam.ScreenPointToRay(mousePosition);

        int mxIntersections = int.MaxValue;
        if (cam != null && cam.GetComponent<Physics2DRaycaster>() != null && cam.GetComponent<Physics2DRaycaster>().maxRayIntersections > 0)
            mxIntersections = cam.GetComponent<Physics2DRaycaster>().maxRayIntersections;

        if (mxIntersections > 1)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                Debug.Log($"Simulated click hit {hit.collider.name}");

                // Optional: trigger a method on the hit object
                
                hit.collider.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
                hit.collider.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
                hit.collider.gameObject.SendMessage("OnPointerDown", SendMessageOptions.DontRequireReceiver);
        }
    }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"Simulated click hit {hit.collider.name}");
                hit.collider.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
                hit.collider.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
            }
        }

        // Trigger UI click if over UI
        if (EventSystem.current != null)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = mousePosition,
                button = PointerEventData.InputButton.Left,
                clickCount = 1
            };

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                GameObject target = result.gameObject;

                Debug.Log($"Simulated pointer down + click on {target.name}");

                var eventTrigger = target.GetComponent<EventTrigger>();
                if (eventTrigger != null)
                {
                    foreach (var entry in eventTrigger.triggers)
                    {
                        if (entry.eventID == EventTriggerType.PointerDown)
                            entry.callback.Invoke(pointerData);
                        if (entry.eventID == EventTriggerType.PointerClick)
                            entry.callback.Invoke(pointerData);
                        if (entry.eventID == EventTriggerType.PointerUp)
                            entry.callback.Invoke(pointerData);
                    }
                }
                else
                {

                    // Simulate pointer down
                    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerDownHandler);

                    // Simulate click
                    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerClickHandler);

                    // (Optional) Simulate pointer up
                    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerUpHandler);
                }

                if (mxIntersections == 1)
                {
                    break;
                }
            }
        }

    }
}
