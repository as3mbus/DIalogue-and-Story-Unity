using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using LitJson;

public static class ComicManager
{
    public static List<string> streamBundleList;
    //retrive all available comic inside an assetbundle 
    public static string[] getComics(AssetBundle comicBundle)
    {
        List<string> comBunCon = new List<string>();
        foreach (string asset in comicBundle.GetAllAssetNames())
        {
            if (asset.Contains(".png") && !comBunCon.Contains(Path.GetDirectoryName(asset)))
            {
                comBunCon.Add(Path.GetDirectoryName(asset));
                // Debug.Log(Path.GetDirectoryName(asset));
            }
        }
        comicBundle.Unload(false);
        return comBunCon.ToArray();
    }
    //list all asset bundle inside streaming asset folder which is packaged into apk on built 
    public static void listStreamingComicsBundleJson(string filePath)
    {
        var streamingPath = Application.streamingAssetsPath;
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = true;
        writer.IndentValue = 4;
        writer.WriteObjectStart();
        writer.WritePropertyName("streamingcombun");
        writer.WriteArrayStart();
        FileInfo[] bundles = new DirectoryInfo(streamingPath).GetFiles();
        List<string> comicBundles = new List<string>();
        foreach (var item in bundles)
            if (!item.Name.Contains("."))
            {
                comicBundles.Add(item.Name);
                writer.Write(item.Name);
            }
        writer.WriteArrayEnd();
        writer.WriteObjectEnd();
        writeStringBuilder(sb, filePath);
    }
    //read json data about asset bundle inside streaming assets folder. to access streaming asset in android build 
    public static string[] readComicsBundleList(string filePath)
    {
        streamBundleList = new List<string>(parseComicBundleJson(readStreamTextFile(filePath)));
        return streamBundleList.ToArray();
    }

    //parse json that contain list of asset bundle in streaming asset
    public static string[] parseComicBundleJson(string jsonText)
    {
        JsonData jsonComBun = JsonMapper.ToObject(jsonText);
        List<string> comBuns = new List<string>();
        foreach (JsonData comBun in jsonComBun["streamingcombun"])
            comBuns.Add(comBun.ToString());
        return comBuns.ToArray();
    }
    //list comic folder and it's content in streaming asset folder @deprecated 
    public static void listStreamingComicsJson(string filePath)
    {
        var comicPath = Path.Combine(Application.dataPath, "Comic");
        var comicDirectories = new DirectoryInfo(comicPath).GetDirectories();
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = true;
        writer.IndentValue = 4;

        writer.WriteObjectStart();
        writer.WritePropertyName("comic");
        writer.WriteArrayStart();
        foreach (DirectoryInfo dir in comicDirectories)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName("name");
            writer.Write(dir.Name);
            writer.WritePropertyName("content");
            writer.WriteArrayStart();
            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Extension == ".png")
                    writer.Write(Path.GetFileNameWithoutExtension(file.Name));
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
        writer.WriteArrayEnd();
        writer.WriteObjectEnd();
        writeStringBuilder(sb, filePath);
    }

    // read all comics inside asset bundle @deprecated and ??????
    public static string[] getComicsListFromBundle(string bundlePath, string assetPath)
    {
        AssetBundle comicBundle = readStreamBundles(bundlePath);
        TextAsset comicjson = comicBundle.LoadAsset<TextAsset>(assetPath);
        comicBundle.Unload(false);
        return parseComicListJson(comicjson.text);
    }
    //read bundle inside streaming asset path 
    public static AssetBundle readStreamBundles(string bundlePath){
        byte[] bundleByte = loadStreamingAssetFile(bundlePath);
        return AssetBundle.LoadFromMemory(bundleByte);
    }
    // read comic list inside streaming asset @deprecated
    public static string[] getComicsList(string pathFile)
    {
        return parseComicListJson(readStreamTextFile(pathFile));
    }
    //read textfile in streaming asset path into string 
    public static string readStreamTextFile(string pathFile)
    {
        byte[] jsonByte = loadStreamingAssetFile(pathFile);
        string jsonText = System.Text.Encoding.Default.GetString(jsonByte);
        return jsonText;
    }
    //parse comic list inside streaming asset directory to list all available comic in streaming asset @deprecated
    public static string[] parseComicListJson(string jsonText)
    {
        JsonData jsonComic = JsonMapper.ToObject(jsonText);
        string[] comicList = new string[jsonComic["comic"].Count];
        for (int i = 0; i < jsonComic["comic"].Count; i++)
            comicList[i] = jsonComic["comic"][i]["name"].ToString();
        return comicList;
    }
    //load file in streaming assets as bytes [] 
    static byte[] loadStreamingAssetFile(string filePath)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW data = new WWW(filePath);
            while (!data.isDone) { }
            return data.bytes;
        }
        else
            return File.ReadAllBytes(filePath);
    }
    //write a file using designated string builder into designated path 
    public static void writeStringBuilder(StringBuilder sb, string filePath)
    {
        var sr = File.CreateText(filePath);
        sr.Write(sb.ToString());
        sr.Close();
    }
    //check if bundle name is a content of streaming asset bundles 
    public static bool streamContent(string bundleName){
        return streamBundleList.Contains(bundleName);
    }
    //fetch streaming asset path for updating and new content after build release 
    public static string bundlePath(string bundleName){
        if(streamContent(bundleName))
            return Path.Combine(Application.streamingAssetsPath, bundleName);
        else
            return Path.Combine(Application.persistentDataPath, bundleName);
    }
}
