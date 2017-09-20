using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySceneController : MonoBehaviour
{
    Story cerita;
    public GameObject phaseCanvas;
    public int currentPhase;
    public string filename;
    // Use this for initialization
    void Start()
    {
        currentPhase = 0;
        cerita = new Story(filename);
        loadPhase(currentPhase);
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
