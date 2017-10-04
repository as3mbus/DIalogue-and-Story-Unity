using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using as3mbus.Story;

public class StoryController : MonoBehaviour
{
    Story cerita;
    public GameObject phaseCanvas;
    public int currentPhase;
    public TextAsset storyJson;
    public string nextScene;
    // Use this for initialization
    void Start()
    {
        // try
        // {
        cerita = new Story(StoryManager.storyType);
        nextScene = StoryManager.nextScene;
        // }
        // catch (System.Exception)
        // {
        //     cerita = new Story(storyJson);
        // }
        currentPhase = -1;
        nextPhase();
    }
    void loadPhase(int number)
    {
        if (number >= cerita.phases.Count)
            return;
        phaseCanvas.SetActive(true);
        Phase fase = (Phase)cerita.phases[number];
        phaseCanvas.GetComponent<PhaseController>().startPhase(fase);

    }

    public void nextPhase()
    {
        currentPhase++;
        if (currentPhase < cerita.phases.Count)

            loadPhase(currentPhase);

        else SceneManager.LoadScene(nextScene);

    }
}
