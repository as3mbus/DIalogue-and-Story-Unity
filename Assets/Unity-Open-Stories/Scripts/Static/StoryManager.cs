using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

//class to control story loading for scene transition etc 
namespace as3mbus.Story
{
    public enum storyDataType
    {
        String, JsonData, TextAsset, AssetBundle, BundlePath, DataPath, Story, New
    }
    public static class StoryManager
    {
        public static Story stori = new Story();
        public static bool skipable = false;

        public static string nextScene;
    }
}

