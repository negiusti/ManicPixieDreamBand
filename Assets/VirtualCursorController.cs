using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Rewired;


public class VirtualCursorController : MonoBehaviour
{
    public RectTransform cursorTransform; // Assign the Image RectTransform in Inspector
    public float moveSpeed = 1000f;
    public float mouseSyncThreshold = 0.1f; // How much movement before syncing to OS cursor
    private Player player;
    private Vector2 virtualCursorPos;
    private Vector2 lastMousePos;

    void Start()
    {
        player = ReInput.players.GetPlayer(0);

        // Initialize to current OS mouse position
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            virtualCursorPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
            lastMousePos = virtualCursorPos;
        }
        UpdateCursorPosition();
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            Vector2 currentMouse = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
            Vector2 mouseDelta = currentMouse - lastMousePos;

            if (mouseDelta.sqrMagnitude > mouseSyncThreshold * mouseSyncThreshold)
            {
                virtualCursorPos = currentMouse;
            }

            lastMousePos = currentMouse;
        }

        // Keyboard or gamepad input
        float horizontal = player.GetAxis("Move Cursor Horizontal");
        float vertical = player.GetAxis("Move Cursor Vertical");
        Vector2 move = new Vector2(
            horizontal,
            vertical
        );

        if (move.sqrMagnitude > 0.01f)
        {
            virtualCursorPos += move * moveSpeed * Time.deltaTime;
        }

        // Clamp to screen
        virtualCursorPos.x = Mathf.Clamp(virtualCursorPos.x, 0, Screen.width);
        virtualCursorPos.y = Mathf.Clamp(virtualCursorPos.y, 0, Screen.height);

        UpdateCursorPosition();

        if (Input.GetKeyDown(KeyCode.Space)) // Or your input of choice
        {
            SimulateClick();
        }
    }

    void UpdateCursorPosition()
    {
        if (cursorTransform == null || cursorTransform.parent == null)
            return;

        Canvas canvas = cursorTransform.GetComponentInParent<Canvas>();
        if (canvas == null)
            return;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Convert screen-space to local position in the canvas
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, virtualCursorPos, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out localPoint))
        {
            cursorTransform.localPosition = localPoint;
        }
    }

    void SimulateClick()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = virtualCursorPos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            GameObject target = result.gameObject;

            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerEnterHandler);
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerClickHandler);
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerUpHandler);

            Debug.Log("Simulated Click on: " + target.name);
            break; // topmost only
        }
    }
}
