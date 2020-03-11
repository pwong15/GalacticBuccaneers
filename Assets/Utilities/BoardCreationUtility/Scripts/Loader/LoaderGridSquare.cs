using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderGridSquare : DefaultGridSquare
{
    private void OnMouseOver()
    {
        // Left click
        if (Input.GetMouseButtonDown(0))
            rend.material.color = Color.yellow;
    }
}

