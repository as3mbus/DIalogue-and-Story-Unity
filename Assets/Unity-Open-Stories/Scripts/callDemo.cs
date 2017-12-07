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
        DataManager.listStreamingAssetBundleJson(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        DataManager.readStreamingAssetBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        AssetBundle comic2 = DataManager.readAssetBundles(DataManager.bundlePath("comic 2"));
        ayam = Story.parseJson(JsonMapper.ToObject(comic2.LoadAsset<TextAsset>("Story2").text));
        comic2.Unload(true);
        ayam.loadResources();

        // print(DataManager.findItemInBundle(bundle, "story/prologue 1.json"));
    }
    public void panggilCerita()
    {

        // StoryManager.storyType = storyDataType.DataPath;
        // StoryManager.stringOrDataPath = Path.Combine(Application.dataPath, "Story/Story A/Story2.json");

        // JsonData jsondat = JsonMapper.ToObject(DataManager.readAssetsTextFile(Path.Combine(Application.dataPath, "Story/Story A/Story2.json")));
        // StoryManager.storyType = storyDataType.JsonData;
        // StoryManager.json = jsondat;

        // string storyString = DataManager.readAssetsTextFile(Path.Combine(Application.dataPath, "Story/Prologue 1.json"));
        // StoryManager.storyType = storyDataType.String;
        // StoryManager.stringOrDataPath = storyString;

        // StoryManager.storyType = storyDataType.TextAsset;
        // StoryManager.textAsset = textAsset;

        // StoryManager.storyType = storyDataType.Story;
        // StoryManager.stori = ayam;


        // bundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles/prologue 1"));
        // StoryManager.storyType = storyDataType.AssetBundle;
        // StoryManager.bundle= bundle;
        // StoryManager.stringOrDataPath = "story/prologue 1.json";
        // print(StoryManager.stringOrBundlePath);
        StoryManager.stori = ayam;
        StoryManager.nextScene = "Demo";
        SceneManager.LoadScene("Player");

    }
}
