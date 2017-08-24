using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using LitJson;

public class DialogueController : MonoBehaviour
{
    public GameObject[] Chara;
    public GameObject dPanel;
    public Text dName, dText;
    public string[] dialogueLines;
    public int currentLine = 0;

    // Use this for initialization
    void Awake()
    {

    }
    void Start()
    {
        currentLine=0;
        showDialogue(dialogueLines[currentLine]);
    }

    // Update is called once per frame
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
        dPanel.SetActive(true);
        dText.text = dialogue;
        Cursor.lockState = CursorLockMode.None;
    }
    public void hideDialogue()
    {
        dPanel.SetActive(false);
    }

}
