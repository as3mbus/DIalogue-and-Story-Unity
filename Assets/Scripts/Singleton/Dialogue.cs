using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class Dialogue
{
    public List<string> characters = new List<string>();
    public List<string> messages = new List<string>();
    public List<int> pages = new List<int>();
    public List<float> zooms = new List<float>();
    public List<Vector3> paths = new List<Vector3>();
    public Comic comic;
    public Dialogue(){

    }
    public Dialogue(JsonData chara, JsonData msg)
    {
        for (int i = 0; i < chara.Count; i++)
        {
            this.characters.Add(chara[i].ToString());
            this.messages.Add(msg[i].ToString());
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
        foreach (var item in this.pages)
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
        writer.WritePropertyName("zoom");
        writer.WriteArrayStart();
        foreach (var item in this.zooms)
        {
            writer.Write(item);
        }
        writer.WriteArrayEnd();
        writer.WritePropertyName("character");
        writer.WriteArrayStart();
        foreach (var item in this.characters)
        {
            writer.Write(item);
        }
        writer.WriteArrayEnd();
        writer.WritePropertyName("message");
        writer.WriteArrayStart();
        foreach (var item in this.messages)
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

                this.pages.Add((int)dialogueData["page"][i]);
                this.paths.Add(
                    new Vector3(
                        float.Parse(dialogueData["x"][i].ToString()),
                        float.Parse(dialogueData["y"][i].ToString()),
                        -10f
                    )
                );
                Debug.Log("Testing " + paths[i].ToString());
                this.zooms.Add(float.Parse(dialogueData["zoom"][i].ToString()));
                this.characters.Add(dialogueData["character"][i].ToString());
                this.messages.Add(dialogueData["message"][i].ToString());
            }
        }
        catch (System.Exception)
        {
            for (int i = 0; i < dialogueData["message"].Count; i++)
            {
                this.characters.Add(dialogueData["character"][i].ToString());
                this.messages.Add(dialogueData["message"][i].ToString());
            }
            throw;
        }

    }
    public void newLine(){
        this.characters.Add("");
        this.messages.Add("");
        this.pages.Add(0);
        this.zooms.Add(5f);
        this.paths.Add(new Vector3());
    }
    public void deleteLine(int index){
        this.characters.RemoveAt(index);
        this.messages.RemoveAt(index);
        this.pages.RemoveAt(index);
        this.zooms.RemoveAt(index);
        this.paths.RemoveAt(index);
    }
    public void UpdateLine(string character,string message, int pageNo, float zoom, Vector3 path, int index ){
        this.characters[index]=character;
        this.messages[index]=message;
        this.pages[index]=pageNo;
        this.zooms[index]=zoom;
        this.paths[index]=path;
    }
    public void insertLine(int index){
        this.characters.Insert(index,"");
        this.messages.Insert(index,"");
        this.pages.Insert(index,0);
        this.zooms.Insert(index,5f);
        this.paths.Insert(index,new Vector3());
    }
}

// [System.Serializable]
// public class JsonDialogue
// {
//     public string comic;
//     public string[] character;
//     public string[] message;
//     public int[] page;
//     public float[] zoom;
//     public string[] x, y, z;
//     public JsonDialogue(Dialogue source)
//     {
//         this.comic = source.comic.ToString();
//         this.character = source.characters.ToArray();
//         this.message = source.messages.ToArray();
//         this.page = source.pages.ToArray();
//         this.zoom = source.zooms.ToArray();

//         var pathcount = source.paths.Count;
//         this.x = new string[pathcount];
//         this.y = new string[pathcount];
//         this.z = new string[pathcount];
//         for (int i = 0; i < pathcount; i++)
//         {
//             this.x[i] = source.paths[i].x.ToString();
//             this.y[i] = source.paths[i].y.ToString();
//             this.z[i] = source.paths[i].z.ToString();
//         }

//     }
// }