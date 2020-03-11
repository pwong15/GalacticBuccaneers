using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Loader : Board
{
   
    public override void CreateGrid()
    {
        int gridIndex = 0;
        gridSquares = new DefaultGridSquare[GRID_WIDTH, GRID_HEIGHT];
        string saveFile = Directory.GetCurrentDirectory() + "\\Assets\\Utilities\\BoardCreationUtility\\output\\" + FILE_NAME + ".txt";
        string gridValues = string.Join("", File.ReadAllLines(saveFile));


        for (int row = 0; row < GRID_HEIGHT; row++)
        {
            for (int column = 0; column < GRID_WIDTH; column++)
            {
                GameObject gridVisual;
                char gridValue = gridValues[gridIndex];

                switch (gridValue)
                {
                    case 'w':
                        gridVisual = Instantiate(Resources.Load("Prefabs/ship1") as GameObject);
                        break;
                    case '.':
                        gridVisual = Instantiate(Resources.Load("Prefabs/blankSquare") as GameObject);
                        break;
                    case '|':
                        gridVisual = Instantiate(Resources.Load("Prefabs/ship2") as GameObject);
                        break;
                    default:
                        gridVisual = Instantiate(Resources.Load("Prefabs/blankSquare") as GameObject);
                        break;

                }

                LoaderGridSquare gridSquare = gridVisual.AddComponent<LoaderGridSquare>();
                gridSquare.Initialize(this, column, -row, -1);
                gridSquares[column, row] = gridSquare;
                gridIndex++;
            }
        }
    }
}
