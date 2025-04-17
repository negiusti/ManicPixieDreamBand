using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;

public class SnapToGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public string blockTag = "Obstacle"; // The tag to avoid collisions with
    private Player player;
    public float snapIncrement = 0.3f;
    private Vector3 mousePositionOffset;
    private bool selected;

    private Vector3 startingPos;

    private Collider2D col;

    private int startingWidth;
    private int startingHeight;

    private float startingRot;

    public Vector2 gridPosition;

    public int width;
    public int height;

    private TrunkGrid grid;

    public bool inTrunk;

    // Width is cols, height is rows. This is the array in which the shape in the inspector will be stored into
    public bool[,] ignoredCells; // height, width -> rows, cols

    private float resetStartTime;
    private float resetDuration = 0.5f;
    private bool resetInProgress;
    private Animator anim;

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
        //transform.position = GetSnappedPosition();
        player = ReInput.players.GetPlayer(0);
        col = GetComponent<Collider2D>();

        startingPos = transform.position;

        startingRot = transform.localEulerAngles.z;

        startingWidth = width;
        startingHeight = height;

        ignoredCells = PopulateArray();

        grid = GetComponentInParent<TrunkGrid>();
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("InRange", true);
        }
    }

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("InRange", true);
        }
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();

        // When an object is grabbed, the cells it was occupying becomes unoccupied.
        ClearPosition();
    }

    private void OnMouseDrag()
    {
        //selected = true;
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

        if (Vector3.Distance(transform.position, startingPos) > 0.01f) // Approximately NOT equal
        {
            // Update the GameObject's position to the snapped position UNLESS its in the starting position
            transform.position = GetSnappedPosition();
        }

        // Rotate (Right Mouse Button) while NOT dragging
        if (selected && Input.GetMouseButtonDown(1) && !(Input.GetMouseButton(0) || player.GetButton("Select Object Under Cursor")))
        {
            ClearPosition();

            Rotate();

            // Call this here so that objects with odd widths or heights won't twitch after rotating because of how we adjust their positions
            transform.position = GetSnappedPosition();

            if (inTrunk && !CheckPosition(0, 0))
            {
                if (!ShiftIfNeeded(1))
                {
                    // Because of how we are snapping objects with odd widths and heights, sometimes it needs an extra push to shift it into the right position after rotating
                    if ((width % 2 == 1) || (height % 2 == 1))
                    {
                        if (!ShiftIfNeeded(2))
                        {
                            Unrotate();
                        }
                    }
                    else
                    {
                        Unrotate();
                    }
                }
            }

            SetPosition();
        }
    }

    // The shiftAmount is the number of cells this function shifts objects in
    // We need it because sometimes, objects with odd widths or lengths need to be shifted 2 cells to rotate them properly.
    private bool ShiftIfNeeded(int shiftAmount)
    {
        if (CheckPosition(0, shiftAmount))
        {
            // Move the object up
            transform.position = new Vector2(transform.position.x, transform.position.y + snapIncrement * shiftAmount);

            //Debug.Log("Up");

            SetPosition();

            return true;
        }
        else if (CheckPosition(0, -shiftAmount))
        {
            // Move the object down
            transform.position = new Vector2(transform.position.x, transform.position.y - snapIncrement * shiftAmount);

            //Debug.Log("Down");

            SetPosition();

            return true;
        }
        else if (CheckPosition(-shiftAmount, 0))
        {
            // Move the object left
            transform.position = new Vector2(transform.position.x - snapIncrement * shiftAmount, transform.position.y);

            //Debug.Log("Left");

            SetPosition();

            return true;
        }
        else if (CheckPosition(shiftAmount, 0))
        {
            // Move the object right
            transform.position = new Vector2(transform.position.x + snapIncrement * shiftAmount, transform.position.y);

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

        selected = false;
        col.enabled = false;

        transform.rotation = Quaternion.Euler(0, 0, startingRot);

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
            col.enabled = true;

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
        Debug.Log("Unrotating");

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
            if (!ShiftIfNeeded(1))
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

    private Vector3 RoundPosition(Vector3 position, float snapIncrement)
    {
        float x = SnapValue(position.x, snapIncrement);
        float y = SnapValue(position.y, snapIncrement);
        float z = SnapValue(position.z, snapIncrement);

        return new Vector3(x, y, z);
    }

    private Vector2 GetSnappedPosition()
    {
        // Round the position to the nearest snapIncrement
        Vector2 roundedPosition = RoundPosition(transform.position, snapIncrement);

        // If the width or height is an odd value, correct it so that it's not taking up half a cell
        // Mathf.Round rounds to even numbers if the number is 0.5, so subtracting a tiny value makes it round sensibly

        float xCorrection = ((width % 2f) / 2f) - 0.001f;
        float yCorrection = ((height % 2f) / 2f) - 0.001f;

        
        roundedPosition = new Vector2(roundedPosition.x + xCorrection, roundedPosition.y + yCorrection);

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
        if (Vector3.Distance(transform.position, startingPos) < 1f && xShift == 0 && yShift == 0)
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
        ignoredCells = new bool[height, width];

        // For each cell in the ignoredCellsTable, copy it into ignored Cells
        for (int j = 0; j < ignoredCells.GetLength(0); j++)
        {
            for (int i = 0; i < ignoredCells.GetLength(1); i++)
            {
                ignoredCells[j, i] = rows[j].column[i];
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
                if(clockwise)
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnMouseUp();
    }
}