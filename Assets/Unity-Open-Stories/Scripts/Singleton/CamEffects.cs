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
        // Write Jsonstring with latest version
        public void writeJson(JsonWriter writer)
        {
            writeJson(writer, StoryJsonKey.Latest.CameraEffects);
        }
        // Write json with specified version
        public void writeJson(JsonWriter writer, JsonKey camKey)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName(camKey.elementsKeys[0]);
            writer.Write(this.Position.x);
            writer.WritePropertyName(camKey.elementsKeys[1]);
            writer.Write(this.Position.y);
            writer.WritePropertyName(camKey.elementsKeys[2]);
            writer.Write(this.orthographicSize);
            writer.WritePropertyName(camKey.elementsKeys[3]);
            writer.Write(this.shakeFrequency);
            writer.WritePropertyName(camKey.elementsKeys[4]);
            writer.Write("#" + ColorUtility.ToHtmlStringRGB(this.BackgroundColor));
            writer.WriteObjectEnd();
        }
        // output json string of the object
        public string toJson()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.IndentValue = 4;
            writeJson(writer);
            return sb.ToString();
        }
        // parse json using v1 method with version 1 key
        public static CameraEffects parseJson_1_0(JsonData camFxJsonData, int lineIndex)
        {
            return CameraEffects.parseJson_1_0(camFxJsonData, lineIndex, StoryJsonKey.V_1_0.CameraEffects);

        }
        // parse json using v1 method with specified version jsonkey
        public static CameraEffects parseJson_1_0(JsonData camFxJsonData, int lineIndex, JsonKey camFxKey)
        {
            CameraEffects camFx = new CameraEffects();
            Debug.Log(camFxKey.elementsKeys[0]);
            camFx.position = new Vector3(
                                        float.Parse(camFxJsonData[camFxKey.elementsKeys[0]][lineIndex].ToString()),
                                        float.Parse(camFxJsonData[camFxKey.elementsKeys[1]][lineIndex].ToString()),
                                        -10f
                            );
            camFx._orthographicSize = float.Parse(camFxJsonData[camFxKey.elementsKeys[2]][lineIndex].ToString());
            camFx._shakeFrequency = float.Parse(camFxJsonData[camFxKey.elementsKeys[3]][lineIndex].ToString());
            camFx.backgroundColor = Effects.parseColorFromString(camFxJsonData[camFxKey.elementsKeys[4]][lineIndex].ToString());
            return camFx;
        }
        // parse json using v1.1 method with latest version jsonkey
        public static CameraEffects parseJson_1_1(JsonData camFxJsonData)
        {
            return CameraEffects.parseJson_1_1(camFxJsonData, StoryJsonKey.Latest.CameraEffects);
        }
        // parse json using v1.1 method with specified jsonkey
        public static CameraEffects parseJson_1_1(JsonData camFxJsonData, JsonKey camFxKey)
        {
            CameraEffects camFx = new CameraEffects();
            Debug.Log(camFxKey.elementsKeys[4]);
            camFx.position = new Vector3(
                                        float.Parse(camFxJsonData[camFxKey.elementsKeys[0]].ToString()),
                                        float.Parse(camFxJsonData[camFxKey.elementsKeys[1]].ToString()),
                                        -10f
                            );
            camFx.orthographicSize = float.Parse(camFxJsonData[camFxKey.elementsKeys[2]].ToString());
            camFx.shakeFrequency = float.Parse(camFxJsonData[camFxKey.elementsKeys[3]].ToString());
            camFx.backgroundColor = Effects.parseColorFromString(camFxJsonData[camFxKey.elementsKeys[4]].ToString());
            return camFx;
        }
    }
}
