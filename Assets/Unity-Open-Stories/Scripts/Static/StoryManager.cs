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

        public static storyDataType storyType = storyDataType.New;
        public static string source;
        public static JsonData json;
        public static string stringOrPath;
        public static string stringOrPath2;
        public static AssetBundle bundle;
        public static TextAsset textAsset;
        public static Story stori;
        public static bool skipable = false;

        public static string nextScene;
    }
}

