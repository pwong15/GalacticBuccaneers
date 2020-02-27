using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    private float xCoordf, yCoordf, zCoordf;
    private int column, row, zCoord;
    Renderer rend;
    Board gameBoard;
    List<GridSquare> _neighbors;
    bool canMoveUp = true, canMoveDown = true, canMoveRght = true, canMoveLft = true, isWall = false;
    //bool isHighlighted = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        CheckForCursorHover();
    }

    public void OnMouseDown()
    {
        List<GridSquare> adjacencies = GetNeighbors();
        Debug.Log("Clicked: " + this.ToString());
        foreach(GridSquare g in adjacencies)
        {
            Debug.Log("adjacency: " + g.ToString());
        }
        //if (!isHighlighted)
        //{
        //    Highlight();
        //    Debug.Log(this.xCoordinate + " " + this.yCoordinate);
        //    int column = GridConverter.RoundXCoordToInt(xCoordinate);
        //    int row = GridConverter.RoundYCoordToPosInt(yCoordinate);

        //    gameBoard.MarkSquareAsWall(column, row);
        //}
        //else
        //{
        //    UnHighlight();
        //    Debug.Log("Unmarked " + this.xCoordinate + " " + this.yCoordinate);
        //    int column = GridConverter.RoundXCoordToInt(xCoordinate);
        //    int row = GridConverter.RoundYCoordToPosInt(yCoordinate);

        //    gameBoard.UnMarkSquareAsWall(column, row);
        //}
    }

    //// Changes the tile color to green
    //public void Highlight()
    //{
    //    rend.material.color = Color.green;
    //    isHighlighted = true;
    //}

    //public void UnHighlight()
    //{
    //    isHighlighted = false;
    //    rend.material.color = Color.white;
    //}

    // Creates an empty tile, sets its location, adds a box collider
    public void Initialize(Board gameBoard, int xLocation, int yLocation, int zLocation, char layoutSymbol)
    {
        this.xCoordf = xLocation;
        this.yCoordf = yLocation;
        this.zCoordf = zLocation;

        this.column = xLocation;
        this.row = -yLocation;

        this.gameBoard = gameBoard;
        this.gameObject.AddComponent(typeof(BoxCollider));
        this.transform.position = new Vector3(xCoordf, yCoordf, zCoordf);
        GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        LocateWalls(layoutSymbol);
    }

    // Checks if the cursor is hovering on this tile
    private void CheckForCursorHover()
    {
        Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool cursorIsOnTile = CursorIsOnTile(cursorLocation.x - xCoordf, cursorLocation.y - yCoordf);

        // If cursor is inside the tile: highlight the tile
        if (cursorIsOnTile && !isWall)
        {
            GetComponent<Renderer>().enabled = true; 
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }

    

    // Returns a bool indicating if the cursor is hover on tile
    private bool CursorIsOnTile(float mouseXLocation, float mouseYLocation)
    {
        if ((-0.5f < mouseXLocation && mouseXLocation < 0.5) && (-0.5 < mouseYLocation && mouseYLocation < 0.5))
        {
            return true;
        }
        return false;
    }

    private void LocateWalls(char layoutChar)
    {
        switch (layoutChar)
        {
            case 'w':
                isWall = true;
                canMoveDown = false;
                canMoveLft = false;
                canMoveUp = false;
                canMoveRght = false;
                break;         
            case '-':   // - means half wall is at top of square
                canMoveUp = false;
                break;              
            case '|':   // | means half wall on left side of a square
                canMoveLft = false; 
                break;
            case '/':
                canMoveLft = false;
                canMoveUp = false;
                break;
            case '\\':
                canMoveUp = false;
                canMoveRght = false;
                break;
            case 'L':
                canMoveLft = false;
                canMoveDown = false;
                break;
            case '_':
                canMoveRght = false;
                canMoveDown = false;
                break;
            default:
                return;
        }
    }

   public List<GridSquare> GetNeighbors() {
        if (_neighbors != null)
            return _neighbors;

        _neighbors = new List<GridSquare>();
        //@TODO need to catch outof bounds exception
        GridSquare rightNeighb = gameBoard.tiles[column +1, row];  // column, row
        GridSquare leftNeighb = gameBoard.tiles[column -1, row];
        GridSquare aboveNeighb = gameBoard.tiles[column, row -1];
        GridSquare belowNeighb = gameBoard.tiles[column, row +1];

        if (this.canMoveRght && rightNeighb.canMoveLft)
            _neighbors.Add(rightNeighb);

        if (this.canMoveLft && leftNeighb.canMoveRght)
            _neighbors.Add(leftNeighb);

        if (this.canMoveDown && belowNeighb.canMoveUp)
            _neighbors.Add(belowNeighb);

        if (this.canMoveUp && aboveNeighb.canMoveDown)
            _neighbors.Add(aboveNeighb);

        return _neighbors;
    }

    public override string ToString() {
        return column + "," + row;
    }
}


