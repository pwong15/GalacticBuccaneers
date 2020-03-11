using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;

public class Board : MonoBehaviour
{
    private readonly int GRID_HEIGHT = 11;
    private readonly int GRID_WIDTH = 16;
    //private readonly string MAP_NAME = "LatestMap";
    private readonly string MAP_NAME = "Test";
    string[,] boardAsTextSymbols;
    //private List<Tile> _neighbors;
    public Grid grid { get; }
    public GridSquare[,] boardGridSquares;
    public int xCoord { get; }
    public int yCoord { get; }


    void Start()
    {
        CreateGrid();
    }


    private void CreateGrid()
    {
        boardAsTextSymbols = new string[GRID_WIDTH, GRID_HEIGHT];
        boardGridSquares = new GridSquare[GRID_WIDTH, GRID_HEIGHT];
        string saveFile = Directory.GetCurrentDirectory() + "\\Assets\\Utilities\\BoardCreationUtility\\output\\" + MAP_NAME + ".txt";
        //string wallLayout = string.Join("", File.ReadAllLines(saveFile));
        int wallIndex = 0;

        for (int row = 0; row < GRID_HEIGHT; row++)
        {
            for (int column = 0; column < GRID_WIDTH; column++)
            {
                GameObject gridVisual = Instantiate(Resources.Load("Prefabs/opaqueSquare") as GameObject);
                GridSquare gridSquare = gridVisual.AddComponent<GridSquare>();
                //gridSquare.Initialize(this, column, -row, -1, wallLayout[wallIndex++]);
                gridSquare.Initialize(this, column, -row, -1);
                boardGridSquares[column, row] = gridSquare;
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

        string fileLocation = Directory.GetCurrentDirectory() + "\\Assets\\Utilities\\BoardCreationUtility\\output\\" + MAP_NAME + ".txt";
        File.WriteAllText(fileLocation, boardAsAString);
    }

    public void MarkSquare(int column, int row, string symbol)
    {
        boardAsTextSymbols[column, row] = symbol;
    }
}
//public class Tile
//{
//    int xCoord;
//    int yCoord;
    
//    public Tile(Grid g, int x, int y)
//{
//    xCoord = x;
//    yCoord = y;
//    grid = g;
//    this.Terrain = new Terrain(Terrain.Sprite.None);
//}

//public List<Tile> Neighbors
//{
//    get
//    {
//        if (_neighbors != null)
//        {
//            return _neighbors;
//        }
//        _neighbors = new List<Tile>();
//        Tile tile;
//        Tuple<int, int>[] possibleMoves = {
//            Tuple.Create(0, 1),
//            Tuple.Create(1, 0),
//            Tuple.Create(-1, 0),
//            Tuple.Create(0, -1),
//        };
//        for (int i = 0; i < 4; i++)
//        {
//            Tuple<int, int> move = possibleMoves[i];
//            tile = grid.GetGridObject(grid.GetWorldPosition(xCoord + move.Item1, yCoord + move.Item2));
//            if (tile != default(Tile))
//            {
//                _neighbors.Add(tile);
//            }
//        }
//        return _neighbors;
//    }
//}





