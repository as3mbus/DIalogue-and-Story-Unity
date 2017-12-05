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

        public void toJson(JsonWriter writer)
        {

            writer.WriteObjectStart();
            writer.WritePropertyName("page");
            writer.Write(this.page);
            writer.WritePropertyName("duration");
            writer.Write(this.duration);
            writer.WritePropertyName("fadeMode");
            writer.Write(this.fadeMode.ToString("g"));
            writer.WritePropertyName("camera");
            this.cameraEffects.toJson(writer);
            writer.WriteObjectEnd();

        }
        public string toJson()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.IndentValue = 4;
            toJson(writer);
            return sb.ToString();
        }

        public static Effects parseJson_1_0(JsonData fxJsonData, int lineIndex)
        {
            Effects fx = Effects.parseJson_1_0(fxJsonData,lineIndex,EffectsJsonKey.V_1_0);
            return fx;
        }
        public static Effects parseJson_1_0(JsonData fxJsonData, int indexLine, EffectsJsonKey fxKey)
        {
            Effects fx = new Effects();
            fx.page = (int)fxJsonData[fxKey.keys[0]][indexLine];
            fx.duration = float.Parse(fxJsonData[fxKey.keys[1]][indexLine].ToString());
            fx.fadeMode = Effects.parseFadeMode(fxJsonData[fxKey.keys[2]][indexLine].ToString());
            fx.cameraEffects = CameraEffects.parseJson_1_0(fxJsonData, indexLine, fxKey.cameraEffectsKey);
            return fx;
        }

    }
    public class EffectsJsonKey
    {
        public string[] keys = new string[] { "comicPage", "duration", "fadeMode", "camera" };
        public CameraEffectsJsonKey cameraEffectsKey = new CameraEffectsJsonKey();
        public EffectsJsonKey(string[] newKeys, CameraEffectsJsonKey newCameraEffectsKey)
        {
            for (int i = 0; i < newKeys.Length; i++)
                if (!String.IsNullOrEmpty(newKeys[i]))
                    keys[i] = newKeys[i];
            if (newCameraEffectsKey != null)
                cameraEffectsKey = newCameraEffectsKey;
        }
        public EffectsJsonKey() { }

        public static EffectsJsonKey V_1_0
        {
            get { return new EffectsJsonKey(new string[] { "page", "duration", "fademode", "camera" }, CameraEffectsJsonKey.V_1_0); }
        }

    }
}