using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using as3mbus.Story;
using UnityEngine.SceneManagement;

//Demo story calling  
public class callDemo : MonoBehaviour
{
    Story ayam;
    void Start()
    {
        ComicManager.readComicsBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        ayam = new Story(Path.Combine(Application.dataPath, "Data/Story2.json"));
    }
    public void panggilCerita()
    {
        StoryManager.storyType = storyDataType.Story;
        StoryManager.stori = ayam;
        
		StoryManager.nextScene = "dynamic player";
        SceneManager.LoadScene("dynamic player");

    }
}
