using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class Dialogue
{
    public List<string> character = new List<string>();
    public List<string> message = new List<string>();
    public List<int> page = new List<int>();
    public List<float> zoom = new List<float>();
    public List<Vector3> paths = new List<Vector3>();
    public Comic comic;
    public Dialogue(JsonData chara, JsonData msg)
    {
        for (int i = 0; i < chara.Count; i++)
        {
            this.character.Add(chara[i].ToString());
            this.message.Add(msg[i].ToString());
        }
    }
    public string toJson()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint=true;
        writer.IndentValue=4;

        writer.WriteObjectStart();
        writer.WritePropertyName("type");
        writer.Write("dialogue");
        writer.WritePropertyName("comic");
        writer.Write(this.comic.toString());
        writer.WritePropertyName("page");
        writer.WriteArrayStart();
        foreach (var item in this.page)
        {
            writer.Write(item);
        }
        writer.WriteArrayEnd();
        writer.WritePropertyName("x");
        writer.WriteArrayStart();
        foreach (var item in this.paths)
        {
            writer.Write(item.x);
        }
        writer.WriteArrayEnd();
        writer.WritePropertyName("y");
        writer.WriteArrayStart();
        foreach (var item in this.paths)
        {
            writer.Write(item.y);
        }
        writer.WriteArrayEnd();
        writer.WritePropertyName("z");
        writer.WriteArrayStart();
        foreach (var item in this.paths)
        {
            writer.Write(item.z);
        }
        writer.WriteArrayEnd();
        writer.WritePropertyName("zoom");
        writer.WriteArrayStart();
        foreach (var item in this.zoom)
        {
            writer.Write(item);
        }
        writer.WriteArrayEnd();
        writer.WritePropertyName("character");
        writer.WriteArrayStart();
        foreach (var item in this.character)
        {
            writer.Write(item);
        }
        writer.WriteArrayEnd();
        writer.WritePropertyName("message");
        writer.WriteArrayStart();
        foreach (var item in this.message)
        {
            writer.Write(item);
        }
        writer.WriteArrayEnd();
        //writer.Write(string.Join("", new List<int>(array).ConvertAll(i => i.ToString()).ToArray()));
        // writer.WriteArrayEnd();

        writer.WriteObjectEnd();

        return sb.ToString();
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
                        float.Parse(dialogueData["x"][i].ToString()),
                        float.Parse(dialogueData["y"][i].ToString()),
                        float.Parse(dialogueData["z"][i].ToString())
                    )
                );
                Debug.Log("Testing " + paths[i].ToString());
                this.zoom.Add(float.Parse(dialogueData["zoom"][i].ToString()));
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

[System.Serializable]
public class JsonDialogue
{
    public string comic;
    public string[] character;
    public string[] message;
    public int[] page;
    public float[] zoom;
    public string[] x, y, z;
    public JsonDialogue(Dialogue source)
    {
        this.comic = source.comic.ToString();
        this.character = source.character.ToArray();
        this.message = source.message.ToArray();
        this.page = source.page.ToArray();
        this.zoom = source.zoom.ToArray();

        var pathcount = source.paths.Count;
        this.x = new string[pathcount];
        this.y = new string[pathcount];
        this.z = new string[pathcount];
        for (int i = 0; i < pathcount; i++)
        {
            this.x[i] = source.paths[i].x.ToString();
            this.y[i] = source.paths[i].y.ToString();
            this.z[i] = source.paths[i].z.ToString();
        }

    }
}