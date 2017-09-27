using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {
	public MouseCamControlPan cams;
	void OnMouseDrag ()
	{
		cams.enabled=false;
		Vector3 mousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y,-1);
		Vector3 ObjPos = Camera.main.ScreenToWorldPoint(mousePos);
		transform.position = ObjPos;
	}
	void OnMouseUp(){
		cams.enabled=true;
	}
	void OnMouseEnter(){
		
	}
	// Use this for initialization
	
}
