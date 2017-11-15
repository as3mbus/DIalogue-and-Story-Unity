using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using LitJson;

public static class ComicManager
{
    public static string[] getComics(AssetBundle comicBundle)
    {
        List<string> comBunCon = new List<string>();
        foreach (string asset in comicBundle.GetAllAssetNames())
        {
            if ((asset.Contains(".png") || asset.Contains(".jpg") || asset.Contains(".jpeg")) && !comBunCon.Contains(Path.GetDirectoryName(asset)))
            {
                comBunCon.Add(Path.GetDirectoryName(asset));
                // Debug.Log(Path.GetDirectoryName(asset));
            }
        }
        comicBundle.Unload(false);
        return comBunCon.ToArray();
    }
}
