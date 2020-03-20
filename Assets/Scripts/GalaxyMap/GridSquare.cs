using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GalaxyMap
{
    public class GridSquare : global::GridSquare
    {
        private string linkedScene = "";
        public Texture2D skull;

        public string LinkedScene { set {linkedScene = value; } }

        public void Start()
        {
            rend = GetComponent<Renderer>();
            skull = Resources.Load("skull") as Texture2D;
        }

        public void Update()
        {
            Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool cursorIsOnTile = CursorIsOnTile(cursorLocation.x - xCoordf, cursorLocation.y - yCoordf);

            if (!cursorIsOnTile) 
                rend.material.color = Color.white;

            if (linkedScene == "" && cursorIsOnTile)
                Cursor.SetCursor(null, cursorLocation, CursorMode.Auto);
        }

        private void OnMouseOver()
        {

            if (linkedScene != "")
            {
                Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cursorLocation.x += 25;
                cursorLocation.y += 25;
                Cursor.SetCursor(skull, cursorLocation, CursorMode.ForceSoftware);
            }

            rend.material.color = Color.gray;
            //Cursor.SetCursor(skull, cursorLocation, CursorMode.ForceSoftware);
            // Left click
            if (Input.GetMouseButtonDown(0) && linkedScene != "")
            {
                Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SceneController.LoadScene(linkedScene);
                Cursor.SetCursor(null, cursorLocation, CursorMode.Auto);
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
    }
}

