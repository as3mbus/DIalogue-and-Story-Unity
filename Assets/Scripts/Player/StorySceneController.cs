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
        // try
        // {
            cerita = new Story(StoryManager.storyType);
        // }
        // catch (System.Exception)
        // {
        //     cerita = new Story(storyJson);
        // }
        currentPhase = 0;
        nextPhase();
    }
    void loadPhase(int number)
    {

        phaseCanvas.SetActive(true);
        Phase fase = (Phase)cerita.phases[number];
        phaseCanvas.GetComponent<PhaseController>().startPhase(fase);

    }

    public void nextPhase()
    {
        if(currentPhase >= cerita.phases.Count) return;
        currentPhase++;
        print(currentPhase);
        print(cerita.phases.Count);
        if (currentPhase < cerita.phases.Count)
        {
            loadPhase(currentPhase);
        }
    }
}
