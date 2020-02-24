using UnityEngine;

namespace Components { 
public class CameraController : MonoBehaviour {
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float arrowSensitivity;
    private Camera boardCamera;

    // Start is called before the first frame update
    private void Start() {
        GetComponent<Transform>().position = new Vector3(x / 2, y / 2, -10);
        boardCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            boardCamera.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * mouseSensitivity;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            boardCamera.transform.position += Vector3.left * arrowSensitivity;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            boardCamera.transform.position += Vector3.right * arrowSensitivity;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            boardCamera.transform.position += Vector3.down * arrowSensitivity;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            boardCamera.transform.position += Vector3.up * arrowSensitivity;
        }
    }
}
}