using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using LitJson;

namespace as3mbus.Story
{
    public class Comic
    {
        public string name;
        public string toString()
        {
            return source;
        }
        public List<Sprite> pages = new List<Sprite>();
        public string source;
        public Comic(string name, string comicPath)
        {
            this.name = name;
            Debug.Log(this.name);
            this.source = comicPath;
            foreach (var item in Resources.LoadAll<Sprite>("Comic/" + comicPath.ToString()))
            {
                Debug.Log(item.name);
                this.pages.Add(item);
            }
        }
        public Comic(string name, string comicPath, AssetBundle asetbundle)
        {
            this.name = name;
            Debug.Log(this.name);
            this.source = comicPath.ToString();
            foreach (string asetname in asetbundle.GetAllAssetNames())
            {
                if (asetname.Contains(comicPath))
                {
                    this.pages.Add(asetbundle.LoadAsset<Sprite>(asetname));
                }
            }
        }
        public Comic(JsonData directory) : this(directory.ToString())
        {
        }
        public Comic(string resDir)
        {
            this.source = resDir.ToString();
            foreach (var item in Resources.LoadAll<Sprite>("Comic/" + resDir.ToString()))
            {
                this.pages.Add(item);
            }
        }
        public string toJson()
        {
            return "";
        }
        public void toJson(JsonWriter writer)
        {

        }

    }
}
