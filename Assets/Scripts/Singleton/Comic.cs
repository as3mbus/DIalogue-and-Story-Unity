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

        public Comic(string bundleName, string comicPath)
        {
            this.name = Path.GetFileName(comicPath);
            this.source = bundleName.ToString();
            AssetBundle comicBundle = ComicManager.readStreamBundles(ComicManager.bundlePath(bundleName));
            foreach (string asetname in comicBundle.GetAllAssetNames())
                if (asetname.Contains(comicPath))
                    this.pages.Add(comicBundle.LoadAsset<Sprite>(asetname));
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
