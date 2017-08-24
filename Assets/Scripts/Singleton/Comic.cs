using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Comic
{
    public List<Sprite> pages = new List<Sprite>();

    public Comic(JsonData directory)
    {
        foreach (var item in Resources.LoadAll<Sprite>("Comic/" + directory.ToString()))
        {
            this.pages.Add(item);
        }
    }
}