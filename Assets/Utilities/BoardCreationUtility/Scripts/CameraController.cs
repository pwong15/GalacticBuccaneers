using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private readonly float MAX_ZOOM_IN = 1.4f;
    private readonly float MAX_ZOOM_OUT = 36.0f;
    // Update is called once per frame
    void Update()
    {
        listenForCameraZoom();
        listenForCameraPan();
    }

    // Move camera right, left, up, down with wasd
    private void listenForCameraPan()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3(currentPosition.x, currentPosition.y + 5, currentPosition.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3(currentPosition.x - 5, currentPosition.y, currentPosition.z);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3(currentPosition.x, currentPosition.y - 5, currentPosition.z);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3(currentPosition.x + 5, currentPosition.y, currentPosition.z);
        }
    }

    // Zoom camera in/out with mouse wheel
    private void listenForCameraZoom()
    {
        // Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            var cameraLocation = GetComponent<Camera>().orthographicSize;

            if (cameraLocation > MAX_ZOOM_IN)  // Ensure you can't zoom to backside of map 
            {
                GetComponent<Camera>().orthographicSize--;
            }
        }

        // Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            var cameraLocation = GetComponent<Camera>().orthographicSize;

            if (cameraLocation < MAX_ZOOM_OUT)  // Zooming farther out stretches the grid 
            {
                GetComponent<Camera>().orthographicSize++;

            }
        }
    }
}

