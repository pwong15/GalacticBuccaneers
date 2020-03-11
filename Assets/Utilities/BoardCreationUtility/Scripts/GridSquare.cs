using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    private float xCoordf, yCoordf, zCoordf;
    private int column, row, zCoord;
    Renderer rend;
    Board board;
    int highlightIndex = 0;

    private List<Color> colors = new List<Color> {Color.blue, Color.green, Color.red, Color.white};

    // The symbol each color writes to file
    private Dictionary<Color, string> colorToStrTable= new Dictionary<Color, string>{
        {Color.blue, "w"},
        {Color.green, "_" },
        {Color.red,"|"},
        {Color.white, "*"} };


    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void OnMouseOver()
    {
        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            // Wrap to other side if at end of list
            if (highlightIndex == this.colors.Count)
                highlightIndex = 0;
            Color currColor = colors[highlightIndex++];
            rend.material.color = currColor;
            board.MarkSquare(column, row, this.colorToStrTable[currColor]);
        }

        // Right click
        if (Input.GetMouseButtonDown(1)){
            // Wrap to other side if at end of list
            if (highlightIndex == 0)
                highlightIndex = this.colors.Count;
            Color currColor = colors[--highlightIndex];
            rend.material.color = currColor;
            board.MarkSquare(column, row, this.colorToStrTable[currColor]);
        }
    }

    // Creates an empty tile, sets its location, adds a box collider
    public void Initialize(Board gameBoard, int xLocation, int yLocation, int zLocation)
    {
        this.xCoordf = xLocation;
        this.yCoordf = yLocation;
        this.zCoordf = zLocation;

        this.column = xLocation;
        this.row = -yLocation;

        this.board = gameBoard;
        this.gameObject.AddComponent(typeof(BoxCollider));
        this.transform.position = new Vector3(xCoordf, yCoordf, zCoordf);

    }

    public override string ToString()
    {
        return column + "," + row;
    }
}


