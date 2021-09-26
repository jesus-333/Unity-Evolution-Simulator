using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{

    public float scroll_speed = 10f;
    public Camera camera;

    private Vector3 start_point, end_point;

    void Start()
    {

    }

    void Update()
    {

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Zoom
        if(camera.orthographic){
            camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scroll_speed;
        } else {
            if(camera.fieldOfView >= 1) { camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * scroll_speed; }
            else { camera.fieldOfView = 1f; }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Move
        // if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
        //     camera.transform.position += new Vector3(0, 0, 1f);
        // } else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
        //     camera.transform.position += new Vector3(0, 0, -1f);
        // } else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
        //     camera.transform.position += new Vector3(-1f, 0, 0);
        // } else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
        //     camera.transform.position += new Vector3(+1f, 0, 0);
        // }
        Vector3 camera_translation = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * camera.fieldOfView / 60;
        camera.transform.Translate(camera_translation);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Draw safe zone
        if(Input.GetMouseButtonDown(0)){
            print(Input.mousePosition);
            print(camera.ViewportToWorldPoint(Input.mousePosition));
        }

        if(Input.GetMouseButton(0)){

        }
    }

    // Function use to convert the click of the mouse in the relative position in the world.
    // All the calculation are simply a projection to the xz plane with y = 0
    Vector3 EvaluateMouseCoordinate(){
        float width = Screen.width, height = Screen.height;
        Vector3 point, tmp_mouse_position;;

        point = new Vector3(0,0,0);
        // Get mouse position
        tmp_mouse_position = Input.mousePosition;

        return point;
    }
}
