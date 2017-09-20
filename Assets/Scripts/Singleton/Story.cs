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
    public void toJson(JsonWriter writer){
        writer.WriteObjectStart();
        writer.WritePropertyName("name");
        writer.Write(this.name);
        writer.WritePropertyName("phase");
        writer.WriteArrayStart();

        foreach (var phase in phases)
        {
            if (phase.GetType() == typeof(Dialogue))
            {
                Dialogue dialog = (Dialogue)phase;
                dialog.toJson(writer);
            }
            else if (phase.GetType() == typeof(Comic))
            {
                Comic komik = (Comic)phase;
                komik.toJson(writer);
            }
            else{
                Phase fase = (Phase)phase;
                fase.toJson(writer);
            }
        }
        writer.WriteArrayEnd();
        writer.WriteObjectEnd();
    }
    public Story StoryOld(string filename)
    {
        JsonData jsonStory;
        TextAsset teksJson;
        teksJson = Resources.Load("Data/" + filename) as TextAsset;
        jsonStory = JsonMapper.ToObject(teksJson.ToString());
        foreach (JsonData cerita in jsonStory["phase"])
        {
            if (cerita["type"].ToString() == "Dialogue")
            {
                Dialogue dialog = new Dialogue(cerita);
                this.phases.Add(dialog);
            }
            else if (cerita["type"].ToString() == "MotionPic")
            {
                MotionPic MoPic = new MotionPic(cerita);
                this.phases.Add(MoPic);
            }
        }
        return this;
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