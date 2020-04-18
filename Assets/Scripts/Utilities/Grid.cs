using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    protected int GRID_HEIGHT = 22;
    protected int GRID_WIDTH = 22;
    protected string FILE_NAME = "LatestMap";
    public GridSquare[,] gridSquares;
    protected string[,] gridAsTextSymbols;
    public Dictionary<char, List<char>> validSelections = new Dictionary<char, List<char>>();
    public Dictionary<char, List<string>> pathAssociations = new Dictionary<char, List<string>>();
    public char currentLocation = '1';


    public virtual void CreateGrid() { }
    public virtual void RemoveFog(char removalKey) { }
    public virtual void ShowPaths(char ship) { }
    public virtual void SaveFog() { }
    public virtual void SaveLocation() { }
}




