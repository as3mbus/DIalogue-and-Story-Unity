using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseController : MonoBehaviour {
   public Transform kamera;
    public Text dName, dText;
    public int currentLine = 0, currentChar = 0;
    public Sprite backgroundSprite;
    public float speed = 5f, routeRadius = 1f, typeDelay = 0.2f;
    float timeCount;
    Phase activePhase;
    private StorySceneController ssControl;
    

    public void startPhase(Phase fase)
    {
        this.activePhase = fase;
        Debug.Log(fase.toJson());
        currentLine = 0;
        readLine(currentLine);
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
            if (currentChar >= activePhase.messages[currentLine].Length)
            {
                if (currentLine < activePhase.messages.Count - 1)
                {
                    currentLine++;
                    readLine(currentLine);
                }
                else
                {
                    hidePhase();
                }
            }
            else
            {
                showLine(activePhase.messages[currentLine]);
            }
        }
        textPerSec(typeDelay);
        camRoute();
    }
    public void showLine(string line)
    {
        dText.text = line;
        currentChar = line.Length;
    }
    public void readLine(int line)
    {
        currentChar = 0;
        dName.text = activePhase.characters[line];
        dText.text = "";
    }
    public void textPerSec(float delay)
    {
        if (currentChar >= activePhase.messages[currentLine].Length)
            return;
        timeCount += Time.deltaTime;
        if (timeCount > delay)
        {
            dText.text = dText.text + activePhase.messages[currentLine][currentChar];
            currentChar++;
            timeCount = 0;
        }
    }

    public void camRoute()
    {
        float distance = Vector3.Distance(activePhase.paths[currentLine], kamera.position);
        float zoomDistance = Mathf.Abs(kamera.GetComponent<Camera>().orthographicSize - activePhase.zooms[currentLine]);
        if (distance != 0)
            kamera.position = Vector3.MoveTowards(kamera.position, activePhase.paths[currentLine], Time.deltaTime * speed);
        if (zoomDistance!=0)
            kamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(kamera.GetComponent<Camera>().orthographicSize, activePhase.zooms[currentLine], Time.deltaTime * speed);
        // if (Input.GetButtonDown("Fire1") && currentLine < activePhase.paths.Count)
        // {
        //     currentLine++;
        // }
    }

    public void hidePhase()
    {
        gameObject.SetActive(false);
        ssControl.nextPhase();
    }
}
