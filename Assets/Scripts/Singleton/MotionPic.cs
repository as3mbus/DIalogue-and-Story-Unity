using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LitJson;

public class MotionPic
{
    public string name;
    public Comic comic;
    public List<int> pages = new List<int>();
    public List<float> zooms = new List<float>();
    public List<Vector3> paths = new List<Vector3>();
    public List<string> animations = new List<string>();
    public MotionPic()
    {
        this.name = "";
        this.comic = new Comic("sample comic");
    }
    public MotionPic(string name, string comicDir)
    {
        this.name = name;
        this.comic = new Comic(comicDir);
    }

    public MotionPic(JsonData dialogueData)
    {
        this.name = dialogueData["name"].ToString();
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
            this.zooms.Add(float.Parse(dialogueData["zoom"][i].ToString()));
            this.animations.Add(dialogueData["character"][i].ToString());
        }
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
        writer.WritePropertyName("type");
        writer.Write("MotionPic");
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
        writer.WritePropertyName("animation");
        writer.WriteArrayStart();
        foreach (var item in this.animations)
        {
            writer.Write(item);
        }
        writer.WriteArrayEnd();
        writer.WriteObjectEnd();
    }
	public void newLine()
    {
        this.animations.Add("");
        this.pages.Add(0);
        this.zooms.Add(5f);
        this.paths.Add(new Vector3());
    }

    public void deleteLine(int index)
    {
        this.animations.RemoveAt(index);
        this.pages.RemoveAt(index);
        this.zooms.RemoveAt(index);
        this.paths.RemoveAt(index);
    }

    public void UpdateLine(string character, string message, int pageNo, float zoom, Vector3 path, int index)
    {
        this.animations[index] = character;
        this.pages[index] = pageNo;
        this.zooms[index] = zoom;
        this.paths[index] = path;
        Debug.Log(this.toJson());
    }

    public void insertLine(int index)
    {
        this.animations.Insert(index, "");
        this.pages.Insert(index, 0);
        this.zooms.Insert(index, 5f);
        this.paths.Insert(index, new Vector3());
    }
}
