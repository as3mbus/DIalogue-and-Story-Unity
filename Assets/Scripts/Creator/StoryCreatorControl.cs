using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class StoryCreatorControl : MonoBehaviour
{

    Story targetStory;
    public GameObject typeCanvas, phaseScrollView;
    public InputField storyNameField;
    public Dropdown comicDropdown;

    // Use this for initialization
    void Start()
    {
        targetStory = new Story();
        loadComics();
        print(listComicJson());
        writeComicJson();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void newPhase()
    {
        typeWindowActive(true);


    }
    public void newDialogue()
    {
        targetStory.phase.Add(new Dialogue());
        typeWindowActive(false);

    }
    public void newComic()
    {
        typeWindowActive(false);
    }
    public void cancelPhase()
    {
        typeWindowActive(false);
    }
    void typeWindowActive(bool mode)
    {
        typeCanvas.SetActive(mode);
        foreach (Transform child in phaseScrollView.transform.Find("Viewport").transform.Find("Content").transform)
        {
            if (child.name.Contains("Content"))
            {
                child.GetChild(0).GetComponent<Button>().interactable = !mode;
                child.GetChild(1).GetComponent<Button>().interactable = !mode;
            }
            else
            {
                child.GetComponent<Button>().interactable = !mode;
            }
        }
    }
    void loadComics()
    {
        JsonData jsonComic;
        jsonComic = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath+"/comic.json"));
        foreach (JsonData comic in jsonComic["comic"])
        {
            comicDropdown.options.Add(new Dropdown.OptionData(comic["name"].ToString()));
        }
        // var comicPath = "jar:file://" + Application.dataPath + "!/assets/Comic/";
        // var comicDirectories = new DirectoryInfo(comicPath).GetDirectories();
        // foreach (DirectoryInfo dir in comicDirectories) comicDropdown.options.Add(new Dropdown.OptionData(dir.Name));
    }
    string listComicJson()
    {
        var comicPath = Application.streamingAssetsPath+"/Comic/";
        var comicDirectories = new DirectoryInfo(comicPath).GetDirectories();
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = true;
        writer.IndentValue = 4;

        writer.WriteObjectStart();
        writer.WritePropertyName("comic");
        writer.WriteArrayStart();
        foreach (DirectoryInfo dir in comicDirectories)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName("name");
            writer.Write(dir.Name);
            writer.WritePropertyName("content");
            writer.WriteArrayStart();
            foreach (FileInfo file in dir.GetFiles())
            {
                if(file.Extension==".png")
                    writer.Write(Path.GetFileNameWithoutExtension(file.Name));
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
        writer.WriteArrayEnd();
        writer.WriteObjectEnd();

        return sb.ToString();
    }
    void writeComicJson(){
        var sr = File.CreateText(Application.streamingAssetsPath+"/comic.json");
        sr.Write(listComicJson());
        sr.Close();
    }
}
