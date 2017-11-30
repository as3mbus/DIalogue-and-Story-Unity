using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using as3mbus.Story;
using UnityEngine.SceneManagement;
using LitJson;

//Demo story calling  
public class callDemo : MonoBehaviour
{
    AssetBundle bundle;
    public TextAsset textAsset;
    Story ayam;
    void Start()
    {
        DataManager.listStreamingAssetsBundleJson(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        DataManager.readStreamingAssetsBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        ayam = new Story(Path.Combine(Application.dataPath, "Story/Prologue 1.json"));

        // print(DataManager.findItemInBundle(bundle, "story/prologue 1.json"));
    }
    public void panggilCerita()
    {
        // StoryManager.storyType = storyDataType.BundlePath;
        // StoryManager.stringOrBundlePath = "prologue 1";
        // StoryManager.stringOrDataPath = "story/prologue 1.json";

        // StoryManager.storyType = storyDataType.DataPath;
        // StoryManager.stringOrDataPath = Path.Combine(Application.dataPath, "Story/Prologue 1.json");

        // JsonData jsondat = JsonMapper.ToObject(DataManager.readAssetsTextFile(Path.Combine(Application.dataPath, "Story/Prologue 1.json")));
        // StoryManager.storyType = storyDataType.JsonData;
        // StoryManager.json = jsondat;

        // string storyString = DataManager.readAssetsTextFile(Path.Combine(Application.dataPath, "Story/Prologue 1.json"));
        // StoryManager.storyType = storyDataType.String;
        // StoryManager.stringOrDataPath = storyString;

        // StoryManager.storyType = storyDataType.TextAsset;
        // StoryManager.textAsset = textAsset;

        StoryManager.storyType = storyDataType.Story;
        StoryManager.stori = ayam;


        // bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles/prologue 1"));
        // StoryManager.storyType = storyDataType.AssetBundle;
        // StoryManager.bundle= bundle;
        // StoryManager.stringOrDataPath = "story/prologue 1.json";
        print(StoryManager.stringOrBundlePath);

        StoryManager.nextScene = "Demo";
        SceneManager.LoadScene("Player");

    }
}
