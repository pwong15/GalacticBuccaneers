using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;

public class Board : MonoBehaviour
{
    protected readonly int GRID_HEIGHT = 22;
    protected readonly int GRID_WIDTH = 22;
    protected readonly string FILE_NAME = "Test";
    public DefaultGridSquare[,] gridSquares;
    protected string[,] boardAsTextSymbols;

    private void Start()
    {
        CreateGrid();
    }

    public virtual void CreateGrid() { }
}




