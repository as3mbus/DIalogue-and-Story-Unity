using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

//class to control story loading for scene transition etc 
namespace as3mbus.Story
{
	public enum storyDataType
        {
            String, JsonData, TextAsset, BundlePath, DataPath, Story, New
        }
    public static class StoryManager
    {
        
        public static storyDataType storyType = storyDataType.New;
        public static string source;
        public static JsonData json;
        public static string stringOrPath;
        public static TextAsset textAsset;
        public static StoryEx stori;

        public static string nextScene;
    }
}

