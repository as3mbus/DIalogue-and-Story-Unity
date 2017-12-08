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
        // create default line
        public Line()
        {
            Message = "";
            Character = "";
            Effects = new Effects();
        }
        // create empty line with specified effects
        public Line(Effects efek)
        {
            Message = "";
            Character = "";
            Effects = new Effects(efek);
        }
        // set value
        public bool setValue(string msg, string chara, Effects fx)
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
            setValue(msg, chara, fx);
        }
        // write json string with specified version of json code key
        public void writeJson(JsonWriter writer, JsonKey lineKey)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName(lineKey.elementsKeys[0]);
            writer.Write(this.Message);
            writer.WritePropertyName(lineKey.elementsKeys[1]);
            writer.Write(this.Character);
            writer.WritePropertyName(lineKey.elementsjsonKey[0].objectName);
            Debug.Log(lineKey.elementsjsonKey[0].objectName);
            this.Effects.writeJson(writer, lineKey.elementsjsonKey[0]);
            writer.WriteObjectEnd();

        }
        // return json string writed with latest version of json code key
        public string toJson()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.IndentValue = 4;
            writeJson(writer, StoryJsonKey.Latest.Line);
            return sb.ToString();
        }

        // parse json string using v1.0 method and vspecified json keycode
        public static Line parseJson_1_0(JsonData phaseLineJsonData, int lineIndex, JsonKey lineKey)
        {
            Line line = new Line();
            line.message = phaseLineJsonData[lineKey.elementsKeys[0]][lineIndex].ToString();
            line.character = phaseLineJsonData[lineKey.elementsKeys[1]][lineIndex].ToString();
            line.effects = Effects.parseJson_1_0(phaseLineJsonData, lineIndex, lineKey.elementsjsonKey[0]);
            return line;
        }

        // parse json string using v1.1 method and specified json keycode
        public static Line parseJson_1_1(JsonData phaseLineJsonData, JsonKey lineKey)
        {
            Line line = new Line();
            line.message = phaseLineJsonData[lineKey.elementsKeys[0]].ToString();
            line.character = phaseLineJsonData[lineKey.elementsKeys[1]].ToString();
            line.effects = Effects.parseJson_1_1(phaseLineJsonData[lineKey.elementsjsonKey[0].objectName], lineKey.elementsjsonKey[0]);
            return line;
        }
    }
}