using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using LitJson;
using as3mbus.Story;

namespace as3mbus.Story
{
    public class Story
    {
        public string name;
        public ArrayList phases = new ArrayList();
        //new empty story 
        public Story()
        {

        }
        //load/create story based on static class story manager 
        public Story(storyDataType storyType)
        {
            switch (storyType)
            {
                case storyDataType.BundlePath:
                    break;
                case storyDataType.DataPath:
                    break;
                case storyDataType.JsonData:
                    loadJsonStory(StoryManager.json);
                    break;
                case storyDataType.String:
                    loadJsonStory(StoryManager.stringOrPath);
                    break;
                case storyDataType.TextAsset:
                    loadJsonStory(StoryManager.textAsset);
                    break;
                case storyDataType.Story:
                    loadJsonStory(StoryManager.stori);
                    break;
                case storyDataType.New:
                    phases = new ArrayList();
                    break;
                default:
                    Debug.Log("Story not Found!");
                    break;
            }
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
        //read story with filepath 
        public Story(string filePath)
        {
            this.name = Path.GetFileNameWithoutExtension(filePath);
            JsonData jsonStory = JsonMapper.ToObject(File.ReadAllText(filePath));
            loadJsonStory(jsonStory);
        }
        //read story data using text asset 
        public Story(TextAsset storyJson)
        {
            loadJsonStory(storyJson);
        }
        //read json story from textasset
        void loadJsonStory(TextAsset storyJson)
        {
            this.name = storyJson.name;
            loadJsonStory(storyJson.text);
        }

        //read json story based on json string
        void loadJsonStory(string storyJsonString)
        {
            JsonData jsonStory = JsonMapper.ToObject(storyJsonString);
            loadJsonStory(jsonStory);
        }
        //read json story based on another story
        void loadJsonStory(Story story)
        {
            loadJsonStory(story.toJson());
        }
        //parse json story data
        void loadJsonStory(JsonData storyJson)
        {
            this.name = storyJson["name"].ToString();
            foreach (JsonData cerita in storyJson["phase"])
            {
                Phase fase = new Phase(); ;
                fase = fase.parseJson(cerita);
                this.phases.Add(fase);
            }
        }

    }

}