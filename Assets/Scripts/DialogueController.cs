﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using LitJson;

public class DialogueController : MonoBehaviour
{
    public GameObject[] Chara;
    public Text dName, dText;
    public string[] dialogueLines,characterNames;
    public int currentLine = 0;
    private StorySceneController ssControl;

    public void startDialogue(Dialogue dialog){
        dialogueLines=dialog.message.ToArray();
        currentLine=0;
        showDialogue(dialogueLines[currentLine]);
    }
    // Update is called once per frame
    void Start(){
        ssControl=FindObjectOfType<StorySceneController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentLine < dialogueLines.Length - 1)
            {
                currentLine++;
                showDialogue(dialogueLines[currentLine]);
            }
            else
            {
                hideDialogue();
            }

        }
    }
    public void showDialogue(string dialogue)
    {
        dText.text = dialogue;
    }
    public void hideDialogue()
    {
        gameObject.SetActive(false);
        ssControl.nextPhase();
    }

}