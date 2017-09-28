using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LitJson;

namespace as3mbus.Story
{
    public enum fadeMode
    {
        none, color, transition
    }

    public class Phase
    {
        public static fadeMode parseFadeMode(string fM)
        {
            return
                fM.ToLower() == "transition" ? fadeMode.transition
                : fM.ToLower() == "color" ? fadeMode.color :
                fadeMode.none;
        }

        public string name;
        public Comic comic;
        public List<int> pages = new List<int>();
        public List<string> characters = new List<string>();
        public List<string> messages = new List<string>();
        public List<Vector3> paths = new List<Vector3>();
        public List<float> zooms = new List<float>();
        public List<float> shake = new List<float>();
        public List<Vector3> baloonpos = new List<Vector3>();
        public List<float> baloonsize = new List<float>();
        public List<fadeMode> fademode = new List<fadeMode>();
        public List<Color> bgcolor = new List<Color>();
        public Phase()
        {

        }
        public Phase(string name, string bundleName, string comicPath)
        {
            this.name = name;
            this.comic = new Comic(bundleName, comicPath);
        }
        public Phase(JsonData chara, JsonData msg)
        {
            for (int i = 0; i < chara.Count; i++)
            {
                this.characters.Add(chara[i].ToString());
                this.messages.Add(msg[i].ToString());
            }
        }

