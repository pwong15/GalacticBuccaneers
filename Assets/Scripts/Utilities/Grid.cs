using UnityEngine;

public class Grid : MonoBehaviour
{
    protected int GRID_HEIGHT = 22;
    protected int GRID_WIDTH = 22;
    protected string FILE_NAME = "LatestMap";
    public GridSquare[,] gridSquares;
    protected string[,] gridAsTextSymbols;

    public virtual void CreateGrid() { }
}




