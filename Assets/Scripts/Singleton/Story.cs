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
        foreach (var phase in phases)
        {
            if (phase.GetType() == typeof(Dialogue))
            {
                Dialogue dialog = (Dialogue)phase;
                dialog.toJson(writer);
            }
            else
            {
                Comic komik = (Comic)phase;
                komik.toJson(writer);
            }
        }
        return "";
    }
    public Story(string filename)
    {
        JsonData jsonStory;
        TextAsset teksJson;
        teksJson = Resources.Load("Data/" + filename) as TextAsset;
        jsonStory = JsonMapper.ToObject(teksJson.ToString());
        foreach (JsonData cerita in jsonStory["Story"])
        {
            if (cerita["type"].ToString() == "Dialogue")
            {
                Dialogue dialog = new Dialogue(cerita);
                this.phases.Add(dialog);
            }
            else if (cerita["type"].ToString() == "Comic")
            {
                Comic komik = new Comic(cerita["content"]);
                this.phases.Add(komik);
            }
        }
    }

}