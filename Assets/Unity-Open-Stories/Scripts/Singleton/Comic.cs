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
        public string comicDirectory;
        public bool loaded = false;
        public List<Sprite> pages = new List<Sprite>();
        public string bundleName;


        //create comic data by loading comic folder inside a asset bundle
        public Comic(string bundleName, string comicDirectory)
        {
            this.bundleName = bundleName;
            this.comicDirectory = comicDirectory;
            this.loaded = false;
        }
        // handle story and comic in the same bundle
        public void loadAllPages()
        {
            if (!this.loaded)
            {
                AssetBundle bundle = DataManager.readAssetBundles(DataManager.bundlePath(this.bundleName));
                this.bundleName = bundle.name;
                this.pages = new List<Sprite>();
                foreach (string asetName in bundle.GetAllAssetNames())
                    if (asetName.Contains(this.comicDirectory))
                    {
                        this.pages.Add(bundle.LoadAsset<Sprite>(asetName));
                    }
                bundle.Unload(false);
                this.loaded = true;
            }
        }
        public void loadPages(int[] pageNums)
        {
            AssetBundle bundle = DataManager.readAssetBundles(DataManager.bundlePath(this.bundleName));
            this.bundleName = bundle.name;
            int n = -1;
            foreach (string asetName in bundle.GetAllAssetNames())
            {
                if (asetName.Contains(comicDirectory))
                {
                    n++;
                    if (pageNums.Contains(n))
                    {

                        Debug.Log(n);
                        this.pages.Add(bundle.LoadAsset<Sprite>(asetName));
                    }
                    else
                        this.pages.Add(null);
                }
            }
            bundle.Unload(false);

        }

        //load comic on resource folder @deprecated 
        public Comic(string resDir)
        {
            this.bundleName = resDir.ToString();
            foreach (var item in Resources.LoadAll<Sprite>("Comic/" + resDir.ToString()))
                this.pages.Add(item);

        }
        // return page name of every pages
        public List<string> pageName
        {
            get
            {
                List<string> names = new List<string>();
                foreach (Sprite item in this.pages)
                {
                    names.Add(item.name);
                }
                return names;
            }
        }
        // return list of folder containing file with image extension inside an asset bundle
        public static string[] getComics(AssetBundle comicBundle)
        {
            List<string> comBunCon = new List<string>();
            foreach (string asset in comicBundle.GetAllAssetNames())
                if ((asset.Contains(".png") || asset.Contains(".jpg") || asset.Contains(".jpeg")) && !comBunCon.Contains(Path.GetDirectoryName(asset)))
                    comBunCon.Add(Path.GetDirectoryName(asset));
            comicBundle.Unload(false);
            return comBunCon.ToArray();
        }
    }
}
