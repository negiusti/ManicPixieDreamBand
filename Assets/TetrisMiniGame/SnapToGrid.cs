using System;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    public string blockTag = "Obstacle"; // The tag to avoid collisions with

    public float snapIncrement = 0.3f;
    private Vector3 mousePositionOffset;
    private bool selected;

    private Vector3 startingPos;

    private int startingWidth;
    private int startingHeight;

    public Vector2 gridPosition;

    public int width;
    public int height;

    private TrunkGrid grid;

    public bool inTrunk;

    // Width is rows, height is columns. This is the array in which the shape in the inspector will be stored into
    public bool[,] ignoredCells;

    private float resetStartTime;
    private float resetDuration = 0.5f;
    private bool resetInProgress;

    private Vector3 resetFromPosition;

    [System.Serializable]
    public class IgnoreCellsTable
    {
        public bool[] column;
    }

    [SerializeField]
    // This is the shape in the inspector with all of the cells that are ignored
    public IgnoreCellsTable[] rows;

    private void Start()
    {
        transform.position = GetSnappedPosition();

        startingPos = transform.position;

        startingWidth = width;
        startingHeight = height;

        ignoredCells = PopulateArray();

        grid = GetComponentInParent<TrunkGrid>();
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();

        // When an object is grabbed, the cells it was occupying becomes unoccupied.
        ClearPosition();
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
        if (resetInProgress)
        {
            LerpToStartingPosition();
            return;
        }

        // Update the GameObject's position to the snapped position
        transform.position = GetSnappedPosition();

        // Rotate (Right Mouse Button) while NOT dragging
        if (selected && Input.GetMouseButtonDown(1) && !Input.GetMouseButton(0))
        {
            ClearPosition();

            Rotate();

            if (!CheckPosition(0, 0))
            {
                // Going into here when it shouldn't be
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
    }

    private bool ShiftIfNeeded()
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

    private void Reset()
    {
        resetInProgress = true;
        resetStartTime = Time.time;
        resetFromPosition = transform.position;

        transform.rotation = Quaternion.Euler(0, 0, 0);

        width = startingWidth;
        height = startingHeight;

        ignoredCells = PopulateArray();
    }

    private void LerpToStartingPosition()
    {
        // Calculate the lerp parameter based on elapsed time
        float t = (Time.time - resetStartTime) / resetDuration;

        // Interpolate between the start and target positions
        transform.position = Vector3.Lerp(resetFromPosition, startingPos, t);

        // If the lerp parameter reaches 1, the reset is complete
        if (t >= 1.0f)
        {
            //Debug.Log("Reset position complete!");

            resetInProgress = false;

            SetPosition();
        }
    }

    private void Rotate()
    {
        // Rotate the game object by 90 degrees around the up (Y) axis
        transform.Rotate(Vector3.forward, 90f);

        // Rotate() go counterclockwise
        ignoredCells = RotateArray(ignoredCells, false);

        // Swapping the width and the height
        int Temporary = width;
        width = height;
        height = Temporary;
    }

    private void Unrotate()
    {
        // Rotate the game object by 90 degrees around the up (Y) axis
        transform.Rotate(Vector3.forward, -90f);

        // Rotate() go clockwise
        ignoredCells = RotateArray(ignoredCells, true);

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
                Reset();

                inTrunk = false;
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

    private Vector2 GetSnappedPosition()
    {
        // Round the position to the nearest snapIncrement
        Vector2 roundedPosition = RoundPosition(transform.position, snapIncrement);

        // If the width or height is an odd value, correct it so that it's not taking up half a cell

        float xCorrection = (width % 2f) / 2f;
        float yCorrection = (height % 2f) / 2f;

        // Mathf.Round rounds to even numbers if the number is 0.5, so this makes it round sensibly
        roundedPosition = new Vector2(roundedPosition.x + xCorrection - 0.001f, roundedPosition.y + yCorrection - 0.001f);

        return roundedPosition;
    }

    private void UpdateGridPositions()
    {
        gridPosition = WorldToGridCoorindates(GetSnappedPosition(), width, height);
    }

    private Vector2 WorldToGridCoorindates(Vector2 Coords, float w, float h)
    {
        // The gridPosition of each object is at its bottom left corner. This is obtained by subtracting half the width and half the height from the snapped position.
        return new Vector2(Mathf.RoundToInt(Coords.x - (w / 2)), Mathf.RoundToInt(Coords.y - (h / 2)));
    }

    // CheckPosition takes in two values for where in relation to the gridPosition it should check
    private bool CheckPosition(int xShift, int yShift)
    {
        UpdateGridPositions();

        // If we're in our starting position, then that's a valid position to be in
        if (Vector3.Distance(transform.position, startingPos) < 0.01f && xShift == 0 && yShift == 0)
        {
            return true;
        }

        if (grid.CheckIfInGrid(new Vector2(gridPosition.x + xShift, gridPosition.y + yShift), width, height, ignoredCells))
        {
            return grid.CheckPosition(new Vector2(gridPosition.x + xShift, gridPosition.y + yShift), width, height, ignoredCells);
        }
        else
        {
            return false;
        }
    }

    private void SetPosition()
    {
        UpdateGridPositions();

        if (grid.CheckIfInGrid(gridPosition, width, height, ignoredCells))
        {
            inTrunk = true;

            grid.CheckWin();

            grid.SetPosition(gridPosition, width, height, ignoredCells);
        }
    }

    private void ClearPosition()
    {
        if (grid.CheckIfInGrid(gridPosition, width, height, ignoredCells) && inTrunk)
        {
            grid.ClearPosition(gridPosition, width, height, ignoredCells);
        }
    }

    // Populating the ignoredCells using the values assigned to the ignoredCellsTable
    private bool[,] PopulateArray()
    {
        ignoredCells = new bool[width, height];

        // For each cell in the ignoredCellsTable, copy it into ignored Cells
        for (int j = 0; j < ignoredCells.GetLength(1); j++)
        {
            for (int i = 0; i < ignoredCells.GetLength(0); i++)
            {
                ignoredCells[i, j] = rows[i].column[j];
            }
        }

        return ignoredCells;
    }

    // Rotates the ignoredCells array by swapping the width and the height and saving the values of the original array to a new array.
    // The bool determines which direction the function rotates the array in
    private bool[,] RotateArray(bool[,] originalArray, bool clockwise)
    {
        int rows = originalArray.GetLength(0);
        int columns = originalArray.GetLength(1);

        bool[,] rotatedArray = new bool[columns, rows];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if(!clockwise)
                {
                    // Counterclockwise rotation
                    rotatedArray[columns - 1 - j, i] = originalArray[i, j];
                }
                else
                {
                    // Clockwise rotation
                    rotatedArray[j, rows - 1 - i] = originalArray[i, j];
                }
            }
        }

        return rotatedArray;
    }
}