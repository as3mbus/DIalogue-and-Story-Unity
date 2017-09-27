using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    bool dragged = false;
    public MouseCamControlPan cams;
    void OnMouseDrag()
    {
        dragged = true;
        cams.enabled = false;
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9);
        Vector3 ObjPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = ObjPos;
    }
    void OnMouseUp()
    {
		dragged=false;
        cams.enabled = true;
    }
    void OnMouseOver()
    {
        cams.enabled = false;
        float zoom = -Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            transform.localScale = transform.localScale + new Vector3(zoom, zoom);
    }
    void OnMouseExit(){
        if (!dragged)
            cams.enabled = true;
	}

    // Use this for initialization

}
