using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TxtCreator
{
    public class GridSquare : global::GridSquare
    {
        int highlightIndex = 0;
        Grid grid;
        private List<Color> colors = new List<Color> { Color.blue, Color.green, Color.red, Color.white };

        // The symbol each color writes to file
        private Dictionary<Color, string> colorToString = new Dictionary<Color, string>{
        {Color.blue, "w"},
        {Color.green, "_" },
        {Color.red,"|"},
        {Color.white, "."} };

        private void Start()
        {
            grid = (Grid)base.grid;
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
                grid.MarkSquare(column, row, this.colorToString[currColor]);
            }

            // Right click
            if (Input.GetMouseButtonDown(1))
            {
                // Wrap to other side if at end of list
                if (highlightIndex == 0)
                    highlightIndex = this.colors.Count;

                Color currColor = colors[--highlightIndex];
                rend.material.color = currColor;
                grid.MarkSquare(column, row, this.colorToString[currColor]);
            }
        }
    }
}


