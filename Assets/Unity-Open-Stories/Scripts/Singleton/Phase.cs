using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace as3mbus.Story
{


    public class Phase
    {
        public List<Line> Lines;
        public string name;
        public Comic comic;
        public string bgmFileName = "", bgmAssetBundle = "";
        public AudioClip bgm;

        public Phase()
        {
            Lines = new List<Line>();
        }

        //new empty phase with loaded comic 
        public Phase(string name, string bundleName, string comicPath)
        {
            this.name = name;
            this.comic = new Comic(bundleName, comicPath);
            this.Lines = new List<Line>();
        }

        //parse phase json for v1 format
        public static Phase parseJson_1_0(JsonData phaseJsonData, AssetBundle storyAssetBundle)
        {
            Phase fase = new Phase();
            fase.name = phaseJsonData["name"].ToString();
            // handling comic inside the same bundle as story
            for (int lineIndex = 0; lineIndex < phaseJsonData["message"].Count; lineIndex++)
            {
                fase.Lines.Add(
                    Line.parseJson_1_0(phaseJsonData, lineIndex)
                );
                // Debug.Log(fase.Lines[i].toJson());
            }
            loadComic(storyAssetBundle, phaseJsonData["comicsource"].ToString(), phaseJsonData["comicname"].ToString(), fase);
            return fase;
        }

        //parse phase json for v1.1 format
        public static Phase parseJson_1_1(JsonData phaseData, AssetBundle storyBundle)
        {
            Phase fase = new Phase();
            JsonData contentjson;
            fase.name = phaseData["name"].ToString();
            // handling comic inside the same bundle as story
            for (int i = 0; i < phaseData["content"].Count; i++)
            {
                contentjson = phaseData["content"][i];
                // Debug.Log(contentjson["effect"]["camera"]["shake"].ToString());
                fase.Lines.Add(
                    new Line(
                        contentjson["message"].ToString(),
                        contentjson["character"].ToString(),
                        new Effects(
                            (int)contentjson["effect"]["page"],
                            float.Parse(contentjson["effect"]["duration"].ToString()),
                            Effects.parseFadeMode(contentjson["effect"]["fademode"].ToString()),
                            new CameraEffects(
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
            loadComic(storyBundle, phaseData["comicsource"].ToString(), phaseData["comicname"].ToString(), fase);
            return fase;
        }

        public static Phase parseJson_1_2(JsonData phaseData, AssetBundle storyBundle)
        {
            Phase fase = new Phase();
            JsonData contentjson;
            fase.name = phaseData["name"].ToString();
            fase.bgmAssetBundle = phaseData["bgmAssetBundleName"].ToString();
            fase.bgmFileName = phaseData["bgmFileName"].ToString();
            AssetBundle bgmBundle = DataManager.readAssetBundles(DataManager.bundlePath(phaseData["bgmAssetBundleName"].ToString()));
            fase.bgm = bgmBundle.LoadAsset<AudioClip>(phaseData["bgmFileName"].ToString());
            bgmBundle.Unload(false);
            // handling comic inside the same bundle as story
            for (int i = 0; i < phaseData["content"].Count; i++)
            {
                contentjson = phaseData["content"][i];
                // Debug.Log(contentjson["effect"]["camera"]["shake"].ToString());
                fase.Lines.Add(
                    new Line(
                        contentjson["message"].ToString(),
                        contentjson["character"].ToString(),
                        new Effects(
                            (int)contentjson["effect"]["page"],
                            float.Parse(contentjson["effect"]["duration"].ToString()),
                            Effects.parseFadeMode(contentjson["effect"]["fadeMode"].ToString()),
                            new CameraEffects(
                                new Vector3(
                                    float.Parse(contentjson["effect"]["camera"]["x"].ToString()),
                                    float.Parse(contentjson["effect"]["camera"]["y"].ToString()),
                                    -10f
                                ),
                                float.Parse(contentjson["effect"]["camera"]["size"].ToString()),
                                float.Parse(contentjson["effect"]["camera"]["shake"].ToString()),
                                Effects.parseColorFromString(contentjson["effect"]["camera"]["backgroundColor"].ToString())
                            )
                        )
                    )
                );
            }
            loadComic(storyBundle, phaseData["comicAssetBundleName"].ToString(), phaseData["comicDirectoryName"].ToString(), fase);
            return fase;
        }

        public static void loadComic(AssetBundle storyBundle, string comicAssetBundleName, string comicDirectoryName, Phase fase)
        {
            if (DataManager.isSameBundle(storyBundle, comicAssetBundleName))
                fase.comic = new Comic(storyBundle, comicDirectoryName, fase.getPages());
            else
                fase.comic = new Comic(comicAssetBundleName.ToString(), comicDirectoryName);
        }
        public static void loadBGM(AssetBundle storyBundle, string bgmAssetBundleName, string bgmFileName, Phase fase)
        {
            if (DataManager.isSameBundle(storyBundle, bgmAssetBundleName))
            {
                AssetBundle bgmBundle = storyBundle;
                fase.bgm = bgmBundle.LoadAsset<AudioClip>(bgmFileName);
            }
            else
            {
                AssetBundle bgmBundle = DataManager.readAssetBundles(DataManager.bundlePath(bgmAssetBundleName));
                fase.bgm = bgmBundle.LoadAsset<AudioClip>(bgmFileName);
                bgmBundle.Unload(false);
            }
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
            writer.WritePropertyName("comicAssetBundleName");
            writer.Write(this.comic.source);
            writer.WritePropertyName("comicDirectoryName");
            writer.Write(this.comic.name);
            writer.WritePropertyName("bgmAssetBundleName");
            writer.Write(this.bgmAssetBundle);
            writer.WritePropertyName("bgmFileName");
            writer.Write(this.bgmFileName);
            writer.WritePropertyName("content");
            writer.WriteArrayStart();
            foreach (Line lines in Lines)
            {
                lines.toJson(writer);
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public void newLine()
        {
            Lines.Add(new Line());
        }
        public void newLine(Effects efek)
        {
            Lines.Add(new Line(efek));
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
            this.Lines.Insert(index, new Line());
        }
        //with this effects
        public void insertLine(int index, Effects efek)
        {
            this.Lines.Insert(index, new Line(efek));
        }

        //get all character inside a phase
        public string[] getCharacters()
        {
            string[] characters = new string[this.Lines.Count];
            for (int i = 0; i < this.Lines.Count; i++)
                characters[i] = this.Lines[i].Character;
            return characters.Distinct().ToArray();
        }
        public int[] getPages()
        {
            int[] pages = new int[this.Lines.Count];
            for (int i = 0; i < this.Lines.Count; i++)
                pages[i] = this.Lines[i].Effects.Page;
            return pages.Distinct().ToArray();
        }

    }
    public class PhaseJsonKey
    {
        public string[] keys = new string[] { "name", "comicAssetBundleName", "comicDirectoryName", "bgmAssetBundleName", "bgmFileName", "content" };
        public LineJsonKey lineKey = new LineJsonKey();
        public PhaseJsonKey(string[] newKeys, LineJsonKey newLineKey)
        {
            for (int i = 0; i < newKeys.Length; i++)
                if (!String.IsNullOrEmpty(newKeys[i]))
                    keys[i] = newKeys[i];
            if (newLineKey != null)
                this.lineKey = newLineKey;
        }
        public PhaseJsonKey() { }
        public static PhaseJsonKey V_1_0
        {
            get { return new PhaseJsonKey(new string[] { "name", "comicsource", "comicname", "", "", "content" }, LineJsonKey.V_1_0); }
        }
    }
}