        public Phase parseJson(JsonData phaseData)
        {
            Phase fase = new Phase();
            try
            {
                fase.name = phaseData["name"].ToString();
                fase.comic = new Comic(phaseData["comic"]);
                for (int i = 0; i < phaseData["message"].Count; i++)
                {

                    fase.pages.Add((int)phaseData["page"][i]);
                    fase.paths.Add(
                        new Vector3(
                            float.Parse(phaseData["camx"][i].ToString()),
                            float.Parse(phaseData["camy"][i].ToString()),
                            -10f
                        )
                    );
                    fase.baloonpos.Add(
                        new Vector3(
                            float.Parse(phaseData["baloonx"][i].ToString()),
                            float.Parse(phaseData["baloony"][i].ToString()),
                            -1
                        )
                    );
                    fase.baloonsize.Add(float.Parse(phaseData["baloonsize"][i].ToString()));
                    Debug.Log("Testing " + paths[i].ToString());
                    fase.zooms.Add(float.Parse(phaseData["zoom"][i].ToString()));
                    fase.characters.Add(phaseData["character"][i].ToString());
                    fase.messages.Add(phaseData["message"][i].ToString());
                    switch (phaseData["fademode"][i].ToString())
                    {
                        case "transition":
                            fase.fademode.Add(fadeMode.transition);
                            break;
                        case "color":
                            fase.fademode.Add(fadeMode.color);
                            break;
                        default:
                            fase.fademode.Add(fadeMode.none);
                            break;
                    }
                }
            }
            catch (System.Exception)
            {
                for (int i = 0; i < phaseData["message"].Count; i++)
                {
                    fase.characters.Add(phaseData["character"][i].ToString());
                    fase.messages.Add(phaseData["message"][i].ToString());
                }
                throw;
            }
            return fase;

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

        public void toJson(JsonWriter writer)
        {

            writer.WriteObjectStart();
            writer.WritePropertyName("name");
            writer.Write(this.name);
            writer.WritePropertyName("comic");
            writer.Write(this.comic.toString());
            writer.WritePropertyName("page");
            writer.WriteArrayStart();
            foreach (var item in this.pages)
            {
                writer.Write(item);
            }
            writer.WriteArrayEnd();
            writer.WritePropertyName("character");
            writer.WriteArrayStart();
            foreach (var item in this.characters)
            {
                writer.Write(item);
            }
            writer.WriteArrayEnd();
            writer.WritePropertyName("message");
            writer.WriteArrayStart();
            foreach (var item in this.messages)
            {
                writer.Write(item);
            }
            writer.WriteArrayEnd();
            writer.WritePropertyName("camx");
            writer.WriteArrayStart();
            foreach (var item in this.paths)
            {
                writer.Write(item.x);
            }
            writer.WriteArrayEnd();
            writer.WritePropertyName("camy");
            writer.WriteArrayStart();
            foreach (var item in this.paths)
            {
                writer.Write(item.y);
            }
            writer.WriteArrayEnd();
            writer.WritePropertyName("zoom");
            writer.WriteArrayStart();
            foreach (var item in this.zooms)
            {
                writer.Write(item);
            }
            writer.WriteArrayEnd();
            writer.WritePropertyName("shake");
            writer.WriteArrayStart();
            foreach (var item in this.shake)
            {
                writer.Write(item);
            }
            writer.WriteArrayEnd();
            writer.WritePropertyName("baloonx");
            writer.WriteArrayStart();
            foreach (var item in this.baloonpos)
            {
                writer.Write(item.x);
            }
            writer.WriteArrayEnd();
            writer.WritePropertyName("baloony");
            writer.WriteArrayStart();
            foreach (var item in this.baloonpos)
            {
                writer.Write(item.y);
            }
            writer.WriteArrayEnd();
            writer.WritePropertyName("baloonsize");
            writer.WriteArrayStart();
            foreach (var item in this.baloonsize)
            {
                writer.Write(item);
            }
            writer.WriteArrayEnd();
            writer.WritePropertyName("fademode");
            writer.WriteArrayStart();
            foreach (var item in this.fademode)
            {
                writer.Write(item.ToString("g"));
            }
            writer.WriteArrayEnd();
            //writer.Write(string.Join("", new List<int>(array).ConvertAll(i => i.ToString()).ToArray()));
            // writer.WriteArrayEnd();

            writer.WriteObjectEnd();
        }

        public void newLine()
        {
            this.pages.Add(0);
            this.characters.Add("");
            this.messages.Add("");
            this.paths.Add(new Vector3(0, 0, -10));
            this.zooms.Add(5f);
            this.shake.Add(0);
            this.baloonpos.Add(new Vector3(0,0, 8));
            this.baloonsize.Add(1);
            this.fademode.Add(fadeMode.none);
            this.bgcolor.Add(Color.black);
        }

        public void deleteLine(int index)
        {
            
            this.pages.RemoveAt(index);
            this.characters.RemoveAt(index);
            this.messages.RemoveAt(index);
            this.paths.RemoveAt(index);
            this.zooms.RemoveAt(index);
            this.shake.RemoveAt(index);
            this.baloonpos.RemoveAt(index);
            this.baloonsize.RemoveAt(index);
            this.fademode.RemoveAt(index);
            this.bgcolor.RemoveAt(index);
        }

        public void UpdateLine(string character, string message, int pageNo, float zoom, Vector3 path, int index)
        {
            this.pages[index] = pageNo;
            this.zooms[index] = zoom;
            this.paths[index] = path;
            this.characters[index] = character;
            this.messages[index] = message;
            Debug.Log(this.toJson());
        }
        public void UpdateLine(
            string character,
            string message,
            int pageNo,
            float zoom,
            Vector3 path,
            float shake,
            Vector3 balooncor,
            float baloonsize,
            fadeMode fadeMode,
            Color bakgron,
            int index)
        {
            this.pages[index] = pageNo;
            this.characters[index] = character;
            this.messages[index] = message;
            this.zooms[index] = zoom;
            this.paths[index] = path;
            this.shake[index] = shake;
            this.baloonpos[index] = balooncor;
            this.baloonsize[index] = baloonsize;
            this.fademode[index] = fadeMode;
            this.bgcolor[index] = bakgron;
            Debug.Log(this.toJson());
        }

        public void insertLine(int index)
        {
            this.pages.Insert(index, 0);
            this.characters.Insert(index, "");
            this.messages.Insert(index, "");
            this.paths.Insert(index, new Vector3(0, 0, -10));
            this.zooms.Insert(index, 5f);
            this.shake.Insert(index, 0);
            this.baloonpos.Insert(index, new Vector3(0,0, 8));
            this.baloonsize.Insert(index,1);
            this.fademode.Insert(index,fadeMode.none);
            this.bgcolor.Insert(index, Color.black);
        }
    }
}