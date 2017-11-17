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
        DataManager.listStreamingAssetsBundleJson(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        DataManager.readStreamingAssetsBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        // ayam = new Story(Path.Combine(Application.dataPath, "Story/Story2.json"));
    }
    public void panggilCerita()
    {
        StoryManager.storyType = storyDataType.BundlePath;
        StoryManager.stringOrPath= "comic 2";
        StoryManager.stringOrPath2 = "story2.json";
        print(StoryManager.stringOrPath);
        
		StoryManager.nextScene = "Dynamic Player";
        SceneManager.LoadScene("Player");

    }
}
