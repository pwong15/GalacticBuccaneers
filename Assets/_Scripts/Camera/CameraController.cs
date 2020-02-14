using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Components;

public class CameraController : MonoBehaviour
{
    [SerializeField] int x;
    [SerializeField] int y;
    [SerializeField] float mouseSensitivity;
    Camera boardCamera;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Transform>().position = new Vector3(x / 2, y / 2, -10);
        boardCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            boardCamera.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * mouseSensitivity;
        }

    }
}
