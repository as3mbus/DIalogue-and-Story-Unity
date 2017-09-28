using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    bool dragged = false;
    public MouseCamControlPan cams;
    float size=1;
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
        size -=Input.GetAxis("Mouse ScrollWheel");
        size = size<0? 0 : size;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            transform.localScale = new Vector3(size,size);
    }
    void OnEnable(){
        size = 1 * cams.zoom/5;
        transform.localScale = new Vector3(size,size);
    }
    void OnMouseExit(){
        if (!dragged)
            cams.enabled = true;
	}

    // Use this for initialization

}
