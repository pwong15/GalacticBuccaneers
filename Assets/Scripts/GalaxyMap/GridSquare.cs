using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GalaxyMap
{
    public class GridSquare : global::GridSquare
    {
        private string linkedScene = "";
        public Texture2D skull;
        public GameObject target;
        public string LinkedScene { set {linkedScene = value; } }
        public char shipChar; 

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

            if (!IsValidChoice() && cursorIsOnTile)
            {
                Cursor.SetCursor(null, cursorLocation, CursorMode.Auto);
            }
            if(IsValidChoice() && !cursorIsOnTile && target!= null)
            {
                target.SetActive(false);
            }
        }

        private void OnMouseOver()
        {
            // Show clickable ships
            if (IsValidChoice())
            {
                Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cursorLocation.x += 25;
                cursorLocation.y += 25;
                Cursor.SetCursor(skull, cursorLocation, CursorMode.ForceSoftware);

                // Create red targetting square if it doesnt exist
                if (target is null)
                {
                    target = Instantiate(Resources.Load("Prefabs/targetRed") as GameObject);
                    target.transform.position = this.transform.position;
                }

                // else just show red targetting square
                else
                {
                    target.SetActive(true);
                }
                rend.material.color = Color.gray;
            }

            // Launch Encounter on left click
            if (Input.GetMouseButtonDown(0) && IsValidChoice())
            {
                Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                grid.currentLocation = shipChar;

                // Move Green place marker
                GameObject greenPlaceMarker = GameObject.Find("PlaceMarker");
                Vector3 newPosition = new Vector3(xCoordf-11, yCoordf-23, 0);
                greenPlaceMarker.transform.position = newPosition;

                target.SetActive(false);
                grid.RemoveFog(shipChar);
                grid.ShowPaths(shipChar);
                LoadScene();
                Cursor.SetCursor(null, cursorLocation, CursorMode.Auto);
            }
        }

        // Indicates is ship encounter can currently be played
        public bool IsValidChoice() {
            if(grid.validSelections[grid.currentLocation].Contains(shipChar)) {
                return true;
            }
            return false;
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

        private void LoadScene() {
            grid.SaveFog();
            SceneController.LoadScene(linkedScene);
        }
    }
}

