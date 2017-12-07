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
    // filter item in asset bundle base by keyword
    public static string[] filterItemInBundle(AssetBundle bundle, string keyword)
    {
        List<string> filteredItem = new List<string>();
        foreach (string item in bundle.GetAllAssetNames())
            if (item.Contains(keyword))
                filteredItem.Add(item);
        return filteredItem.ToArray();
    }
    public static bool isSameBundle(AssetBundle bundle, string assetBundleName)
    {
        if (bundle != null)
        {
            if (bundle.name == assetBundleName)
                return true;
            else
                return false;
        }
        else
            return false;
    }
}
// class that handles json key value
public class JsonKey
{
    public string objectName;
    public string[] elementsKeys;
    public List<JsonKey> elementsjsonKey;

    public JsonKey(string objName, string[] elmtKeys, List<JsonKey> elmtjsKeys)
    {
        this.objectName = objName;
        this.elementsKeys = elmtKeys;
        this.elementsjsonKey = elmtjsKeys;
        // Debug.Log(elementsKeys[0]);
    }
    override public string ToString()
    {
        string keyString = objectName + " containing \n";
        foreach (string key in elementsKeys)
            keyString += key + "\n";
        if (elementsjsonKey != null)
            foreach (JsonKey jskey in elementsjsonKey)
                keyString += jskey.ToString();
        return keyString;
    }
    public bool equal(JsonKey obj)
    {
        if (this.objectName == obj.objectName)
            if (this.elementsKeys.Length == obj.elementsKeys.Length)
            {
                for (int i = 0; i < this.elementsKeys.Length; i++)
                    if (this.elementsKeys[i] != obj.elementsKeys[i])
                        return false;
                if (this.elementsjsonKey == null && obj.elementsjsonKey == null)
                    return true;
                if (this.elementsjsonKey.Count == obj.elementsjsonKey.Count)
                    for (int i = 0; i < this.elementsjsonKey.Count; i++)
                        if (!this.elementsjsonKey[i].equal(obj.elementsjsonKey[i]))
                            return false;
                return true;
            }
        return false;
    }
}

public class StoryJsonKey
{
    Dictionary<string, JsonKey> jsonKeys = new Dictionary<string, JsonKey>()
    {
        {"Story", null},
        {"Phase",null},
        {"Line", null},
        {"Effects", null},
        {"CameraEffects", null}
    };
    public StoryJsonKey(JsonKey storyKey, JsonKey phaseKey, JsonKey lineKey, JsonKey effectsKey, JsonKey cameraEffectskey)
    {
        this.jsonKeys["Story"] = storyKey;
        this.jsonKeys["Phase"] = phaseKey;
        this.jsonKeys["Line"] = lineKey;
        this.jsonKeys["Effects"] = effectsKey;
        this.jsonKeys["CameraEffects"] = cameraEffectskey;
        this.connectKey();

    }
    public void connectKey()
    {
        this.Story.elementsjsonKey = new List<JsonKey>();
        this.Phase.elementsjsonKey = new List<JsonKey>();
        this.Line.elementsjsonKey = new List<JsonKey>();
        this.Effects.elementsjsonKey = new List<JsonKey>();
        this.Story.elementsjsonKey.Insert(0, this.Phase);
        this.Phase.elementsjsonKey.Insert(0, this.Line);
        this.Line.elementsjsonKey.Insert(0, this.Effects);
        this.Effects.elementsjsonKey.Insert(0, this.CameraEffects);
    }
    public JsonKey Story
    {
        get { return jsonKeys["Story"]; }
    }
    public JsonKey Phase
    {
        get { return jsonKeys["Phase"]; }
    }
    public JsonKey Line
    {
        get { return jsonKeys["Line"]; }
    }
    public JsonKey Effects
    {
        get { return jsonKeys["Effects"]; }
    }
    public JsonKey CameraEffects
    {
        get { return jsonKeys["CameraEffects"]; }
    }

    public static StoryJsonKey V_1_0
    {
        get
        {
            return new StoryJsonKey
            (
                new JsonKey
                (
                    "",
                    new string[] { "version", "name" },
                    null
                ),
                new JsonKey
                (
                    "phase",
                    new string[] { "name", "comicsource", "comicname", "", "" },
                    null
                ),
                new JsonKey
                (
                    "",
                    new string[] { "message", "character" },
                    null
                ),
                new JsonKey
                (
                    "",
                    new string[] { "page", "duration", "fademode" },
                    null
                ),
                new JsonKey
                (
                    "camera",
                    new string[] { "camx", "camy", "zoom", "shake", "background" },
                    null
                )
            );
        }
    }
    public static StoryJsonKey V_1_1
    {
        get
        {
            return new StoryJsonKey
            (
                storyKey: new JsonKey
                (
                    "",
                    V_1_0.Story.elementsKeys,
                    null
                ),
                phaseKey: new JsonKey
                (
                    "phase",
                    V_1_0.Phase.elementsKeys,
                    null
                ),
                lineKey: new JsonKey
                (
                    "content",
                    V_1_0.Line.elementsKeys,
                    null
                ),
                effectsKey: new JsonKey
                (
                    "effect",
                    V_1_0.Effects.elementsKeys,
                    null
                ),
                cameraEffectskey: new JsonKey
                (
                    "camera",
                    new string[] { "x", "y", "size", V_1_0.CameraEffects.elementsKeys[3], "backgroundcolor" },
                    null
                )
            );
        }
    }
    public static StoryJsonKey V_1_2
    {
        get
        {
            return new StoryJsonKey
            (
                storyKey: new JsonKey
                (
                    "",
                    V_1_1.Story.elementsKeys,
                    null
                ),
                phaseKey: new JsonKey
                (
                    "phase",
                    new string[] { V_1_1.Phase.elementsKeys[0], "comicAssetBundleName", "comicDirectoryName", "bgmAssetBundleName", "bgmFileName" },
                    null
                ),
                lineKey: new JsonKey
                (
                    "content",
                    V_1_1.Line.elementsKeys,
                    null
                ),
                effectsKey: new JsonKey
                (
                    "effects",
                    new string[] { "comicPage", V_1_1.Effects.elementsKeys[1], "fadeMode" },
                    null
                ),
                cameraEffectskey: new JsonKey
                (
                    "camera",
                    new string[] { V_1_1.CameraEffects.elementsKeys[0], V_1_1.CameraEffects.elementsKeys[1], "orthographicSize", "shakeFrequency", "backgroundColor" },
                    null
                )
            );
        }
        // static string effectsName = "effects";
        // static string[] effectsKey = new string[] { "comicPage", V_1_0.Effects.elementsKeys[1], "fadeMode", V_1_0.Effects.elementsKeys[3] };
        // public static JsonKey Effects
        // {
        //     get { return new JsonKey(effectsName, effectsKey, new List<JsonKey> { CameraEffects }); }
        // }
        // public static string cameraEffectsName = V_1_0.CameraEffects.objectName;
        // public static string[] cameraEffectskey = new string[] { V_1_1.CameraEffects.elementsKeys[0], V_1_1.CameraEffects.elementsKeys[1], "orthographicSize", "shakeFrequency", "backgroundColor" };
        // public static JsonKey CameraEffects
        // {
        //     get { return new JsonKey(cameraEffectsName, cameraEffectskey, null); }
        // }
    }
    public static StoryJsonKey Latest
    {
        get
        {
            return V_1_2;
        }

    }

}