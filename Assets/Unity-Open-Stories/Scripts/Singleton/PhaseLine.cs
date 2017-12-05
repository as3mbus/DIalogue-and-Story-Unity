using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace as3mbus.Story
{
    public class Line
    {
        Effects effects;
        string message;
        string character;

        public string Character { get { return character; } set { character = value; } }
        public string Message { get { return message; } set { message = value; } }
        public Effects Effects { get { return effects; } set { effects = value; } }

        public Line()
        {
            Message = "";
            Character = "";
            Effects = new Effects();
        }
        public Line(Effects efek)
        {
            Message = "";
            Character = "";
            Effects = new Effects(efek);
        }

        public bool update(string msg, string chara, Effects fx)
        {
            try
            {
                this.message = msg;
                this.character = chara;
                this.effects = fx;
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        public Line(string msg, string chara, Effects fx)
        {
            update(msg, chara, fx);
        }
        public void toJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName("message");
            writer.Write(this.Message);
            writer.WritePropertyName("character");
            writer.Write(this.Character);
            writer.WritePropertyName("effect");
            this.Effects.toJson(writer);
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

        public static Line parseJson_1_0(JsonData phaseLineJsonData, int lineIndex)
        {
            Line line = Line.parseJson_1_0(phaseLineJsonData,lineIndex,LineJsonKey.V_1_0);
            return line;
        }
        public static Line parseJson_1_0(JsonData phaseLineJsonData, int lineIndex, LineJsonKey lineKey)
        {
            Line line = new Line();
            line.message = phaseLineJsonData[lineKey.keys[0]][lineIndex].ToString();
            line.character = phaseLineJsonData[lineKey.keys[1]][lineIndex].ToString();
            line.effects = Effects.parseJson_1_0(phaseLineJsonData, lineIndex, lineKey.effectsKey);
            return line;
        }
    }
    public class LineJsonKey
    {
        public string[] keys = new string[] { "message", "character", "effects" };
        public EffectsJsonKey effectsKey = new EffectsJsonKey();
        public LineJsonKey(string[] newKeys, EffectsJsonKey newEffectsKey)
        {
            for (int i = 0; i < newKeys.Length; i++)
                if (!String.IsNullOrEmpty(newKeys[i]))
                    keys[i] = newKeys[i];
            if (newEffectsKey != null)
                effectsKey = newEffectsKey;
        }
        public LineJsonKey() { }
        public static LineJsonKey V_1_0
        {
            get { return new LineJsonKey(new string[] { "message", "character", "effects" }, EffectsJsonKey.V_1_0); }
        }
    }
}