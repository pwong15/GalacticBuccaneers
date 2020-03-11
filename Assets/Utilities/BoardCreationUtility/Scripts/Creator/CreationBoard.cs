using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreationBoard : Board

{
    public int xCoord { get; }
    public int yCoord { get; }

    public override void CreateGrid()
    {
        boardAsTextSymbols = new string[GRID_WIDTH, GRID_HEIGHT];
        gridSquares = new GridSquare[GRID_WIDTH, GRID_HEIGHT];
        string saveFile = Directory.GetCurrentDirectory() + "\\Assets\\Utilities\\BoardCreationUtility\\output\\" + FILE_NAME + ".txt";

        for (int row = 0; row < GRID_HEIGHT; row++)
        {
            for (int column = 0; column < GRID_WIDTH; column++)
            {
                GameObject gridVisual = Instantiate(Resources.Load("Prefabs/opaqueSquare") as GameObject);
                GridSquare gridSquare = gridVisual.AddComponent<GridSquare>();
                gridSquare.Initialize(this, column, -row, -1);
                gridSquares[column, row] = gridSquare;
                boardAsTextSymbols[column, row] = ".";
            }
        }
    }

    void OnApplicationQuit()
    {
        string boardAsAString = "";
        for (int row = 0; row < GRID_HEIGHT; row++)
        {
            boardAsAString += "\n";
            for (int column = 0; column < GRID_WIDTH; column++)
            {
                boardAsAString += boardAsTextSymbols[column, row];
            }
        }

        string fileLocation = Directory.GetCurrentDirectory() + "\\Assets\\Utilities\\BoardCreationUtility\\output\\" + FILE_NAME + ".txt";
        File.WriteAllText(fileLocation, boardAsAString);
    }

    public void MarkSquare(int column, int row, string symbol)
    {
        boardAsTextSymbols[column, row] = symbol;
    }
}


