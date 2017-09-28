using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragUI : MonoBehaviour
{
    bool dragged = false;
    public MouseCamControlPan cams;
    void OnMouseDrag()
    {
        cams.enabled = false;
    }
    void OnMouseUp()
    {
        cams.enabled = true;
    }

    void OnMouseExit(){
        cams.enabled = dragged ? false:true;
    }


    // Use this for initialization

}
