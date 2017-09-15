﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LitJson;

public class StoryCreatorControl : MonoBehaviour
{

    static Story targetStory;
    public GameObject phasePanel, phaseScrollView, dialogueCreator, phaseButton, newPhaseButton;
    public DialogueCreatorControl dialogControl;
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
        targetStory.phases.Add(new Dialogue(phasePanel.GetComponentInChildren<InputField>().text, comicDropdown.GetComponentInChildren<Dropdown>().captionText.text));
        typeWindowActive(false);
        newContentButton();

    }
    public void newComic()
    {
        targetStory.phases.Add(new Comic(phasePanel.GetComponentInChildren<InputField>().text, comicDropdown.GetComponentInChildren<Dropdown>().captionText.text));
        typeWindowActive(false);
        newContentButton();
    }
    public void cancelPhase()
    {
        typeWindowActive(false);
    }
    void typeWindowActive(bool mode)
    {
        phasePanel.SetActive(mode);
        var storyButtons = GetComponentsInChildren<Button>();
        GetComponentInChildren<InputField>().interactable = !mode;
        foreach (var button in storyButtons)
        {
            button.interactable = !mode;
        }
    }
    void loadComics()
    {
        JsonData jsonComic;
        jsonComic = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/comic.json"));
        foreach (JsonData comic in jsonComic["comic"])
        {
            comicDropdown.options.Add(new Dropdown.OptionData(comic["name"].ToString()));
        }
        // var comicPath = "jar:file://" + Application.dataPath + "!/assets/Comic/";
        // var comicDirectories = new DirectoryInfo(comicPath).GetDirectories();
        // foreach (DirectoryInfo dir in comicDirectories) comicDropdown.options.Add(new Dropdown.OptionData(dir.Name));
    }
    public static string listComicJson()
    {
        var comicPath = Application.streamingAssetsPath + "/Comic/";
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
                if (file.Extension == ".png")
                    writer.Write(Path.GetFileNameWithoutExtension(file.Name));
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
        writer.WriteArrayEnd();
        writer.WriteObjectEnd();
        return sb.ToString();
    }
    public static void writeComicJson()
    {
        var sr = File.CreateText(Application.streamingAssetsPath + "/comic.json");
        sr.Write(listComicJson());
        sr.Close();
    }
    void contentResize()
    {
        Vector2 contentSize = phaseScrollView.GetComponent<ScrollRect>().content.sizeDelta;
        contentSize.y = 250 * (targetStory.phases.Count + 1) + 50;
        phaseScrollView.GetComponent<ScrollRect>().content.sizeDelta = contentSize;
    }
    void newContentButton()
    {
        GameObject newButton = Object.Instantiate(phaseButton, phaseScrollView.GetComponent<ScrollRect>().content);
        Vector3 newpos = newButton.GetComponent<RectTransform>().localPosition;
        newpos.y -= 250 * (targetStory.phases.Count - 1);
        newButton.GetComponent<RectTransform>().localPosition = newpos;
        newButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => editPhase());
        contentButtonUpdate(newButton);
        newpos.y -= 250;
        newPhaseButton.GetComponent<RectTransform>().localPosition = newpos;
        contentResize();
    }
    void contentButtonUpdate(GameObject button)
    {
        int index = button.transform.GetSiblingIndex() - 1;
        if (targetStory.phases[index].GetType().Equals(new Dialogue().GetType()))
        {
            Dialogue dialog = (Dialogue)targetStory.phases[index];
            button.transform.GetChild(0).Find("Name").GetComponent<Text>().text = dialog.name;
            button.transform.GetChild(0).Find("Type").GetComponent<Text>().text = "D";
            button.transform.GetChild(0).Find("BG").GetComponent<Text>().text = dialog.comic.toString();
            button.transform.GetChild(0).Find("Line").GetComponent<Text>().text = dialog.messages.Count.ToString() + " Line";
        }
        else
        {
            Comic komik = (Comic)targetStory.phases[index];
            button.transform.GetChild(0).Find("Name").GetComponent<Text>().text = komik.name;
            button.transform.GetChild(0).Find("Type").GetComponent<Text>().text = "C";
            button.transform.GetChild(0).Find("BG").GetComponent<Text>().text = komik.toString();
            button.transform.GetChild(0).Find("Line").GetComponent<Text>().text = "";
        }
    }
    public void contentDialogueButtonUpdate(Dialogue dialog)
    {
        int index = targetStory.phases.IndexOf(dialog) + 1;
        GameObject button = phaseScrollView.GetComponent<ScrollRect>().content.GetChild(index).gameObject;

        button.transform.GetChild(0).Find("Name").GetComponent<Text>().text = dialog.name;
        button.transform.GetChild(0).Find("Type").GetComponent<Text>().text = "D";
        button.transform.GetChild(0).Find("BG").GetComponent<Text>().text = dialog.comic.toString();
        button.transform.GetChild(0).Find("Line").GetComponent<Text>().text = dialog.messages.Count.ToString() + " Line";

    }
    public void editPhase()
    {
        int phaseIndex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex() - 1;
        if (targetStory.phases[phaseIndex].GetType().Equals(new Dialogue().GetType()))
        {
            Dialogue dialog = (Dialogue)targetStory.phases[phaseIndex];

            editDialogue(dialog);
        }
    }
    public void editDialogue(Dialogue dialog)
    {
        dialogControl.loadDialogue(dialog);

        dialogControl.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

}