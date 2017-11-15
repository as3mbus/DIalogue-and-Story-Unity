using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkPersistentDataPath : MonoBehaviour {
	public InputField messageField;

	// Use this for initialization
	void Start () {
		print("datapath : "+Application.persistentDataPath);
		messageField.text = "datapath : "+Application.persistentDataPath;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
