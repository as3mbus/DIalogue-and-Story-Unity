using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LitJson;

public class Comic
{
    public string name;
    public string toString()
    {
        return source;
    }
    public List<Sprite> pages = new List<Sprite>();
    public string source;
    public Comic(string name, string comicDir)
    {
        this.name=name;
        Debug.Log(this.name);
        this.source = comicDir.ToString();
        foreach (var item in Resources.LoadAll<Sprite>("Comic/" + comicDir.ToString()))
        {
            this.pages.Add(item);
        }
    }
    public Comic(JsonData directory)
    {
        this.source = directory.ToString();
        foreach (var item in Resources.LoadAll<Sprite>("Comic/" + directory.ToString()))
        {
            this.pages.Add(item);
        }
    }
    public Comic(string resDir)
    {
        this.source = resDir.ToString();
        foreach (var item in Resources.LoadAll<Sprite>("Comic/" + resDir.ToString()))
        {
            this.pages.Add(item);
        }
    }
    public string toJson(){
        return "";
    }
    public void toJson(JsonWriter writer){
        
    }
}