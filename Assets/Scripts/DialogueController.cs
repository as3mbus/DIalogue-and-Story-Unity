using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using LitJson;

public class DialogueController : MonoBehaviour
{
    public GameObject[] Chara;
    public Text dName, dText;
    public string[] dialogueLines, characterNames;
    public int currentLine = 0;
    int currentChar = 0;
    float timeCount;
    float typeDelay = 0.2f;
    private StorySceneController ssControl;


    public void startDialogue(Dialogue dialog)
    {
        dialogueLines = dialog.message.ToArray();
        characterNames = dialog.character.ToArray();
        currentLine = 0;
        readDialogue(currentLine);
    }
    // Update is called once per frame
    void Start()
    {
        ssControl = FindObjectOfType<StorySceneController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentChar >= dialogueLines[currentLine].Length)
            {
                if (currentLine < dialogueLines.Length - 1)
                {
                    currentLine++;
                    readDialogue(currentLine);
                }
                else
                {
                    hideDialogue();
                }
            }
            else
            {
                showDialogue(dialogueLines[currentLine]);
            }
        }
        textPerSec(currentLine, typeDelay);
    }
    public void showDialogue(string dialogue)
    {
        dText.text = dialogue;
        currentChar = dialogue.Length;
    }
    public void readDialogue(int line)
    {
        currentChar = 0;
        dName.text = characterNames[line];
        dText.text = "";
    }
    public void textPerSec(int line, float delay)
    {
        if (currentChar >= dialogueLines[line].Length)
            return;
        timeCount += Time.deltaTime;
        if (timeCount > delay)
        {

            dText.text = dText.text + dialogueLines[line][currentChar];
            currentChar++;
            timeCount = 0;
        }
    }

    public void hideDialogue()
    {
        gameObject.SetActive(false);
        ssControl.nextPhase();
    }

}
