using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for mouse input wheels
        if (Input.GetAxis("Mouse ScrollWheel") > 0f ) {

        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) {

        }

        if(Input.GetMouseButton(1)){
            print("RIGHT");
        }
    }
}
