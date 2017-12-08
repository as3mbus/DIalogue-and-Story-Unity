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
    public GameObject skipButton;
    // Use this for initialization
    void Start()
    {
        DataManager.readStreamingAssetBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));

        // try
        // {
        //load story based on static class story manager 
        cerita = StoryManager.stori;
        nextScene = StoryManager.nextScene;
        skipButton.SetActive(StoryManager.skipable);
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
        fase.loadResources();
        phaseCanvas.GetComponent<PhaseController>().startPhase(fase);

    }

    //call next phase or finish story if there are none
    public void nextPhase()
    {
        currentPhase++;
        if (currentPhase < cerita.phases.Count)
            loadPhase(currentPhase);

        else endStory();

    }

    //end story scene
    public void endStory()
    {
        if (nextScene != null)
        {
            StoryManager.skipable = false;
            SceneManager.LoadScene(nextScene);
        }
    }
}
