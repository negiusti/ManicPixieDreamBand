using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    //private bool isDragging = false;
    private Vector3 mousePositionOffset;
    private bool selected;

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Calculate the offset between the click point and the object's position
            mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
            //isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    isDragging = false;
        //}
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in world coordinates
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = -Camera.main.transform.position.z;
        //return Camera.main.ScreenToWorldPoint(mousePos);
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnMouseEnter()
    {
        selected = true;
    }

    private void OnMouseExit()
    {
        selected = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && selected) // Right mouse button click
        {
            // Rotate the game object by 90 degrees around the up (Y) axis
            transform.Rotate(Vector3.forward, 90f);
        }
    }
}
