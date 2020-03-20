using System.IO;
using UnityEngine;


namespace GalaxyMap
{
    public class Grid : global::Grid
    {
        public void Start()
        {
            this.GRID_HEIGHT = 22;
            this.GRID_HEIGHT = 22;
            this.FILE_NAME = "GalaxyMap";

            CreateGrid();
        }

        public override void CreateGrid()
        {
            int gridIndex = 0;
            gridSquares = new GridSquare[GRID_WIDTH, GRID_HEIGHT];
            string loadFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\" + FILE_NAME + ".txt";
            string gridValues = string.Join("", File.ReadAllLines(loadFile));
            string sceneLink = "";

            // Create a GridSquare for each posistion on the board
            for (int row = 0; row < GRID_HEIGHT; row++){
            for (int column = 0; column < GRID_WIDTH; column++)
            {
                sceneLink = "";
                GameObject gridVisual;
                char gridValue = gridValues[gridIndex];
                // Load the appropriate prefab based on the .txt file
                switch (gridValue)
                {
                    case 'w':
                        gridVisual = Instantiate(Resources.Load("Prefabs/ship1") as GameObject);
                        sceneLink = "Ship1";
                        break;
                    case '.':
                        gridVisual = Instantiate(Resources.Load("Prefabs/blankSquare") as GameObject);
                        break;
                    case '|':
                        gridVisual = Instantiate(Resources.Load("Prefabs/ship2") as GameObject);
                        sceneLink = "Ship1";
                        break;
                    default:
                        gridVisual = Instantiate(Resources.Load("Prefabs/blankSquare") as GameObject);
                        break;

                }

                // Initialize and save each GridSquare
                GridSquare gridSquare = gridVisual.AddComponent<GridSquare>();
                gridSquare.Initialize(this, column, -row, -3);
                gridSquare.LinkedScene = sceneLink;
                gridSquares[column, row] = gridSquare;
                gridIndex++;
            }
        }
        }
    }
}
