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
        public string name, version = "1.2";
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
                    loadJson(StoryManager.bundle, StoryManager.stringOrDataPath);
                    break;
                case storyDataType.BundlePath:
                    loadJson(StoryManager.stringOrBundlePath, StoryManager.stringOrDataPath);
                    break;
                case storyDataType.DataPath:
                    loadJson(StoryManager.stringOrDataPath);
                    break;
                case storyDataType.JsonData:
                    parseJson(StoryManager.json);
                    break;
                case storyDataType.String:
                    parseJson(StoryManager.stringOrDataPath);
                    break;
                case storyDataType.TextAsset:
                    loadJson(StoryManager.textAsset);
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
            loadJson(dataPath);
        }
        //read story data using text asset 
        public Story(TextAsset storyJson)
        {
            loadJson(storyJson);
        }
        //convert story to json string 
        public string toJson()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.IndentValue = 4;

            writeJson(writer);
            return sb.ToString();
        }
        // write json data using assigned json writer and latest story json key
        public void writeJson(JsonWriter writer)
        {
            writeJson(writer, StoryJsonKey.Latest.Story);
        }

        // write json data using assigned json writer and specified story json key
        public void writeJson(JsonWriter writer, JsonKey storyKey)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName(storyKey.elementsKeys[0]);
            writer.Write(this.version);
            writer.WritePropertyName(storyKey.elementsKeys[1]);
            writer.Write(this.name);
            writer.WritePropertyName(storyKey.elementsjsonKey[0].objectName);
            writer.WriteArrayStart();
            foreach (Phase phase in phases)
            {
                phase.writeJson(writer, storyKey.elementsjsonKey[0]);
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
        //read json story from bundle in designated path
        void loadJson(string bundleName, string dataPath)
        {
            StoryManager.stringOrBundlePath = bundleName;
            AssetBundle storyBundle = DataManager.readAssetBundles(DataManager.bundlePath(bundleName));
            // Debug.Log(storyBundle.name);
            loadJson(storyBundle, dataPath);
        }

        //read json story inside bundle
        void loadJson(AssetBundle storyBundle, string dataPath)
        {
            assetBundle = storyBundle;
            StoryManager.stringOrBundlePath = storyBundle.name;
            // Debug.Log(storyBundle.name);
            StoryManager.bundle = storyBundle;
            loadJson(storyBundle.LoadAsset<TextAsset>(DataManager.findItemInBundle(storyBundle, dataPath)));
            storyBundle.Unload(false);
        }
        //read json story from path
        void loadJson(string dataPath)
        {
            this.name = Path.GetFileNameWithoutExtension(dataPath);
            JsonData jsonStory = JsonMapper.ToObject(File.ReadAllText(dataPath));
            parseJson(jsonStory);
        }
        //read json story from textasset
        void loadJson(TextAsset storyJson)
        {
            this.name = storyJson.name;
            parseJson(storyJson.text);
        }
        //clone json story based on another story
        void cloneJsonStory(Story story)
        {
            parseJson(story.toJson());
        }
        // referrence story to a loaded story
        void loadExisting(Story story)
        {
            this.name = story.name;
            this.phases = story.phases;
        }
        // read json story based on json string
        void parseJson(string storyJsonString)
        {
            JsonData jsonStory = JsonMapper.ToObject(storyJsonString);
            parseJson(jsonStory);
        }
        // parse json story data using version related story json key
        void parseJson(JsonData storyJsonData)
        {
            if (storyJsonData.Keys.Contains("version"))
                parseJson(storyJsonData, StoryJsonKey.Latest.Story);
            else
                parseJson(storyJsonData, StoryJsonKey.V_1_1.Story);

        }

        //  parse json story data using specified story json key
        void parseJson(JsonData storyJsonData, JsonKey storyKey)
        {
            this.name = storyJsonData[storyKey.elementsKeys[1]].ToString();
            foreach (JsonData phaseJsonData in storyJsonData[storyKey.elementsjsonKey[0].objectName])
            {
                this.phases.Add(Phase.parseJson(phaseJsonData, this.assetBundle, storyKey.elementsjsonKey[0]));
            }
        }

    }

}