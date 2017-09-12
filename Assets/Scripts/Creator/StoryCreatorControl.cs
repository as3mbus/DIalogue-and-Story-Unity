using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        var info = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory() + "\\Assets\\Resources\\Comic");
        // print(info);
        var fileInfo = info.GetDirectories();
        // storyNameField.text = "Success";
        foreach (DirectoryInfo dir in fileInfo) comicDropdown.options.Add(new Dropdown.OptionData(dir.Name));
    }
	
}
