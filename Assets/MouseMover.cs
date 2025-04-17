using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;
using System.Collections;
using System.Collections.Generic;

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
            SimulateMouseDown();
        }

        if (player.GetButtonUp("Select Object Under Cursor"))
        {
            SimulateMouseUp();
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

    //public void SimulateLeftClick()
    //{
    //    var pointerData = new PointerEventData(EventSystem.current)
    //    {
    //        pointerId = -1,
    //        position = virtualMousePos,
    //        button = PointerEventData.InputButton.Left,
    //        clickCount = 1
    //    };

    //    // --- UI Raycast ---
    //    List<RaycastResult> uiHits = new List<RaycastResult>();
    //    HashSet<GameObject> uiHitObjects = new HashSet<GameObject>();

    //    EventSystem.current.RaycastAll(pointerData, uiHits);

    //    if (uiHits.Count > 0)
    //    {
    //        RaycastResult firstUIHit = uiHits[0];
    //        GameObject target = firstUIHit.gameObject;
    //        uiHitObjects.Add(target);
    //        pointerData.pointerCurrentRaycast = firstUIHit;
    //        Debug.Log($"Simulated click hit {target.name}");
    //        // Set internal click state
    //        pointerData.pointerEnter = target;
    //        pointerData.pointerPressRaycast = firstUIHit;
    //        pointerData.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(target);
    //        pointerData.rawPointerPress = target;
    //        pointerData.eligibleForClick = true;

    //        // Handle EventTrigger UI components
    //        var et = target.GetComponent<EventTrigger>();
    //        if (et != null)
    //        {
    //            foreach (var entry in et.triggers)
    //            {
    //                if (entry.eventID == EventTriggerType.PointerDown)
    //                    entry.callback.Invoke(pointerData);
    //                if (entry.eventID == EventTriggerType.PointerClick)
    //                    entry.callback.Invoke(pointerData);
    //                if (entry.eventID == EventTriggerType.PointerUp)
    //                    entry.callback.Invoke(pointerData);
    //            }
    //        } else
    //        {
    //            // Execute UI events
    //            ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.pointerEnterHandler);
    //            ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.pointerDownHandler);
    //            ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.pointerClickHandler);
    //            ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.pointerUpHandler);
    //        }
    //    }
    //    if (cam == null || !cam.isActiveAndEnabled)
    //        cam = GetFirstActiveCamera();

    //    int max2DHits = 1;
    //    var raycaster2D = cam.GetComponent<UnityEngine.EventSystems.Physics2DRaycaster>();
    //    if (raycaster2D != null) max2DHits = raycaster2D.maxRayIntersections;
    //    int eventMask = raycaster2D != null ? raycaster2D.eventMask : ~0;

    //    Vector2 worldPoint = cam.ScreenToWorldPoint(virtualMousePos);
    //    RaycastHit2D[] hits2D = Physics2D.RaycastAll(worldPoint, Vector2.zero);

    //    if (max2DHits > 0 && hits2D.Length > max2DHits)
    //    {
    //        System.Array.Sort(hits2D, (a, b) => a.distance.CompareTo(b.distance));
    //        System.Array.Resize(ref hits2D, max2DHits);
    //    }

    //    if (hits2D.Length > 0)
    //    {
    //        GameObject target = hits2D[0].collider.gameObject;
    //        Debug.Log($"Simulated click hit {target.name}");
    //        target.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
    //        target.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
    //        target.SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
    //    }

    //    foreach (var hit in hits2D)
    //    {
    //        if (hit.collider == null) continue;

    //        GameObject target = hit.collider.gameObject;

    //        if (((1 << target.layer) & eventMask) == 0) continue; 
    //        if (uiHitObjects.Contains(target)) continue;

    //        Debug.Log($"Simulated 2D click on {target.name}");

    //        target.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
    //        target.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
    //        target.SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
    //    }
    //}

    private void SimulateMouse(PointerEventData.InputButton button, bool isDown)
    {
        var pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
            position = virtualMousePos,
            button = button,
            clickCount = 1
        };

        List<RaycastResult> uiHits = new List<RaycastResult>();
        HashSet<GameObject> uiHitObjects = new HashSet<GameObject>();

        EventSystem.current.RaycastAll(pointerData, uiHits);

        if (uiHits.Count > 0)
        {
            RaycastResult firstUIHit = uiHits[0];
            GameObject target = firstUIHit.gameObject;
            uiHitObjects.Add(target);
            pointerData.pointerCurrentRaycast = firstUIHit;
            pointerData.pointerEnter = target;
            pointerData.pointerPressRaycast = firstUIHit;
            pointerData.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(target);
            pointerData.rawPointerPress = target;
            pointerData.eligibleForClick = true;

            var et = target.GetComponent<EventTrigger>();
            if (et != null)
            {
                foreach (var entry in et.triggers)
                {
                    if (isDown && entry.eventID == EventTriggerType.PointerDown)
                        entry.callback.Invoke(pointerData);
                    if (!isDown && entry.eventID == EventTriggerType.PointerUp)
                        entry.callback.Invoke(pointerData);
                    if (!isDown && entry.eventID == EventTriggerType.PointerClick)
                        entry.callback.Invoke(pointerData);
                }
            }
            else
            {
                ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.pointerEnterHandler);
                if (isDown)
                    ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.pointerDownHandler);
                else
                {
                    ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.pointerClickHandler);
                    ExecuteEvents.ExecuteHierarchy(target, pointerData, ExecuteEvents.pointerUpHandler);
                }
            }
        }

        if (cam == null || !cam.isActiveAndEnabled)
            cam = GetFirstActiveCamera();

        int max2DHits = 1;
        var raycaster2D = cam.GetComponent<UnityEngine.EventSystems.Physics2DRaycaster>();
        if (raycaster2D != null) max2DHits = raycaster2D.maxRayIntersections;
        int eventMask = raycaster2D != null ? raycaster2D.eventMask : ~0;

        Vector2 worldPoint = cam.ScreenToWorldPoint(virtualMousePos);
        RaycastHit2D[] hits2D = Physics2D.RaycastAll(worldPoint, Vector2.zero);

        if (max2DHits > 0 && hits2D.Length > max2DHits)
        {
            System.Array.Sort(hits2D, (a, b) => a.distance.CompareTo(b.distance));
            System.Array.Resize(ref hits2D, max2DHits);
        }

        foreach (var hit in hits2D)
        {
            if (hit.collider == null) continue;

            GameObject target = hit.collider.gameObject;

            if (((1 << target.layer) & eventMask) == 0) continue;
            if (uiHitObjects.Contains(target)) continue;

            Debug.Log($"Simulated 2D {(isDown ? "mouse down" : "mouse up")} on {target.name}");

            if (isDown)
                target.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
            else
            {
                target.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                target.SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void SimulateMouseDown()
    {
        SimulateMouse(PointerEventData.InputButton.Left, true);
    }

    public void SimulateMouseUp()
    {
        SimulateMouse(PointerEventData.InputButton.Left, false);
    }

    public void SimulateRightClick()
    {
        SimulateMouse(PointerEventData.InputButton.Right, false);
        SimulateMouse(PointerEventData.InputButton.Right, true);
    }

}
