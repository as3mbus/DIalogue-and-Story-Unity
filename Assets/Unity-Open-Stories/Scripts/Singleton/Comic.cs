using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using LitJson;
using System.Linq;

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
        public List<string> pagename = new List<string>();


        //create comic data by loading comic folder inside a asset bundle
        public Comic(string bundleName, string comicPath)
        {
            AssetBundle comicBundle = DataManager.readAssetsBundles(DataManager.bundlePath(bundleName));
            loadAllPages(comicBundle, comicPath);
            comicBundle.Unload(false);
        }

        public Comic(string bundleName, string comicPath, int[] pageNums)
        {
            AssetBundle comicBundle = DataManager.readAssetsBundles(DataManager.bundlePath(bundleName));
            loadPages(comicBundle, comicPath, pageNums);
            comicBundle.Unload(false);
        }
        public Comic(AssetBundle bundle, string comicDirectoryPath)
        {
            loadAllPages(bundle, comicDirectoryPath);
        }
        public Comic(AssetBundle bundle, string comicDirectoryPath, int[] pageNums)
        {
            loadPages(bundle, comicDirectoryPath, pageNums);

        }
        // handle story and comic in the same bundle

        public void loadAllPages(AssetBundle bundle, string comicPath)
        {
            this.name = Path.GetFileName(comicPath);
            this.source = bundle.name;
            foreach (string asetName in bundle.GetAllAssetNames())
                if (asetName.Contains(comicPath))
                {
                    this.pages.Add(bundle.LoadAsset<Sprite>(asetName));
                    this.pagename.Add(Path.GetFileNameWithoutExtension(asetName));
                }
        }
        public void loadPages(AssetBundle bundle, string comicPath, int[] pageNums)
        {
            this.name = Path.GetFileName(comicPath);
            this.source = bundle.name;
            int n = -1;
            foreach (string asetName in bundle.GetAllAssetNames())
            {
                n++;
                if (asetName.Contains(comicPath))
                {
                    if (pageNums.Contains(n))
                    {
                        Debug.Log("LOAD");
                        this.pages.Add(bundle.LoadAsset<Sprite>(asetName));
                        this.pagename.Add(Path.GetFileNameWithoutExtension(asetName));
                    }
                    else
                    {
                        this.pages.Add(null);
                        this.pagename.Add(null);
                    }
                }
            }

        }

        public Comic(JsonData directory) : this(directory.ToString())
        {
        }

        //load comic on resource folder @deprecated 
        public Comic(string resDir)
        {
            this.source = resDir.ToString();
            foreach (var item in Resources.LoadAll<Sprite>("Comic/" + resDir.ToString()))
                this.pages.Add(item);

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
