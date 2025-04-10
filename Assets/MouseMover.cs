using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Rewired;

public class MouseMover : MonoBehaviour
{
    public float speed = 100f; // Speed to move the cursor
    private Player player;

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SimulateLeftClick();
        }
        if (UnityEngine.InputSystem.Mouse.current == null)
            return;

        // Get axis input (you can map this in Input Actions or use Keyboard directly)
        float horizontal = player.GetAxis("Move Cursor Horizontal"); //Input.GetAxis("Horizontal"); // Arrow keys / A/D
        float vertical = player.GetAxis("Move Cursor Vertical");     // Arrow keys / W/S
        Debug.Log("horizontal: " + horizontal + " vert: " + vertical);
        if (Mathf.Abs(horizontal) < 0.001f && Mathf.Abs(vertical) < 0.001f)
            return;

        // Get current mouse position
        Vector2 currentMousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

        // Calculate new position
        Vector2 delta = new Vector2(horizontal, vertical) * speed * Time.deltaTime;
        Vector2 newMousePos = currentMousePos + delta;

        // Clamp position to screen bounds
        newMousePos.x = Mathf.Clamp(newMousePos.x, 0, Screen.width);
        newMousePos.y = Mathf.Clamp(newMousePos.y, 0, Screen.height);

        // Warp the cursor
        UnityEngine.InputSystem.Mouse.current.WarpCursorPosition(newMousePos);
    }

    void SimulateLeftClick()
    {
        if (UnityEngine.InputSystem.Mouse.current == null)
            return;

        Vector2 mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

        // Do a raycast into the scene
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"Simulated click hit {hit.collider.name}");

            // Optionally: call something on the object
            // hit.collider.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
        }

        // Optionally: trigger UI click if over UI
        if (EventSystem.current != null)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = mousePosition
            };

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                Debug.Log($"Simulated UI click on {result.gameObject.name}");

                ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
