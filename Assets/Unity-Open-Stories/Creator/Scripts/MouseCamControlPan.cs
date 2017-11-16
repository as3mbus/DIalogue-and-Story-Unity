using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamControlPan : MonoBehaviour
{
    public float camSensitivity = 1f, zoomSensitivity = 1f ;

    public float mouseX, mouseY,zoom = 5;
    //read mouse movement and limit it's movement 
    void mouseRead()
    {
        //get mouse movement
        mouseX -= Input.GetAxis("Mouse X") * camSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * camSensitivity;
        //limit lookup and lookdown
        mouseY = Mathf.Clamp(mouseY, -60f, 60f);
    }
    //moving cam based on mouse drag
    void camPanning()
    {
        transform.position = new Vector3(mouseX, mouseY, 0) * Time.deltaTime + transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        //read scroll wheel to set orthoraphic size (2D Camera)
		zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        //limit zoom value to not below 0
        zoom = zoom<0.1f? 0.1f : zoom;

        if (Input.GetButton("Fire1"))
        {
            mouseRead();
            camPanning();
            mouseX = 0;
            mouseY = 0;
        }
        if (Input.GetAxis("Mouse ScrollWheel")!=0)
		    this.GetComponent<Camera>().orthographicSize=zoom;
    }
}
