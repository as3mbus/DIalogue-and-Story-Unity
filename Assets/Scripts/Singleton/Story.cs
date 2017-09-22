using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using LitJson;

public class Story
{
    public string name;
    public ArrayList phases = new ArrayList();
    public Story()
    {

    }
    public string toJson()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = true;
        writer.IndentValue = 4;

        toJson(writer);
        return sb.ToString();
    }
    public void toJson(JsonWriter writer)
    {
        writer.WriteObjectStart();
        writer.WritePropertyName("name");
        writer.Write(this.name);
        writer.WritePropertyName("phase");
        writer.WriteArrayStart();

        foreach (var phase in phases)
        {
            Phase fase = (Phase)phase;
            fase.toJson(writer);
        }
        writer.WriteArrayEnd();
        writer.WriteObjectEnd();
    }

    public Story(string filename)
    {
        this.name = Path.GetFileNameWithoutExtension(filename);
        TextAsset teksJson = Resources.Load("Data/" + filename) as TextAsset;
        JsonData jsonStory = JsonMapper.ToObject(teksJson.ToString());
        loadJsonStory(jsonStory);
    }
    void loadJsonStory(JsonData storyJson)
    {
        this.name = storyJson["name"].ToString();
        foreach (JsonData cerita in storyJson["phase"])
        {
            Phase fase = new Phase(cerita);
            this.phases.Add(fase);
        }
    }
}