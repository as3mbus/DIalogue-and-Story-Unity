using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using LitJson;

public class DialogueController : MonoBehaviour
{
    public Transform kamera, paths;
    public GameObject[] Chara;
    public Text dName, dText;
    public int currentLine = 0, currentChar = 0;
    public float speed = 1f, routeRadius = 1f, rotationSpeed = 5f, typeDelay = 0.2f;
    float timeCount;
    Dialogue activeDialogue;
    private StorySceneController ssControl;

    public void startDialogue(Dialogue dialog)
    {
        this.activeDialogue=dialog;
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
            if (currentChar >= activeDialogue.message[currentLine].Length)
            {
                if (currentLine < activeDialogue.message.Count - 1)
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
                showDialogue(activeDialogue.message[currentLine]);
            }
        }
        textPerSec(typeDelay);
    }
    public void showDialogue(string dialogue)
    {
        dText.text = dialogue;
        currentChar = dialogue.Length;
    }
    public void readDialogue(int line)
    {
        currentChar = 0;
        dName.text = activeDialogue.character[line];
        dText.text = "";
    }
    public void textPerSec(float delay)
    {
        if (currentChar >= activeDialogue.message[currentLine].Length)
            return;
        timeCount += Time.deltaTime;
        if (timeCount > delay)
        {
            dText.text = dText.text + activeDialogue.message[currentLine][currentChar];
            currentChar++;
            timeCount = 0;
        }
    }

    // public void camRoute(){
    //     float distance = Vector3.Distance(pathRoute.pathObjects[currentWaypoint].position, transform.position);
    //     transform.position = Vector3.MoveTowards(transform.position, pathRoute.pathObjects[currentWaypoint].position, Time.deltaTime * speed);
    //     if (Input.GetButtonDown("Fire1") && distance <= routeRadius && currentWaypoint < pathRoute.pathObjects.Count)
    //     {
    //         currentWaypoint++;
    //     }
    // }

    public void hideDialogue()
    {
        gameObject.SetActive(false);
        ssControl.nextPhase();
    }

}
