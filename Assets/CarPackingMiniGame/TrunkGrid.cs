using UnityEngine;

public class TrunkGrid : MonoBehaviour
{
    public bool[,] grid;

    public int xUpperBound;
    public int yUpperBound;

    private float trueCells;
    private float falseCells;

    private SnapToGrid[] children;

    public GameObject trunkDoor;

    private CarPackingMiniGame minigame;

    private void Start()
    {
        minigame = GetComponentInParent<CarPackingMiniGame>();

        grid = new bool[xUpperBound, yUpperBound];

        children = GetComponentsInChildren<SnapToGrid>();
    }

    private void Update()
    {
        // Press [G] to debug how many cells are marked as unoccupied (false) or occupied (true).

        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (grid[i, j] == true)
                    {
                        trueCells++;
                    }
                    if (grid[i, j] == false)
                    {
                        falseCells++;
                    }
                }
            }

            Debug.Log("Trues: " + trueCells + ". Falses: " + falseCells + ".");

            trueCells = 0;
            falseCells = 0;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (grid[i, j] == true)
                    {
                        Debug.Log("Cell (" + i + ", " + j + ")");
                    }
                }
            }
        }
    }

    public bool CheckPosition(Vector2 pos, float w, float h, bool[,] ignores)
    {
        // Loops through each cell taken up by the object passed in, checking if any of the cells are occupied.

        for (int i = (int)pos.x; i < (int)pos.x + w; i++)
        {
            for (int j = (int)pos.y; j < (int)pos.y + h; j++)
            {
                if (!ignores[i - (int)pos.x, j - (int)pos.y] && grid[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void SetPosition(Vector2 pos, float w, float h, bool[,] ignores)
    {
        // Loops through each cell taken up by the object passed in, setting the cells as occupied.

        for (int i = (int)pos.x; i < (int)pos.x + w; i++)
        {
            for (int j = (int)pos.y; j < (int)pos.y + h; j++)
            {
                if (!ignores[i - (int)pos.x, j - (int)pos.y])
                {
                    grid[i, j] = true;

                    //Debug.Log("Cell at " + i + ", " + j + " filled");
                }
            }
        }
    }

    public void ClearPosition(Vector2 pos, float w, float h, bool[,] ignores)
    {
        // Loops through each cell taken up by the object passed in, setting the cells as unoccupied.

        for (int i = (int)pos.x; i < (int)pos.x + w; i++)
        {
            for (int j = (int)pos.y; j < (int)pos.y + h; j++)
            {
                if (!ignores[i - (int)pos.x, j - (int)pos.y])
                {
                    grid[i, j] = false;

                    //Debug.Log("Cell at " + i + ", " + j + " cleared");
                }
            }
        }
    }

    public bool CheckIfInGrid(Vector2 pos, float w, float h, bool[,] ignores)
    {
        // Loops through each cell taken up by the object passed in, checking if any of the cells are not in the grid.

        for (int i = (int)pos.x; i < (int)pos.x + w; i++)
        {
            for (int j = (int)pos.y; j < (int)pos.y + h; j++)
            {
                if (!ignores[i - (int)pos.x, j - (int)pos.y])
                {
                    if (i < 0 || i >= grid.GetLength(0) || j < 0 || j >= grid.GetLength(1))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public void CheckWin()
    {
        foreach (SnapToGrid child in children)
        {
            if(!child.inTrunk)
            {
                return;
            }
        }

        foreach (SnapToGrid child in children)
        {
            child.gameObject.GetComponent<Collider2D>().enabled = false;
        }

        Debug.Log("You win!!!!!");
        minigame.Win();
    }
}