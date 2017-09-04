using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Comic
{
    public string toString(){
        return source;
    }
    public List<Sprite> pages = new List<Sprite>();
    public string source;

    public Comic(JsonData directory)
    {
        this.source = directory.ToString();
        foreach (var item in Resources.LoadAll<Sprite>("Comic/" + directory.ToString()))
        {
            this.pages.Add(item);
        }
    }
}