using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using as3mbus.Story;
using UnityEngine.SceneManagement;


public class callDemo : MonoBehaviour
{
    Story ayam;
    void Start()
    {
        ComicManager.readComicsBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        

    }
    public void panggilCerita()
    {
        ayam = new Story(Path.Combine(Application.dataPath, "Data/Story2.json"));
        StoryManager.storyType = storyDataType.Story;
        StoryManager.stori = ayam;
        
		StoryManager.nextScene = "Player";
        SceneManager.LoadScene("Player");

    }
}
