using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LitJson;

namespace as3mbus.Story
{
    public enum fadeMode
    {
        color, transition, none
    }

    public class Effects
    {
        int page;
        float duration;
        fadeMode fadeMode;
        CameraEffects cameraEffects;

        public int Page { get { return page; } set { page = value; } }
        public float Duration { get { return duration; } set { duration = value; } }
        public fadeMode FadeMode { get { return fadeMode; } set { fadeMode = value; } }
        public CameraEffects CameraEffects { get { return cameraEffects; } set { cameraEffects = value; } }

        public static fadeMode parseFadeMode(string fM)
        {
            return
                fM.ToLower() == "transition" ? fadeMode.transition
                : fM.ToLower() == "color" ? fadeMode.color :
                fadeMode.none;
        }

        public static Color parseColorFromString(string s)
        {
            Color c;
            ColorUtility.TryParseHtmlString(s, out c);
            return c;
        }
        public Effects()
        {
            page = 0;
            duration = 1;
            fadeMode = fadeMode.none;
            cameraEffects = new CameraEffects();
        }
        public Effects(Effects cloneTarget)
        {
            page = cloneTarget.page;
            duration = cloneTarget.duration;
            fadeMode = cloneTarget.fadeMode;
            cameraEffects = new CameraEffects(cloneTarget.cameraEffects);
        }
        public Effects(int pej, float durat, fadeMode fm, CameraEffects camfx)
        {
            update(pej, durat, fm, camfx);
        }
        public bool update(int pej, float durat, fadeMode fm, CameraEffects camfx)
        {
            try
            {
                this.page = pej;
                this.duration = durat;
                this.fadeMode = fm;
                this.cameraEffects = camfx;
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        // Write Json string with latest version jsonkey using assigned writer
        public void writeJson(JsonWriter writer)
        {
            writeJson(writer, StoryJsonKey.Latest.Effects);
        }

        // Write Json string with latest version using assigned writer
        public void writeJson(JsonWriter writer, JsonKey fxKey)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName(fxKey.elementsKeys[0]);
            writer.Write(this.page);
            writer.WritePropertyName(fxKey.elementsKeys[1]);
            writer.Write(this.duration);
            writer.WritePropertyName(fxKey.elementsKeys[2]);
            writer.Write(this.fadeMode.ToString("g"));
            writer.WritePropertyName(fxKey.elementsjsonKey[0].objectName);
            this.cameraEffects.writeJson(writer, fxKey.elementsjsonKey[0]);
            writer.WriteObjectEnd();
        }

        // return Json string with latest version json key
        public string toJson()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.IndentValue = 4;
            writeJson(writer);
            return sb.ToString();
        }

        // parse json data using v1.0 method and v1.0 json key
        public static Effects parseJson_1_0(JsonData fxJsonData, int lineIndex)
        {
            return Effects.parseJson_1_0(fxJsonData, lineIndex, StoryJsonKey.V_1_0.Effects);
        }
        
        // parse json data using v1.0 method and specified json key
        public static Effects parseJson_1_0(JsonData fxJsonData, int indexLine, JsonKey fxKey)
        {
            Debug.Log(fxKey.elementsKeys[0]);
            Effects fx = new Effects();
            fx.page = (int)fxJsonData[fxKey.elementsKeys[0]][indexLine];
            fx.duration = float.Parse(fxJsonData[fxKey.elementsKeys[1]][indexLine].ToString());
            fx.fadeMode = Effects.parseFadeMode(fxJsonData[fxKey.elementsKeys[2]][indexLine].ToString());
            fx.cameraEffects = CameraEffects.parseJson_1_0(fxJsonData, indexLine, fxKey.elementsjsonKey[0]);
            return fx;
        }
        
        // parse json data using v1.1 method and specified json key
        public static Effects parseJson_1_1(JsonData fxJsonData, JsonKey fxKey)
        {
            Effects fx = new Effects();
            fx.page = (int)fxJsonData[fxKey.elementsKeys[0]];
            fx.duration = float.Parse(fxJsonData[fxKey.elementsKeys[1]].ToString());
            fx.fadeMode = Effects.parseFadeMode(fxJsonData[fxKey.elementsKeys[2]].ToString());
            fx.cameraEffects = CameraEffects.parseJson_1_1(fxJsonData[fxKey.elementsjsonKey[0].objectName], fxKey.elementsjsonKey[0]);
            return fx;
        }

    }
}