using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using as3mbus.Story;

namespace as3mbus.Story
{
    public class Story
    {
        public string name;
        public List<Phase> phases = new List<Phase>();
        //new empty story 
        public AssetBundle assetBundle = null;
        public Story() { }
        //load/create story based on static class story manager 
        public Story(storyDataType storyType)
        {
            switch (storyType)
            {
                case storyDataType.AssetBundle:
                    loadJsonStory(StoryManager.bundle, StoryManager.stringOrDataPath);
                    break;
                case storyDataType.BundlePath:
                    loadJsonStory(StoryManager.stringOrBundlePath, StoryManager.stringOrDataPath);
                    break;
                case storyDataType.DataPath:
                    loadJsonStory(StoryManager.stringOrDataPath);
                    break;
                case storyDataType.JsonData:
                    parseJsonStory(StoryManager.json);
                    break;
                case storyDataType.String:
                    parseJsonStory(StoryManager.stringOrDataPath);
                    break;
                case storyDataType.TextAsset:
                    loadJsonStory(StoryManager.textAsset);
                    break;
                case storyDataType.Story:
                    loadExisting(StoryManager.stori);
                    break;
                case storyDataType.New:
                    phases = new List<Phase>();
                    break;
                default:
                    Debug.Log("Story type not found!");
                    break;
            }
        }
        //read story with filepath 
        public Story(string dataPath)
        {
            loadJsonStory(dataPath);
        }
        //read story data using text asset 
        public Story(TextAsset storyJson)
        {
            loadJsonStory(storyJson);
        }
        //convert story to json string 
        public string toJson()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.IndentValue = 4;

            toJson(writer);
            return sb.ToString();
        }
        //write json data using assigned json writer 
        public void toJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName("name");
            writer.Write(this.name);
            writer.WritePropertyName("phase");
            writer.WriteArrayStart();

            foreach (Phase phase in phases)
            {
                phase.toJson(writer);
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
        //read json story from bundle in designated path
        void loadJsonStory(string bundleName, string dataPath)
        {
            StoryManager.stringOrBundlePath = bundleName;
            AssetBundle storyBundle = DataManager.readAssetBundles(DataManager.bundlePath(bundleName));
            // Debug.Log(storyBundle.name);
            loadJsonStory(storyBundle, dataPath);
        }

        //read json story inside bundle
        void loadJsonStory(AssetBundle storyBundle, string dataPath)
        {
            assetBundle = storyBundle;
            StoryManager.stringOrBundlePath = storyBundle.name;
            // Debug.Log(storyBundle.name);
            StoryManager.bundle = storyBundle;
            loadJsonStory(storyBundle.LoadAsset<TextAsset>(DataManager.findItemInBundle(storyBundle, dataPath)));
            storyBundle.Unload(false);
        }
        //read json story from path
        void loadJsonStory(string dataPath)
        {
            this.name = Path.GetFileNameWithoutExtension(dataPath);
            JsonData jsonStory = JsonMapper.ToObject(File.ReadAllText(dataPath));
            parseJsonStory(jsonStory);
        }
        //read json story from textasset
        void loadJsonStory(TextAsset storyJson)
        {
            this.name = storyJson.name;
            parseJsonStory(storyJson.text);
        }
        //clone json story based on another story
        void cloneJsonStory(Story story)
        {
            parseJsonStory(story.toJson());
        }
        void loadExisting(Story story)
        {
            this.name = story.name;
            this.phases = story.phases;
        }
        //read json story based on json string
        void parseJsonStory(string storyJsonString)
        {
            JsonData jsonStory = JsonMapper.ToObject(storyJsonString);
            parseJsonStory(jsonStory);
        }
        //parse json story data
        void parseJsonStory(JsonData storyJson)
        {
            this.name = storyJson["name"].ToString();
            foreach (JsonData cerita in storyJson["phase"])
            {
                Phase fase;
                if (cerita.Keys.Contains("bgmAssetBundleName"))
                    fase = Phase.parseJson_1_2(cerita, assetBundle);
                else if (cerita.Keys.Contains("content"))
                    fase = Phase.parseJson_1_1(cerita, assetBundle);
                else
                    fase = Phase.parseJson_1_0(cerita, assetBundle);
                this.phases.Add(fase);
            }
        }

    }
    public class StoryJsonKey
    {
        public string[] keys = new string[] { "name", "phase" };
        public PhaseJsonKey phaseKey = new as3mbus.Story.PhaseJsonKey();
        public StoryJsonKey(string[] newKeys, PhaseJsonKey newPhaseKey)
        {
            for (int i = 0; i < newKeys.Length; i++)
                if (!String.IsNullOrEmpty(newKeys[i]))
                    keys[i] = newKeys[i];
            if (newPhaseKey != null)
                phaseKey = newPhaseKey;
        }
        public StoryJsonKey() { }
        public static StoryJsonKey V_1_0
        {
            get { return new StoryJsonKey(new string[] { "name", "comicsource", "comicname", "", "", "content" }, PhaseJsonKey.V_1_0); }
        }
    }

}