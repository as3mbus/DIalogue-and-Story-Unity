using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using as3mbus.Story;
using System.IO;

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
        ComicManager.readComicsBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));

        // try
        // {
        //load story based on static class story manager 
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
    //load phase at index and play it with phase controller 
    void loadPhase(int number)
    {
        if (number >= cerita.phases.Count)
            return;
        phaseCanvas.SetActive(true);
        Phase fase = cerita.phases[number];
        phaseCanvas.GetComponent<PhaseController>().startPhase(fase);

    }

    //call next phase 
    public void nextPhase()
    {
        currentPhase++;
        if (currentPhase < cerita.phases.Count)

            loadPhase(currentPhase);

        else SceneManager.LoadScene(nextScene);

    }
}
