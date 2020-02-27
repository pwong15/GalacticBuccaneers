using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;

public class Board : MonoBehaviour
{
    private readonly int GRID_HEIGHT = 32;
    private readonly int GRID_WIDTH = 24;
    //private readonly string MAP_NAME = "LatestMap";
    private readonly string MAP_NAME = "Layout2";
    string[,] wallLayoutArray;
    //private List<Tile> _neighbors;
    //public Grid grid { get; }
    public GridSquare[,] tiles;
    //public int xCoord { get; }
    //public int yCoord { get; }


    void Start()
    {
        CreateGrid();
    }


    private void CreateGrid()
    {
        wallLayoutArray = new string[GRID_WIDTH, GRID_HEIGHT];
        tiles = new GridSquare[GRID_WIDTH, GRID_HEIGHT];
        string wallLayoutFile = Directory.GetCurrentDirectory() + "\\Assets\\Utilities\\BoardCreationUtility\\output\\" + MAP_NAME + ".txt";
        string wallLayout = string.Join("", File.ReadAllLines(wallLayoutFile));
        int wallIndex = 0;

        for (int row = 0; row < GRID_HEIGHT; row++)
        {
            for (int column = 0; column < GRID_WIDTH; column++)
            {                
                GameObject gridSquare = Instantiate(Resources.Load("Prefabs/target") as GameObject);
                GridSquare tile = gridSquare.AddComponent<GridSquare>();
                tile.Initialize(this, column, -row, -1, wallLayout[wallIndex++]);
                tiles[column, row] = tile;
                wallLayoutArray[column, row] = ".";
            }
        }
    }


    //void OnApplicationQuit()
    //{
    //    string boardAsAString = "";
    //    for (int row = 0; row < GRID_HEIGHT; row++)
    //    {
    //        boardAsAString += "\n";
    //        for (int column = 0; column < GRID_WIDTH; column++)
    //        {
    //            boardAsAString += gameBoard[column, row];
    //        }
    //    }

    //    string fileLocation = Directory.GetCurrentDirectory() + "\\Assets\\Utilities\\BoardCreationUtility\\output\\" + MAP_NAME + ".txt";
    //    Debug.Log(fileLocation);
    //    File.WriteAllText(fileLocation, boardAsAString);
    //}

    //public void MarkSquareAsWall(int column, int row)
    //{
    //    gameBoard[column, row] = "w";
    //}

    //public void UnMarkSquareAsWall(int column, int row)
    //{
    //    gameBoard[column, row] = ".";
    //}
}
    //public class Tile
    //{
    //    public Tile(Gridg, int x, int y)
    //    {
    //        xCoord = x;
    //        yCoord = y;
    //        grid = g;
    //        this.Terrain = new Terrain(Terrain.Sprite.None);
    //    }

    //    public List<Tile> Neighbors
    //    {
    //        get
    //        {
    //            if (_neighbors != null)
    //            {
    //                return _neighbors;
    //            }
    //            _neighbors = new List<Tile>();
    //            Tile tile;
    //            Tuple<int, int>[] possibleMoves = {
    //            Tuple.Create(0, 1),
    //            Tuple.Create(1, 0),
    //            Tuple.Create(-1, 0),
    //            Tuple.Create(0, -1),
    //        };
    //            for (int i = 0; i < 4; i++)
    //            {
    //                Tuple<int, int> move = possibleMoves[i];
    //                tile = grid.GetGridObject(grid.GetWorldPosition(xCoord + move.Item1, yCoord + move.Item2));
    //                if (tile != default(Tile))
    //                {
    //                    _neighbors.Add(tile);
    //                }
    //            }
    //            return _neighbors;
    //        }
    //    }
//}




