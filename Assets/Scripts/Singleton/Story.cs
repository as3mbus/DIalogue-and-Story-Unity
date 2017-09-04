using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Story
{
    public ArrayList phase = new ArrayList();

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
                this.phase.Add(dialog);
            }
            else if (cerita["type"].ToString() == "Comic")
            {
                Comic komik = new Comic(cerita["content"]);
                this.phase.Add(komik);
            }
        }
    }

}