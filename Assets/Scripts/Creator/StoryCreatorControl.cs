using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryCreatorControl : MonoBehaviour {
	Story targetStory;
	public GameObject typeCanvas;
	// Use this for initialization
	void Start () {
		targetStory=new Story();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void newPhase(){
		typeCanvas.SetActive(true);
	}
	public void newDialogue(){
		targetStory.phase.Add(new Dialogue());
		typeCanvas.SetActive(false);
	}
	public void newComic(){
		typeCanvas.SetActive(false);
	}
	public void cancelPhase(){
		typeCanvas.SetActive(false);
	}

}
