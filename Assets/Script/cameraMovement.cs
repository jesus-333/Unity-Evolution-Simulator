using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{

    public float scroll_speed = 10f;
    public Camera camera_var;

    private Vector3 start_point, end_point;

    void Start()
    {

    }

    void Update()
    {

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Zoom
        if(camera_var.orthographic){
            camera_var.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scroll_speed;
        } else {
            if(camera_var.fieldOfView >= 1) { camera_var.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * scroll_speed; }
            else { camera_var.fieldOfView = 1f; }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        Vector3 camera_translation = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * camera_var.fieldOfView / 60;
        camera_var.transform.Translate(camera_translation);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Draw safe zone
        if(Input.GetMouseButtonDown(0)){
            print(Input.mousePosition);
            print(camera_var.ViewportToWorldPoint(Input.mousePosition));
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
