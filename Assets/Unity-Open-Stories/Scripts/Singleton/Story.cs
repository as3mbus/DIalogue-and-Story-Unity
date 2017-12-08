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
        public Story() { }

        // parse json story data using version related story json key
        public static Story parseJson(JsonData storyJsonData)
        {

            if (storyJsonData.Keys.Contains("version"))
                return parseJson(storyJsonData, StoryJsonKey.Latest.Story);
            else
                return parseJson(storyJsonData, StoryJsonKey.V_1_1.Story);

        }

        //  parse json story data using specified story json key
        public static Story parseJson(JsonData storyJsonData, JsonKey storyKey)
        {
            Story parseResult = new Story();
            parseResult.name = storyJsonData[storyKey.elementsKeys[1]].ToString();
            foreach (JsonData phaseJsonData in storyJsonData[storyKey.elementsjsonKey[0].objectName])
                parseResult.phases.Add(Phase.parseJson(phaseJsonData, storyKey.elementsjsonKey[0]));
            return parseResult;
        }

        // load all needed resources
        public void loadResources()
        {
            foreach (Phase fase in phases)
                fase.loadResources();
        }

        //convert story to json string 
        public string toJson()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.IndentValue = 4;

            writeJson(writer, StoryJsonKey.Latest.Story);
            return sb.ToString();
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
    }
}