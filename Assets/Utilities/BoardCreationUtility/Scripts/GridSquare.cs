using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class GridSquare : MonoBehaviour
{
    private float xCoordinate, yCoordinate, zCoordinate;
    Renderer rend;
    Board gameBoard;
    bool isHighlighted = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void OnMouseDown()
    {
        if (!isHighlighted)
        {
            Highlight();
            Debug.Log(this.xCoordinate + " " + this.yCoordinate);
            int column = GridConverter.RoundXCoordToInt(xCoordinate);
            int row = GridConverter.RoundYCoordToPosInt(yCoordinate);

            gameBoard.MarkSquareAsWall(column, row);
        }
        else
        {
            UnHighlight();
            Debug.Log("Unmarked " + this.xCoordinate + " " + this.yCoordinate);
            int column = GridConverter.RoundXCoordToInt(xCoordinate);
            int row = GridConverter.RoundYCoordToPosInt(yCoordinate);

            gameBoard.UnMarkSquareAsWall(column, row);
        }
    }

    // Changes the tile color to green
    public void Highlight()
    {
        rend.material.color = Color.green;
        isHighlighted = true;
    }

    public void UnHighlight()
    {
        isHighlighted = false;
        rend.material.color = Color.white;
    }

    // Creates an empty tile, sets its location, adds a box collider
    public void Initialize(Board gameBoard, float xLocation, float yLocation, float zLocation)
    {
        this.xCoordinate = xLocation;
        this.yCoordinate = yLocation;
        this.zCoordinate = zLocation;
        this.gameBoard = gameBoard;
        this.gameObject.AddComponent(typeof(BoxCollider));
        this.transform.position = new Vector3(xCoordinate, yCoordinate, zCoordinate);
    }
}

