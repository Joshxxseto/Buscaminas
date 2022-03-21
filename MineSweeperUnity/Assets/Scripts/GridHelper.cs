using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelper : MonoBehaviour
{
    public static GridHelper sharedIsntance;
    public static int width = 15, height = 15;
    public static Cell[,] cells = new Cell[width, height];

    private Vector2 position;
    [SerializeField] private GameObject panel;

    void Awake()
    {
        if (!sharedIsntance) sharedIsntance = this;
        position = new Vector2(0, 0);
        generateCells();
    }

    void generateCells()
    {
        //Here x & y are going to be our coordinates in the unity Universe
        for (int y = 0; y < height; y++)
        {
            position.y = y;
            for (int x = 0; x < width; x++)
            {
                position.x = x;
                cells[x,y] = Instantiate(panel, position, Quaternion.identity, this.transform).GetComponent<Cell>();
            }
        }
    }

    public bool hasMineAt(int x, int y)
    {
        if (x < width && y < height && x >= 0 && y >= 0)//If im in playable range
        {
            Cell cellToEvaluate = cells[x, y];
            return cellToEvaluate.hasMine;
        }
        else //If outsisde the map
        {
            return false;
        }
    }

    public int CountAdjacentMines(int x, int y)
    {
        int count = 0;
        if (hasMineAt(x - 1, y)) count++;
        if (hasMineAt(x + 1, y)) count++;

        if (hasMineAt(x - 1, y + 1)) count++;
        if (hasMineAt(x + 0, y + 1)) count++;
        if (hasMineAt(x + 1, y + 1)) count++;

        if (hasMineAt(x - 1, y - 1)) count++;
        if (hasMineAt(x + 0, y - 1)) count++;
        if (hasMineAt(x + 1, y - 1)) count++;

        return count;
    }


    public void FloodFill(int x, int y,bool[,]visited)
    {
        if (x < width && y < height && x >= 0 && y >= 0)//If im in playable range
        {
            if (visited[x, y])
            {
                return;
            }
            else
            {
                //We put the textutre on the cell
                cells[x, y].SetTexture(CountAdjacentMines(x, y));
                //Set as visited
                visited[x, y] = true;
                //and if it has mines then, we skip this cell
                if (CountAdjacentMines(x, y) > 0)
                {
                    return;
                }
                else//otherwise, we will continue the algorithm
                {
                    /*
                     * Conectivity - 4
                     * 
                    FloodFill(x - 1, y, visited);//Left
                    FloodFill(x, y + 1, visited);//Up
                    FloodFill(x + 1, y, visited);//Right
                    FloodFill(x, y - 1, visited);//Down
                    */
                    //Conectivity - 4
                    FloodFill(x - 1, y + 0, visited);//Left
                    FloodFill(x - 1, y + 1, visited);//LeftUp
                    FloodFill(x + 0, y + 1, visited);//Up
                    FloodFill(x + 1, y + 1, visited);//RightUp
                    FloodFill(x + 1, y + 0, visited);//Right
                    FloodFill(x + 1, y - 1, visited);//RighDown
                    FloodFill(x + 0, y - 1, visited);//Down
                    FloodFill(x - 1, y - 1, visited);//LeftDown
                }
            }
        }
        else
        {
            return;
        }
    }

    public void UncoverAllTheMines()
    {
        foreach (Cell c in cells)
        {
            if (c.hasMine)
            {
                c.SetTexture(0);
            }
        }
    }

    public static bool HasTheGameEnded()
    {
        foreach (Cell cell in cells)
        {
            if (cell.isCovered && !cell.hasMine)
            {
                return false;
            }
        }
        return true;
    }
}
