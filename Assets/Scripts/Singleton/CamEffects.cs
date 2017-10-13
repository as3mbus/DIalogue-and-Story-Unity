using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LitJson;

namespace as3mbus.Story
{
    public class CamEffects
    {
        Vector3 position;
        float size, shake;
        Color backgroundColor;

        public Vector3 Position { get {return position;} set {position = value;} }
        public float Size { get {return size;} set {size = value;} }
        public float Shake { get{return shake;} set {shake = value;} }
        public Color BackgroundColor { get {return backgroundColor;} set {backgroundColor = value;} }

        public CamEffects()
        {
            Size = 5f;
            Shake = 0f;
            BackgroundColor = Color.black;
            Position = new Vector3(0, 0, -10);
        }
        public CamEffects(Vector3 camPos, float camsz, float camshake, Color bgcolor){
            update(camPos,camsz,camshake,bgcolor);
        }
        public bool update(Vector3 camPos, float camsz, float camshake, Color bgcolor){
            try
            {
                this.Position = camPos;
                this.Size = camsz;
                this.Shake = camshake;
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
            writer.Write(this.Size);
            writer.WritePropertyName("shake");
            writer.Write(this.Shake);
            writer.WritePropertyName("backgroundcolor");
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
    }
}