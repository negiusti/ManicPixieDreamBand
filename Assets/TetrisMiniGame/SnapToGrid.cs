using System.Collections.Generic;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    //private bool isDragging = false;
    public string blockTag = "Obstacle";    // The tag to avoid collision with
    public float snapIncrement = 0.3f;
    private Vector3 mousePositionOffset;
    private bool selected;
    private Vector3 startingPos;
    private HashSet<string> collidingObjs;
    private static readonly object lockObject = new object();

    void Start()
    {
        collidingObjs = new HashSet<string>();
        startingPos = transform.position;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            // Calculate the offset between the click point and the object's position
            mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
            //isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        selected = false;
        //rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
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
        selected = true;
        Vector3 snappedPosition = GetMouseWorldPosition() + mousePositionOffset;
        // Update the GameObject's position to the snapped position
        transform.position = RoundPosition(snappedPosition, snapIncrement);
        //IsCollidingWithBlock();
        //if (!IsCollidingWithBlock())
        //{
        //    lastValidPos = transform.position;
        //}
        //else
        //{
        //    transform.position = lastValidPos;
        //}
    }

    private void OnMouseEnter()
    {
        
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

        // Get the current position of the GameObject
        Vector3 currentPosition = transform.position;

        // Round the position to the nearest snapIncrement
        Vector3 snappedPosition = RoundPosition(currentPosition, snapIncrement);
  
        // Update the GameObject's position to the snapped position
        transform.position = snappedPosition;
        //lock (lockObject)
        //{
        //    bool isColliding = collidingObjs.Count > 0;
        //    if (!isColliding)
        //    {
        //        lastValidPos = transform.position;
        //    }
        //    else
        //    {
        //        transform.position = lastValidPos;
        //    }
        //}

    }

    private Vector3 RoundPosition(Vector3 position, float snapValue)
    {
        float x = SnapValue(position.x, snapValue);
        float y = SnapValue(position.y, snapValue);
        float z = SnapValue(position.z, snapValue);

        return new Vector3(x, y, z);
    }

    private float SnapValue(float value, float snapIncrement)
    {
        return Mathf.Round(value / snapIncrement) * snapIncrement;
    }

    //private bool IsCollidingWithBlock()
    //{
    //    lock (lockObject)
    //    {
    //        bool isColliding = collidingObjs.Count > 0;
    //        if (!isColliding)
    //        {
    //            lastValidPos = transform.position;
    //        }
    //        else
    //        {
    //            transform.position = lastValidPos;
    //        }
    //        return isColliding;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        lock (lockObject)
        {
            if (collision.CompareTag(blockTag))
            {
                collidingObjs.Add(collision.gameObject.name);
            }

            bool isColliding = collidingObjs.Count > 0;
            if (isColliding && selected)
            {
                transform.position = startingPos;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        lock (lockObject)
        {
            if (collision.CompareTag(blockTag))
            {
                collidingObjs.Add(collision.gameObject.name);
            }

            bool isColliding = collidingObjs.Count > 0;
            if (isColliding && selected)
            {
                transform.position = startingPos;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        lock (lockObject)
        {
            if (collision.CompareTag(blockTag))
            {
                collidingObjs.Remove(collision.gameObject.name);
            }
        }
    }
}
