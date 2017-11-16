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
        CamEffects cameraEffects;

        public int Page { get { return page; } set { page = value; } }
        public float Duration { get { return duration; } set { duration = value; } }
        public fadeMode FadeMode { get { return fadeMode; } set { fadeMode = value; } }
        public CamEffects CameraEffects { get { return cameraEffects; } set { cameraEffects = value; } }

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
            cameraEffects = new CamEffects();
        }
        public Effects(Effects cloneTarget){
            page = cloneTarget.page;
            duration = cloneTarget.duration;
            fadeMode = cloneTarget.fadeMode;
            cameraEffects = new CamEffects(cloneTarget.cameraEffects);
        }
        public Effects(int pej, float durat, fadeMode fm, CamEffects camfx)
        {
            update(pej, durat, fm, camfx);
        }
        public bool update(int pej, float durat, fadeMode fm, CamEffects camfx)
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
            writer.WritePropertyName("fademode");
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

    }
}