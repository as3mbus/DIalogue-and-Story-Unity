using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using LitJson;

public static class DataManager
{
    public static List<string> streamingAssetBundleList;

    //list all asset bundle inside streaming asset folder which is packaged into apk on built 
    public static void listStreamingAssetBundleJson(string filePath)
    {
        var streamingPath = Application.streamingAssetsPath;
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = true;
        writer.IndentValue = 4;
        writer.WriteObjectStart();
        writer.WritePropertyName("streamingcombun");
        writer.WriteArrayStart();
        FileInfo[] files = new DirectoryInfo(streamingPath).GetFiles();
        List<string> comicBundles = new List<string>();
        foreach (var item in files)
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
    public static string[] readStreamingAssetBundleList(string filePath)
    {
        streamingAssetBundleList = new List<string>(parseStreamingAssetBundleListJson(readAssetsTextFile(filePath)));
        return streamingAssetBundleList.ToArray();
    }

    //parse json that contain list of asset bundle in streaming asset
    public static string[] parseStreamingAssetBundleListJson(string jsonText)
    {
        JsonData jsonComBun = JsonMapper.ToObject(jsonText);
        List<string> comBuns = new List<string>();
        foreach (JsonData comBun in jsonComBun["streamingcombun"])
            comBuns.Add(comBun.ToString());
        return comBuns.ToArray();
    }

    //read bundle inside streaming asset path 
    public static AssetBundle readAssetBundles(string bundlePath)
    {
        byte[] bundleByte = loadAssetsFile(bundlePath);
        return AssetBundle.LoadFromMemory(bundleByte);
    }

    //read textfile in streaming asset path into string 
    public static string readAssetsTextFile(string pathFile)
    {
        byte[] jsonByte = loadAssetsFile(pathFile);
        string jsonText = System.Text.Encoding.Default.GetString(jsonByte);
        return jsonText;
    }

    //load file in streaming assets as bytes [] 
    static byte[] loadAssetsFile(string filePath)
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
    public static bool isStreamingAssetsContent(string bundleName)
    {
        return streamingAssetBundleList.Contains(bundleName);
    }

    //fetch streaming asset path for updating and new content after build release 
    public static string bundlePath(string bundleName)
    {
        if (isStreamingAssetsContent(bundleName))
            return Path.Combine(Application.streamingAssetsPath, bundleName);
        else
            return Path.Combine(Application.persistentDataPath, bundleName);
    }

    public static string findItemInBundle(AssetBundle bundle, string keyword)
    {
        foreach (string item in bundle.GetAllAssetNames())
        {
            Debug.Log(item);
            if (item.Contains(keyword))
                return item;
        }
        return "";
    }
    public static string[] filterItemInBundle(AssetBundle bundle, string keyword)
    {
        List<string> filteredItem = new List<string>();
        foreach (string item in bundle.GetAllAssetNames())
            if (item.Contains(keyword))
                filteredItem.Add(item);
        return filteredItem.ToArray();
    }
}