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
        public bool fullyLoaded
        {
            get { return comicLoaded && bgmLoaded; }
        }
        public bool comicLoaded
        {
            get { return useComicLoaded || comic.loaded; }
        }
        public bool useComicLoaded = false, bgmLoaded = false;
        public List<Line> Lines;
        public string name;
        public Comic comic;
        public string bgmFileName = "", bgmAssetBundleName = "";
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

        // parse json using latest vesion phase json key
        public static Phase parseJson(JsonData phaseJsonData)
        {
            return parseJson(phaseJsonData, StoryJsonKey.Latest.Phase);
        }
        // parse json using specified version of json key
        public static Phase parseJson(JsonData phaseJsonData, JsonKey phaseKey)
        {
            Phase fase = new Phase();
            fase.name = phaseJsonData[phaseKey.elementsKeys[0]].ToString();
            if (phaseJsonData.Keys.Contains(phaseKey.elementsjsonKey[0].objectName))
                fase.parseLines_1_1(phaseJsonData, phaseKey);
            else
            {
                if (phaseKey.equal(StoryJsonKey.V_1_1.Phase))
                    fase.parseLines_1_0(phaseJsonData);
                else
                    fase.parseLines_1_0(phaseJsonData, phaseKey.elementsjsonKey[0]);
            }
            fase.comic = new Comic(phaseJsonData[phaseKey.elementsKeys[1]].ToString(), phaseJsonData[phaseKey.elementsKeys[2]].ToString());
            if (phaseJsonData.Keys.Contains(phaseKey.elementsKeys[3]))
            {
                fase.bgmAssetBundleName = phaseJsonData[phaseKey.elementsKeys[3]].ToString();
                fase.bgmFileName = phaseJsonData[phaseKey.elementsKeys[4]].ToString();
            }
            return fase;
        }

        // parse lines of the phase using v1.0 method and v1.0 Line json key
        public void parseLines_1_0(JsonData phaseJsonData)
        {
            // handling comic inside the same bundle as story
            parseLines_1_0(phaseJsonData, StoryJsonKey.V_1_0.Line);
        }
        // parse lines of the phase using v1.0 method and specified Line json key
        public void parseLines_1_0(JsonData phaseJsonData, JsonKey lineKey)
        {
            for (int lineIndex = 0; lineIndex < phaseJsonData[lineKey.elementsKeys[0]].Count; lineIndex++)
            {
                this.Lines.Add(
                    Line.parseJson_1_0(phaseJsonData, lineIndex, lineKey)
                );
                // Debug.Log(fase.Lines[i].toJson());
            }
        }

        // parse lines of a phase using v1.1 method and latest lines json key 
        public void parseLines_1_1(JsonData phaseJsonData)
        {
            // handling comic inside the same bundle as story
            parseLines_1_1(phaseJsonData, StoryJsonKey.Latest.Phase);
        }

        // parse lines of the phase using v1.0 method and v1.0 Line json key
        public void parseLines_1_1(JsonData phaseData, JsonKey LineKey)
        {
            JsonData contentjson;
            // handling comic inside the same bundle as story
            for (int i = 0; i < phaseData[LineKey.elementsjsonKey[0].objectName].Count; i++)
            {
                contentjson = phaseData[LineKey.elementsjsonKey[0].objectName][i];
                // Debug.Log(contentjson["effect"]["camera"]["shake"].ToString());
                this.Lines.Add(
                    Line.parseJson_1_1(contentjson, LineKey.elementsjsonKey[0])
                );
            }
        }
        public void loadResources()
        {
            if (!comicLoaded)
            {
                Debug.Log("comuc should be loaded");
                loadComic();
            }
            if (!bgmLoaded)
                loadBGM();
        }

        // load comic from bundle that mentioned in json data (data in this case)
        public void loadComic()
        {
            if (!useComicLoaded)
            {
                this.comic.loadPages(this.getPages());
                this.useComicLoaded = true;
            }
        }

        // load bgm from bundle that mentioned in json data (data in this case)
        public void loadBGM()
        {
            if (!bgmLoaded)
                if (!String.IsNullOrEmpty(this.bgmAssetBundleName))
                {
                    AssetBundle bgmBundle = DataManager.readAssetBundles(DataManager.bundlePath(this.bgmAssetBundleName));
                    this.bgm = bgmBundle.LoadAsset<AudioClip>(this.bgmFileName);
                    bgmBundle.Unload(false);
                }
            this.bgmLoaded = true;
        }


        //create json date (string) based on a phase content
        public string toJson()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.IndentValue = 4;
            writeJson(writer);
            return sb.ToString();
        }
        // write json string using latest phase json key 
        public void writeJson(JsonWriter writer)
        {
            writeJson(writer, StoryJsonKey.Latest.Phase);
        }
        // write json string using specified phase json key 
        public void writeJson(JsonWriter writer, JsonKey phaseKey)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName(phaseKey.elementsKeys[0]);
            writer.Write(this.name);
            writer.WritePropertyName(phaseKey.elementsKeys[1]);
            writer.Write(this.comic.bundleName);
            writer.WritePropertyName(phaseKey.elementsKeys[2]);
            writer.Write(this.comic.comicDirectory);
            writer.WritePropertyName(phaseKey.elementsKeys[3]);
            writer.Write(this.bgmAssetBundleName);
            writer.WritePropertyName(phaseKey.elementsKeys[4]);
            writer.Write(this.bgmFileName);
            writer.WritePropertyName(phaseKey.elementsjsonKey[0].objectName);
            writer.WriteArrayStart();
            foreach (Line lines in Lines)
            {
                lines.writeJson(writer, phaseKey.elementsjsonKey[0]);
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        // new empty line
        public void newLine()
        {
            Lines.Add(new Line());
        }
        // new line with old effect assigned
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
        // get all needed pages index inside a phase
        public int[] getPages()
        {
            int[] pages = new int[this.Lines.Count];
            for (int i = 0; i < this.Lines.Count; i++)
                pages[i] = this.Lines[i].Effects.Page;
            return pages.Distinct().ToArray();
        }

    }
}