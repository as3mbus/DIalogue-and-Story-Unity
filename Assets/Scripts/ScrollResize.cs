using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollResize : MonoBehaviour
{
    public MouseCamControlPan cams;
    float size=1;
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

    // Use this for initialization

}
