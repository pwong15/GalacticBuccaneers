using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndGameLauncher : MonoBehaviour
{
    protected Renderer rend;

    public void Start() {
        rend = GetComponent<Renderer>();

    }
    void Update()
    {
        Vector3 cursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool cursorIsOnTile = CursorIsOnTile(cursorLocation.x - 16, cursorLocation.y + 19.5f);

        if (cursorIsOnTile && EarthIsValid()) {
            rend.material.color = Color.green;
        } 
        else {
            rend.material.color = Color.white;
        }

        // Register Earth was clicked
        if (cursorIsOnTile && Input.GetMouseButtonDown(0) && EarthIsValid()) {
            SceneController.LoadScene("Ship2");
        }
    }

    // Returns a bool indicating if the cursor is hover on tile
    private bool CursorIsOnTile(float mouseXLocation, float mouseYLocation) {
        if ((-1.5f < mouseXLocation && mouseXLocation < 1.5) && (-1.5 < mouseYLocation && mouseYLocation < 1.5)) {
            return true;
        }
        return false;
    }

    private bool EarthIsValid() {
        GameObject placeMarker = GameObject.Find("PlaceMarker");
        
        // Ensure on an adjacent ship
        if(placeMarker.transform.position.y < -36.0f && placeMarker.transform.position.x > 0) {
            return true;
        }

        return false;
    }
}
