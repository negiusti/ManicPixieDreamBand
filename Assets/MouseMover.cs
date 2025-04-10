using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;

public class MouseMover : MonoBehaviour
{
    public float speed = 500f; // Speed to move the cursor
    private Player player;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        if (player.GetButtonDown("Select Object Under Cursor"))
        {
            SimulateLeftClick();
        }
        if (UnityEngine.InputSystem.Mouse.current == null)
            return;

        // Get axis input (you can map this in Input Actions or use Keyboard directly)
        float horizontal = player.GetAxis("Move Cursor Horizontal"); //Input.GetAxis("Horizontal"); // Arrow keys / A/D
        float vertical = player.GetAxis("Move Cursor Vertical");     // Arrow keys / W/S

        if (Mathf.Abs(horizontal) < 0.001f && Mathf.Abs(vertical) < 0.001f)
            return;

        Debug.Log("horizontal: " + horizontal + " vert: " + vertical);
        // Get current mouse position
        Vector2 currentMousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        Debug.Log("currentMousePos: " + currentMousePos.x + ", " + currentMousePos.y);

        // Calculate new position
        Vector2 delta = new Vector2(horizontal, vertical) * speed * Mathf.Min(0.01f,Time.deltaTime);
        Debug.Log("delta: " + delta.x + ", " + delta.y);
        Vector2 newMousePos = currentMousePos + delta;
        Debug.Log("newMousePos: " + newMousePos.x + ", " + newMousePos.y);

        // Clamp position to screen bounds
        newMousePos.x = Mathf.Clamp(newMousePos.x, 0, Screen.width);
        newMousePos.y = Mathf.Clamp(newMousePos.y, 0, Screen.height);
        Debug.Log("clamped newMousePos: " + newMousePos.x + ", " + newMousePos.y);

        // Warp the cursor
        UnityEngine.InputSystem.Mouse.current.WarpCursorPosition(newMousePos);
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
        if (UnityEngine.InputSystem.Mouse.current == null)
            return;

        Vector2 mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

        // Raycast into 3D scene (optional)
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"Simulated click hit {hit.collider.name}");
            // hit.collider.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
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

                // Simulate pointer down
                ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerDownHandler);

                // Simulate click
                ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerClickHandler);

                // (Optional) Simulate pointer up
                ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerUpHandler);
            }
        }
    }
}
