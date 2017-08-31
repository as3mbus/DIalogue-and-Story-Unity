using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class Dialogue
{
    public List<string> character = new List<string>();
    public List<string> message = new List<string>();
    public List<int> page = new List<int>();
    public List<float> zoom = new List<float>();
    public List<Vector3> paths;
    public Comic comic;
    public Dialogue(JsonData chara, JsonData msg)
    {
        for (int i = 0; i < chara.Count; i++)
        {
            this.character.Add(chara[i].ToString());
            this.message.Add(msg[i].ToString());
        }
    }
    public Dialogue(JsonData dialogueData)
    {
        try
        {
            this.comic = new Comic(dialogueData["comic"]);
            for (int i = 0; i < dialogueData["message"].Count; i++)
            {

                this.page.Add((int)dialogueData["page"][i]);
                this.paths.Add(
                    new Vector3(
                        (float)dialogueData["path"]["x"][i],
                        (float)dialogueData["path"]["y"][i],
                        (float)dialogueData["path"]["z"][i]
                    )
                );
                this.zoom.Add((float)dialogueData["zoom"][i]);
                this.character.Add(dialogueData["character"][i].ToString());
                this.message.Add(dialogueData["message"][i].ToString());
            }
        }
        catch (System.Exception)
        {
            for (int i = 0; i < dialogueData["message"].Count; i++)
            {
                this.character.Add(dialogueData["character"][i].ToString());
                this.message.Add(dialogueData["message"][i].ToString());
            }
            throw;
        }

    }
}