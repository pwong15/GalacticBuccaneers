using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private readonly float MAX_ZOOM_IN = 1.4f;
    private readonly float MAX_ZOOM_OUT = 36.0f;

    void Update()
    {
        listenForCameraZoom();
        listenForCameraPan();
    }

    // Move camera right, left, up, down with wasd/ By holding right mouse button and panning
    private void listenForCameraPan()
    {
        Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cameraSize = GetComponent<Camera>().orthographicSize;
        Vector3 panAmnt = (mouseLoc - transform.position);

        if (Input.GetMouseButtonDown(1))
        {
            transform.position += panAmnt;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3(currentPosition.x, currentPosition.y + 1, currentPosition.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3(currentPosition.x - 1, currentPosition.y, currentPosition.z);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3(currentPosition.x, currentPosition.y - 1, currentPosition.z);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3(currentPosition.x + 1, currentPosition.y, currentPosition.z);
        }
    }

    // Zoom camera in/out with mouse wheel
    private void listenForCameraZoom()
    {
        Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cameraSize = GetComponent<Camera>().orthographicSize;
        Vector3 panOutAmnt = -(mouseLoc - transform.position) / cameraSize;
        Vector3 panInAmnt = (mouseLoc - transform.position) / cameraSize;

        // Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            // Ensure you can't zoom to backside of map 
            if (cameraSize > MAX_ZOOM_IN)
            {
                transform.position += panInAmnt;
                GetComponent<Camera>().orthographicSize--;
            }
        }

        // Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            // Limit zoom out
            if (cameraSize < MAX_ZOOM_OUT)
            {
                transform.position += panOutAmnt;
                GetComponent<Camera>().orthographicSize++;
            }  
        }
    }
}

