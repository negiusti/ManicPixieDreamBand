using System;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    public string blockTag = "Obstacle"; // The tag to avoid collisions with

    public float snapIncrement = 0.3f;
    private Vector3 mousePositionOffset;
    private bool selected;

    private Vector3 startingPos;

    public Vector2 gridPosition;

    public int width;
    public int height;

    private TrunkGrid grid;

    // Width is rows, height is columns
    public bool[,] ignoredCells;

    void Start()
    {
        ignoredCells = new bool[width, height];
        ignoredCells[0, 0] = true;

        startingPos = transform.position;

        grid = GetComponentInParent<TrunkGrid>();

        // Occupying the cells under this object on start
        SetPosition();
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();

        // When an object is grabbed, the cells it was occupying becomes unoccupied.
        grid.ClearPosition(gridPosition, width, height, ignoredCells);
    }

    private void OnMouseDrag()
    {
        selected = true;
        Vector3 snappedPosition = GetMouseWorldPosition() + mousePositionOffset;

        // Update the GameObject's position to the snappedPosition.
        transform.position = RoundPosition(snappedPosition, snapIncrement);
    }

    // Places the dragged object wherever the mouse is released, shifts the object if needed, and resets the position if the object is overlapping too much with another object.
    private void OnMouseUp()
    {
        PlaceObject();
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Get the mousePosition in world coordinates.
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        // Update the GameObject's position to the snapped position
        transform.position = GetSnappedPosition();

        // Rotate (Right Mouse Button) while NOT dragging
        if (selected && Input.GetMouseButtonDown(1) && !Input.GetMouseButton(0))
        {
            grid.ClearPosition(gridPosition, width, height, ignoredCells);

            Rotate();

            if (!CheckPosition(0, 0))
            {
                if (!ShiftIfNeeded())
                {
                    Unrotate();
                }
            }

            SetPosition();
        }

        // The occupied cells beneath the object does not need to be cleared if you are dragging.
        if (selected && Input.GetMouseButtonDown(1) && Input.GetMouseButton(0)) // Right mouse button click while dragging
        {
            Rotate();
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            PrintIgnoredCells();
        }
    }

    private void PrintIgnoredCells()
    {
        for (int j = 0; j < ignoredCells.GetLength(1); j++)
        {
            for (int i = 0; i < ignoredCells.GetLength(0); i++)
            {
                var msg = "[" + i.ToString() + ", " + j.ToString() + "] = " + ignoredCells[i, j].ToString();
                Debug.Log(msg);
            }
        }
    }

    private Vector2 WorldToGridCoorindates(Vector2 Coords, float w, float h)
    {
        // The gridPosition of each object is at its bottom left corner. This is obtained by subtracting half the width and half the height from the snapped position.
        return new Vector2(Coords.x - Mathf.FloorToInt(w / 2), Coords.y - Mathf.FloorToInt(h / 2));
    }

    public bool ShiftIfNeeded()
    {
        if (CheckPosition(0, 1))
        {
            // Move the object up by 1 cell
            transform.position = new Vector2(transform.position.x, transform.position.y + snapIncrement);

            //Debug.Log("Up");

            SetPosition();

            return true;
        }
        else if (CheckPosition(0, -1))
        {
            // Move the object down by 1 cell
            transform.position = new Vector2(transform.position.x, transform.position.y - snapIncrement);

            //Debug.Log("Down");

            SetPosition();

            return true;
        }
        else if (CheckPosition(-1, 0))
        {
            // Move the object left by 1 cell
            transform.position = new Vector2(transform.position.x - snapIncrement, transform.position.y);

            //Debug.Log("Left");

            SetPosition();

            return true;
        }
        else if (CheckPosition(1, 0))
        {
            // Move the object right by 1 cell
            transform.position = new Vector2(transform.position.x + snapIncrement, transform.position.y);

            //Debug.Log("Right");

            SetPosition();

            return true;
        }

        return false;
    }

    public void ResetPosition()
    {
        transform.position = startingPos;

        //Debug.Log("Reset");
        
        SetPosition();
    }

    public void Rotate()
    {
        // Rotate the game object by 90 degrees around the up (Y) axis
        transform.Rotate(Vector3.forward, 90f);

        ignoredCells = RotateArray(ignoredCells);

        // Swapping the width and the height
        int Temporary = width;
        width = height;
        height = Temporary;
    }

    public void Unrotate()
    {
        // Rotate the game object by 90 degrees around the up (Y) axis
        transform.Rotate(Vector3.forward, -90f);

        // Swapping the width and the height
        int Temporary = width;
        width = height;
        height = Temporary;
    }

    private void PlaceObject()
    {
        if (CheckPosition(0, 0))
        {
            SetPosition();
        }
        else
        {
            if (!ShiftIfNeeded())
            {
                ResetPosition();
            }
        }
    }

    private float SnapValue(float value, float snapIncrement)
    {
        return Mathf.Round(value / snapIncrement) * snapIncrement;
    }

    private Vector3 RoundPosition(Vector3 position, float snapValue)
    {
        float x = SnapValue(position.x, snapValue);
        float y = SnapValue(position.y, snapValue);
        float z = SnapValue(position.z, snapValue);

        return new Vector3(x, y, z);
    }

    public Vector2 GetSnappedPosition()
    {
        // Get the current position of the GameObject
        Vector3 currentPosition = transform.position;

        // Round the position to the nearest snapIncrement
        return RoundPosition(currentPosition, snapIncrement);
    }

    private void UpdateGridPositions()
    {
        gridPosition = WorldToGridCoorindates(GetSnappedPosition(), width, height);
    }

    // CheckPosition takes in two values for where in relation to the gridPosition it should check
    private bool CheckPosition(int xShift, int yShift)
    {
        UpdateGridPositions();
        return grid.CheckPosition(new Vector2(gridPosition.x + xShift, gridPosition.y + yShift), width, height, ignoredCells);
    }

    private void SetPosition()
    {
        UpdateGridPositions();
        grid.SetPosition(gridPosition, width, height, ignoredCells);
    }

    private bool[,] RotateArray(bool[,] originalArray)
    {
        int rows = originalArray.GetLength(0);
        int cols = originalArray.GetLength(1);

        bool[,] rotatedArray = new bool[cols, rows];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                rotatedArray[cols - 1 - j, i] = originalArray[i, j];
            }
        }

        return rotatedArray;
    }
}