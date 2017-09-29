using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace as3mbus.Story
{
	public enum storyDataType
        {
            String, JsonData, TextAsset, BundlePath, DataPath, Story
        }
    public static class StoryManager
    {
        
        public static storyDataType storyType;
        public static string source;
        public static JsonData json;
        public static string stringOrPath;
        public static TextAsset textAsset;
        public static Story stori;
    }
}

