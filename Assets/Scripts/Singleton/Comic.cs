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
}