using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySceneController : MonoBehaviour
{
    Story cerita;
    public GameObject dialogueCanvas, comicCanvas, phaseCanvas;
    public int currentPhase;
    public string filename;
    private DialogueController dControl;
    private ComicController cControl;
    // Use this for initialization
    void Start()
    {
        currentPhase = 0;
        cerita = new Story(filename);
        loadPhase(currentPhase);
    }
    void loadPhaseOld(int number)
    {
        if (cerita.phases[number].GetType().ToString() == "Dialogue")
        {
            dialogueCanvas.SetActive(true);
            Dialogue dialog = (Dialogue)cerita.phases[number];
            dialogueCanvas.GetComponent<DialogueController>().startDialogue(dialog);
        }
        else if (cerita.phases[number].GetType().ToString() == "Comic")
        {
            comicCanvas.SetActive(true);
            Comic komik = (Comic)cerita.phases[number];
            comicCanvas.GetComponent<ComicController>().startComic(komik);
        }
    }
    void loadPhase(int number)
    {

        phaseCanvas.SetActive(true);
        Phase fase = (Phase)cerita.phases[number];
        phaseCanvas.GetComponent<PhaseController>().startPhase(fase);

    }

    public void nextPhase()
    {
        currentPhase++;
        if (currentPhase < cerita.phases.Count)
        {
            loadPhase(currentPhase);
        }
    }
}
