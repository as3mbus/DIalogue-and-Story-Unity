using System.Collections.Generic;
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