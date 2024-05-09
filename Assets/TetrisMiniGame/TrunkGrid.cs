using UnityEngine;

public class TrunkGrid : MonoBehaviour
{
    public bool[,] grid = new bool[68, 38];

    private float trueCells;
    private float falseCells;

    private void Update()
    {
        // Press [G] to debug how many cells are marked as unoccupied (false) or occupied (true).

        if(Input.GetKeyDown(KeyCode.G))
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
    }

    public bool CheckPosition(Vector2 pos, float w, float h, bool[,] ignores)
    {
        // Loops through each cell taken up by the object passed in, checking if any of the cells are occupied.

        for (int i = (int)pos.x; i < (int)pos.x + w; i++)
        {
            for (int j = (int)pos.y; j < (int)pos.y + h; j++)
            {
                if(grid[i, j] && !ignores[i - (int)pos.x, j - (int)pos.y])
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
}
