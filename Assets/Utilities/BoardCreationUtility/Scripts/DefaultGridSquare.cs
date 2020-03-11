﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGridSquare : MonoBehaviour
{
    protected float xCoordf, yCoordf, zCoordf;
    protected int column, row, zCoord;
    protected Renderer rend;
    protected Board board;

    void Start()
    {
        rend = GetComponent<Renderer>();
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


