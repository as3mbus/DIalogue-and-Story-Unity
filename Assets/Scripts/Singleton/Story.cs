using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        JsonData jsonStory;
        TextAsset teksJson;
        teksJson = Resources.Load("Data/" + filename) as TextAsset;
        jsonStory = JsonMapper.ToObject(teksJson.ToString());
        foreach (JsonData cerita in jsonStory["phase"])
        {
            Phase fase = new Phase(cerita);
            this.phases.Add(fase);
        }
    }

}