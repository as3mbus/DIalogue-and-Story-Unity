using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using LitJson;

namespace as3mbus.Story
{
    public class CameraEffects
    {
        Vector3 position;
        float _orthographicSize, _shakeFrequency;
        Color backgroundColor;

        public Vector3 Position { get { return position; } set { position = value; } }
        public float orthographicSize { get { return _orthographicSize; } set { _orthographicSize = value; } }
        public float shakeFrequency { get { return _shakeFrequency; } set { _shakeFrequency = value; } }
        public Color BackgroundColor { get { return backgroundColor; } set { backgroundColor = value; } }

        public CameraEffects()
        {
            orthographicSize = 5f;
            shakeFrequency = 0f;
            BackgroundColor = Color.black;
            Position = new Vector3(0, 0, -10);
        }
        public CameraEffects(CameraEffects cloneTarget)
        {
            orthographicSize = cloneTarget._orthographicSize;
            shakeFrequency = cloneTarget._shakeFrequency;
            BackgroundColor = cloneTarget.backgroundColor;
            Position = cloneTarget.position;
        }
        public CameraEffects(Vector3 camPos, float camsz, float camshake, Color bgcolor)
        {
            update(camPos, camsz, camshake, bgcolor);
        }
        public bool update(Vector3 camPos, float camsz, float camshake, Color bgcolor)
        {
            try
            {
                this.Position = camPos;
                this.orthographicSize = camsz;
                this.shakeFrequency = camshake;
                this.BackgroundColor = bgcolor;
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
        public void toJson(JsonWriter writer)
        {

            writer.WriteObjectStart();
            writer.WritePropertyName("x");
            writer.Write(this.Position.x);
            writer.WritePropertyName("y");
            writer.Write(this.Position.y);
            writer.WritePropertyName("size");
            writer.Write(this.orthographicSize);
            writer.WritePropertyName("shake");
            writer.Write(this.shakeFrequency);
            writer.WritePropertyName("backgroundColor");
            writer.Write("#" + ColorUtility.ToHtmlStringRGB(this.BackgroundColor));
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

        public static CameraEffects parseJson_1_0(JsonData camFxJsonData, int lineIndex)
        {
            CameraEffects camFx = CameraEffects.parseJson_1_0(camFxJsonData, lineIndex, CameraEffectsJsonKey.V_1_0);
            return camFx;
        }
        public static CameraEffects parseJson_1_0(JsonData camFxJsonData, int lineIndex, CameraEffectsJsonKey camFxKey)
        {
            CameraEffects camFx = new CameraEffects();
            camFx.position = new Vector3(
                                    float.Parse(camFxJsonData[camFxKey.keys[0]][lineIndex].ToString()),
                                    float.Parse(camFxJsonData[camFxKey.keys[1]][lineIndex].ToString()),
                                    -10f
                            );
            camFx._orthographicSize = float.Parse(camFxJsonData[camFxKey.keys[2]][lineIndex].ToString());
            camFx._shakeFrequency = float.Parse(camFxJsonData[camFxKey.keys[3]][lineIndex].ToString());
            camFx.backgroundColor = Effects.parseColorFromString(camFxJsonData[camFxKey.keys[4]][lineIndex].ToString());
            return camFx;
        }
    }
    public class CameraEffectsJsonKey
    {
        public string[] keys = new string[] { "x", "y", "orthographicSize", "shakeFrequency", "backgroundColor" };
        public CameraEffectsJsonKey(string[] newKeys)
        {
            for (int i = 0; i < newKeys.Length; i++)
                if (!String.IsNullOrEmpty(newKeys[i]))
                    keys[i] = newKeys[i];
        }
        public CameraEffectsJsonKey() { }

        public static CameraEffectsJsonKey V_1_0
        {
            get { return new CameraEffectsJsonKey(new string[] { "camx", "camy", "zoom", "shake", "background" }); }
        }
    }
}