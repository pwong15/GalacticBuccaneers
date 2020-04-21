using System.IO;
using UnityEngine;

namespace TxtCreator
{
    public class Grid : global::Grid

    {
        public int xCoord { get; }
        public int yCoord { get; }

        public void Start()
        {
            this.GRID_HEIGHT = 22;
            this.GRID_HEIGHT = 22;
            this.FILE_NAME = "LatestMap";

            CreateGrid();
        }

        public override void CreateGrid()
        {
            gridAsTextSymbols = new string[GRID_WIDTH, GRID_HEIGHT];
            gridSquares = new GridSquare[GRID_WIDTH, GRID_HEIGHT];
            string saveFile = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxTFiles\\output\\" + FILE_NAME + ".txt";

            for (int row = 0; row < GRID_HEIGHT; row++)
            {
                for (int column = 0; column < GRID_WIDTH; column++)
                {
                    GameObject gridVisual = Instantiate(Resources.Load("Prefabs/opaqueSquare") as GameObject);
                    GridSquare gridSquare = gridVisual.AddComponent<GridSquare>();
                    gridSquare.Initialize(this, column, -row, -1);
                    gridSquares[column, row] = gridSquare;
                    gridAsTextSymbols[column, row] = ".";
                }
            }
        }

        void OnApplicationQuit()
        {
            string fileLocation = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\BoardTxtFiles\\" + FILE_NAME + ".txt";
            File.WriteAllText(fileLocation, string.Empty);
        }

        public void MarkSquare(int column, int row, string symbol)
        {
            gridAsTextSymbols[column, row] = symbol;
        }
    }
}

