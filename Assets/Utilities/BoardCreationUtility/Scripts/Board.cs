using System.IO;
using UnityEngine;

public class Board : MonoBehaviour
{
    private readonly int GRID_HEIGHT = 32;
    private readonly int GRID_WIDTH = 24;
    private readonly string MAP_NAME = "LatestMap";
    string[,] gameBoard;



    void Start()
    {
        CreateGrid();
    }

    void OnApplicationQuit()
    {
        string boardAsAString = "";
        for (int row = 0; row < GRID_HEIGHT; row++)
        {
            boardAsAString += "\n";
            for (int column = 0; column < GRID_WIDTH; column++)
            {
                boardAsAString += gameBoard[column, row];
            }
        }

        string fileLocation = Directory.GetCurrentDirectory() + "\\Assets\\Utilities\\BoardCreationUtility\\output\\" + MAP_NAME + ".txt";
        Debug.Log(fileLocation);
        File.WriteAllText(fileLocation, boardAsAString);
    }

    public void MarkSquareAsWall(int column, int row)
    {
        gameBoard[column, row] = "w";
    }

    public void UnMarkSquareAsWall(int column, int row)
    {
        gameBoard[column, row] = ".";
    }

    private void CreateGrid()
    {
        gameBoard = new string[GRID_WIDTH, GRID_HEIGHT];

        for (int row = 0; row < GRID_HEIGHT; row++)
        {
            for (int column = 0; column < GRID_WIDTH; column++)
            {
                GameObject gridSquare = Instantiate(Resources.Load("Prefabs/opaqueSquare") as GameObject);
                GridSquare emptyTile = gridSquare.AddComponent<GridSquare>();
                emptyTile.Initialize(this, column, -row, -1);

                gameBoard[column, row] = ".";
            }
        }
    }
}
