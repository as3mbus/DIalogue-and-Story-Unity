using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using as3mbus.Story;

public class StorySceneController : MonoBehaviour
{
    Story cerita;
    public GameObject phaseCanvas;
    public int currentPhase;
    public TextAsset storyJson;
    // Use this for initialization
    void Start()
    {
        currentPhase = 0;
        cerita = new Story(storyJson);
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
