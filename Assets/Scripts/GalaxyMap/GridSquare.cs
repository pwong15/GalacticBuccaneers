using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GalaxyMap
{
    public class GridSquare : global::GridSquare
    {
        private string linkedScene = "";

        public string LinkedScene { set {linkedScene = value; } }

        private void OnMouseOver()
        {
            // Left click
            if (Input.GetMouseButtonDown(0) && linkedScene != "")
            {
                rend.material.color = Color.yellow;
                SceneController.LoadScene(linkedScene);
            }
        }
    }
}

