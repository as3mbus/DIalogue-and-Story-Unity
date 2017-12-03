﻿using System.Collections;
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
        ayam = new Story(Path.Combine(Application.dataPath, "Story/Story A/Story2.json"));

        // print(DataManager.findItemInBundle(bundle, "story/prologue 1.json"));
    }
    public void panggilCerita()
    {
        StoryManager.storyType = storyDataType.BundlePath;
<<<<<<< 12298132c883da0c8de38a0f694cd0a1e3b3aceb

        StoryManager.stringOrBundlePath = "prologue 1";
        StoryManager.stringOrDataPath = "story/prologue 1.json";
=======
        StoryManager.stringOrBundlePath = "comic 2";
        StoryManager.stringOrDataPath = "story/story a/story2.json";
>>>>>>> change call param for demo bundle

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
        print(StoryManager.stringOrBundlePath);

        StoryManager.nextScene = "Demo";
        SceneManager.LoadScene("Player");

    }
}
