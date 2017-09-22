using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
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
    public Comic(string name, string comicDirName)
    {
        this.name=name;
        Debug.Log(this.name);
        this.source = comicDirName.ToString();
        foreach (var item in Resources.LoadAll<Sprite>("Comic/" + comicDirName.ToString()))
        {
            Debug.Log(item.name);
            this.pages.Add(item);
        }
    }
    public Comic(string name, string comicDirName, AssetBundle asetbundle)
    {
        this.name=name;
        Debug.Log(this.name);
        this.source = comicDirName.ToString();
        foreach (string asetname in asetbundle.GetAllAssetNames())
        {
            if (asetname.Contains(comicDirName))
            {
               this.pages.Add(asetbundle.LoadAsset<Sprite>(asetname));
            }
        }
    }
    public Comic(JsonData directory): this(directory.ToString())
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
    public string toJson(){
        return "";
    }
    public void toJson(JsonWriter writer){
        
    }
    public static string listComicsJson()
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
        return sb.ToString();
    }
    public static void writeComicsJson()
    {
        var sr = File.CreateText(Path.Combine(Application.dataPath, "Comic/comics.json"));
        sr.Write(listComicsJson());
        sr.Close();
    }
    public static string[] getComicDataList()
    {
        
        JsonData jsonComic;
        if (Application.platform == RuntimePlatform.Android)
        {
            jsonComic = loadComicsDataAndroid();
        }
        else
        {
            jsonComic = loadComicsDataDesktop();
        }
        string[] comicList = new string[jsonComic["comic"].Count];
        for (int i = 0; i < jsonComic["comic"].Count; i++)
        {
            comicList[i] =  jsonComic["comic"][i]["name"].ToString();
        }
        return comicList;
    }
    void loadComicsBundles()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            loadComicsDataAndroid();
        }
        else
        {
            loadComicsDataDesktop();
        }
        
    }
    static JsonData loadComicsBundle(){
        string comicPath = Path.Combine(Application.streamingAssetsPath, "Comics") ;
        AssetBundle comicBundle = AssetBundle.LoadFromFile(comicPath);
        TextAsset comicjson = comicBundle.LoadAsset<TextAsset>("assets/comic/comics.json");
        JsonData jsonComic = JsonMapper.ToObject(comicjson.text);
        return jsonComic;
    }
    static JsonData loadComicsDataAndroid()
    {
        string comicPath = Application.streamingAssetsPath + "/comics.json";
        WWW data = new WWW(comicPath);
        while (!data.isDone) { }
        string text = data.text;
        JsonData jsonComic = JsonMapper.ToObject(text);
        return jsonComic;
    }
    static JsonData loadComicsDataDesktop()
    {
        string comicPath = Application.streamingAssetsPath + "/comics.json";
        string text = (File.ReadAllText(comicPath));
        JsonData jsonComic = JsonMapper.ToObject(text);
        return jsonComic;
    }
}