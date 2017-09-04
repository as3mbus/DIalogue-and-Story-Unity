using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamControlPan : MonoBehaviour
{
    public float camSensitivity = 1f, zoomSensitivity = 1f ;

    private float mouseX, mouseY,zoom = 5;

    // Use this for initialization
    void Start()
    {

    }
    void mouseRead()
    {

        //get mouse movement
        mouseX -= Input.GetAxis("Mouse X") * camSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * camSensitivity;


        //limit lookup and lookdown
        mouseY = Mathf.Clamp(mouseY, -60f, 60f);

    }
    void camPanning()
    {
        //rotating center point
        transform.position = new Vector3(mouseX, mouseY, 0) * Time.deltaTime + transform.position;
        //rotating camera to always look at center
    }
    // Update is called once per frame
    void Update()
    {
		zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        if (Input.GetButton("Fire1"))
        {
            mouseRead();

            camPanning();
            mouseX = 0;
            mouseY = 0;
        }
		this.GetComponent<Camera>().orthographicSize=zoom;
    }
}
