using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySceneController : MonoBehaviour
{
    Story cerita;
    public GameObject dialogueCanvas, comicCanvas;
    public int currentPhase;
    public string filename;
    private DialogueController dControl;
    private ComicController cControl;
    // Use this for initialization
    void Start()
    {
        currentPhase = 0;
        cerita = new Story(filename);
        loadPhase(0);
    }
    void loadPhase(int number)
    {
        if (cerita.phase[number].GetType().ToString() == "Dialogue")
        {
            dialogueCanvas.SetActive(true);
            Dialogue dialog = (Dialogue)cerita.phase[number];
            dialogueCanvas.GetComponent<DialogueController>().startDialogue(dialog);
        }
        else if (cerita.phase[number].GetType().ToString() == "Comic")
        {
            comicCanvas.SetActive(true);
            Comic komik = (Comic)cerita.phase[number];
            comicCanvas.GetComponent<ComicController>().startComic(komik);
        }
    }
    
    public void nextPhase()
    {
        currentPhase++;
        if (currentPhase < cerita.phase.Count)
        {
            loadPhase(currentPhase);
        }
    }
}
