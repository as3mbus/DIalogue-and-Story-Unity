using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using LitJson;

namespace as3mbus.Story
{


    public class Phase
    {
        public List<PhaseLine> Lines;
        public string name;
        public Comic comic;

        public Phase()
        {
            Lines = new List<PhaseLine>();
        }

        //new empty phase with loaded comic 
        public Phase(string name, string bundleName, string comicPath)
        {
            this.name = name;
            this.comic = new Comic(bundleName, comicPath);
            this.Lines = new List<PhaseLine>();
        }

        //parse old format json
        public static Phase parseOldJson(JsonData phaseData)
        {
            Phase fase = new Phase();
            fase.name = phaseData["name"].ToString();
            fase.comic = new Comic(phaseData["comicsource"].ToString(), phaseData["comicname"].ToString());
            for (int i = 0; i < phaseData["message"].Count; i++)
            {
                fase.Lines.Add(
                    new PhaseLine(
                        phaseData["message"][i].ToString(),
                        phaseData["character"][i].ToString(),
                        new Effects(
                            (int)phaseData["page"][i],
                            float.Parse(phaseData["duration"][i].ToString()),
                            Effects.parseFadeMode(phaseData["fademode"][i].ToString()),
                            new CamEffects(
                                new Vector3(
                                    float.Parse(phaseData["camx"][i].ToString()),
                                    float.Parse(phaseData["camy"][i].ToString()),
                                    -10f
                                ),
                                float.Parse(phaseData["zoom"][i].ToString()),
                                float.Parse(phaseData["shake"][i].ToString()),
                                Effects.parseColorFromString(phaseData["background"][i].ToString())
                            )
                        )
                    )
                );
                Debug.Log(fase.Lines[i].toJson());
            }
            return fase;
        }
        public static Phase parseJson(JsonData phaseData)
        {
            Phase fase = new Phase();
            JsonData contentjson;
            fase.name = phaseData["name"].ToString();
            fase.comic = new Comic(phaseData["comicsource"].ToString(), phaseData["comicname"].ToString());
            for (int i = 0; i < phaseData["content"].Count; i++)
            {
                contentjson = phaseData["content"][i];
                Debug.Log(contentjson["effect"]["camera"]["shake"].ToString());
                fase.Lines.Add(
                    new PhaseLine(
                        contentjson["message"].ToString(),
                        contentjson["character"].ToString(),
                        new Effects(
                            (int)contentjson["effect"]["page"],
                            float.Parse(contentjson["effect"]["duration"].ToString()),
                            Effects.parseFadeMode(contentjson["effect"]["fademode"].ToString()),
                                new CamEffects(
                                    new Vector3(
                                        float.Parse(contentjson["effect"]["camera"]["x"].ToString()),
                                        float.Parse(contentjson["effect"]["camera"]["y"].ToString()),
                                        -10f
                                    ),
                                    float.Parse(contentjson["effect"]["camera"]["size"].ToString()),
                                    float.Parse(contentjson["effect"]["camera"]["shake"].ToString()),
                                    Effects.parseColorFromString(contentjson["effect"]["camera"]["backgroundcolor"].ToString())
                                )
                        )
                    )
                );
            }
            return fase;
        }
        //create json date (string) based on a phase content
        public string toJson()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.IndentValue = 4;
            toJson(writer);
            return sb.ToString();
        }
        //writer json data using json writer 
        public void toJson(JsonWriter writer)
        {

            writer.WriteObjectStart();
            writer.WritePropertyName("name");
            writer.Write(this.name);
            writer.WritePropertyName("comicname");
            writer.Write(this.comic.name);
            writer.WritePropertyName("comicsource");
            writer.Write(this.comic.source);
            writer.WritePropertyName("content");
            writer.WriteArrayStart();
            foreach (PhaseLine lines in Lines)
            {
                lines.toJson(writer);
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();

        }

        public void newLine()
        {
            Lines.Add(new PhaseLine());
        }
        public void newLine(Effects efek)
        {
            Lines.Add(new PhaseLine(efek));
        }
        
        //delete a line on certain index
        public void deleteLine(int index)
        {
            this.Lines.RemoveAt(index);
        }
        //update line
        public void UpdateLine(
            string character,
            string message,
            int index
            )
        {
            this.Lines[index].update(message, character, this.Lines[index].Effects);
        }
        public void UpdateEffect(
            int pageNo,
            float duration,
            fadeMode fadeMode,
            int index
            )
        {
            this.Lines[index].Effects.update(pageNo, duration, fadeMode, this.Lines[index].Effects.CameraEffects);
        }
        public void UpdateCameraEffect(
            Vector3 camPos,
            float camsz,
            float camshake,
            Color bgcolor,
            int index
            )
        {
            this.Lines[index].Effects.CameraEffects.update(camPos, camsz, camshake, bgcolor);
        }
        public void UpdateAll(
            string character,
            string message,
            int pageNo,
            float duration,
            fadeMode fadeMode,
            float camsz,
            Vector3 camPos,
            float camshake,
            Color bgcolor,
            int index)
        {
            UpdateCameraEffect(camPos, camsz, camshake, bgcolor, index);
            UpdateEffect(pageNo, duration, fadeMode, index);
            UpdateLine(character, message, index);
        }
        //insert new line at certain index 
        public void insertLine(int index)
        {
            this.Lines.Insert(index, new PhaseLine());
        }
        //with this effects
        public void insertLine(int index, Effects efek)
        {
            this.Lines.Insert(index, new PhaseLine(efek));
        }

        //get all character inside a phase
        public string[] getCharacters()
        {
            string[] characters = new string[this.Lines.Count];
            for(int i = 0; i<this.Lines.Count; i++){
                characters[i] = this.Lines[i].Character;
            }
            return characters.Distinct().ToArray();
        }

    }
}