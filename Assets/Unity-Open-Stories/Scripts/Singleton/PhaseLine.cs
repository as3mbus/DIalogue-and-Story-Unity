using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LitJson;

namespace as3mbus.Story
{
    public class PhaseLine
    {
        Effects effects;
        string message;
        string character;

        public string Character { get { return character; } set { character = value; } }
        public string Message { get { return message; } set { message = value; } }
        public Effects Effects { get { return effects; } set { effects = value; } }

        public PhaseLine()
        {
            Message = "";
            Character = "";
            Effects = new Effects();
        }
        public PhaseLine(Effects efek)
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
        public PhaseLine(string msg, string chara, Effects fx)
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
    }
}