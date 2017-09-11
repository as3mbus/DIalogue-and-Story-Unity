using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using LitJson;

public class DialogueController : MonoBehaviour
{
    public Transform kamera;
    public Text dName, dText;
    public int currentLine = 0, currentChar = 0;
    public float speed = 5f, routeRadius = 1f, typeDelay = 0.2f;
    float timeCount;
    Dialogue activeDialogue;
    private StorySceneController ssControl;

    public void startDialogue(Dialogue dialog)
    {
        // JsonDialogue tes = new JsonDialogue(dialog);
        // tes.writeJson();

        this.activeDialogue = dialog;
        Debug.Log(dialog.toJson());
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
            if (currentChar >= activeDialogue.messages[currentLine].Length)
            {
                if (currentLine < activeDialogue.messages.Count - 1)
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
                showDialogue(activeDialogue.messages[currentLine]);
            }
        }
        textPerSec(typeDelay);
        camRoute();

    }
    public void showDialogue(string dialogue)
    {
        dText.text = dialogue;
        currentChar = dialogue.Length;
    }
    public void readDialogue(int line)
    {
        currentChar = 0;
        dName.text = activeDialogue.characters[line];
        dText.text = "";
    }
    public void textPerSec(float delay)
    {
        if (currentChar >= activeDialogue.messages[currentLine].Length)
            return;
        timeCount += Time.deltaTime;
        if (timeCount > delay)
        {
            dText.text = dText.text + activeDialogue.messages[currentLine][currentChar];
            currentChar++;
            timeCount = 0;
        }
    }

    public void camRoute()
    {
        float distance = Vector3.Distance(activeDialogue.paths[currentLine], kamera.position);
        float zoomDistance = Mathf.Abs(kamera.GetComponent<Camera>().orthographicSize - activeDialogue.zooms[currentLine]);
        if (distance != 0)
            kamera.position = Vector3.MoveTowards(kamera.position, activeDialogue.paths[currentLine], Time.deltaTime * speed);
        if (zoomDistance!=0)
            kamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(kamera.GetComponent<Camera>().orthographicSize, activeDialogue.zooms[currentLine], Time.deltaTime * speed);
        // if (Input.GetButtonDown("Fire1") && currentLine < activeDialogue.paths.Count)
        // {
        //     currentLine++;
        // }
    }

    public void hideDialogue()
    {
        gameObject.SetActive(false);
        ssControl.nextPhase();
    }

}
