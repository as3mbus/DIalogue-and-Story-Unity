using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//drag class for dragging object in world space using mouse 
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
    void OnMouseExit(){
        cams.enabled = dragged ? false:true;
    }

    // Use this for initialization

}
