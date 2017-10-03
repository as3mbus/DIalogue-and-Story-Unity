using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using as3mbus.Story;
using GracesGames;

public class StoryCreatorControl : MonoBehaviour
{
    Story targetStory;
    public GameObject phasePanel, phaseButton, newPhaseButton;
    public ScrollRect phaseScrollView;
    public PhaseCreator phaseCreator;
    public InputField storyNameField, phaseNameField;
    public Dropdown comicDropdown, bundleDropdown;
    public GameObject FileBrowserPrefab;
    string[] activeComics;

    // Use this for initialization

    void Start()
    {
        targetStory = new Story();
        // print(Comic.listComicsJson());
        // Comic.writeComicsJson();
        // loadComics();
        resetAddPhaseButton();
        ComicManager.listStreamingComicsBundleJson(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        ComicManager.readComicsBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        addDropdownOptions(bundleDropdown, ComicManager.streamBundleList.ToArray());
        bundleDropdown.value = 0;
        bundleDropdown.captionText.text = bundleDropdown.options[0].text;
    }

    /* ====END OF===== */
    /* =MAIN FUNCTION= */
    /* =============== */

    public void newPhases()
    {
        typeWindowActive(true);
    }
    public void createPhase()
    {
        targetStory.phases.Add(new Phase(phaseNameField.text, bundleDropdown.captionText.text, comicDropdown.captionText.text));
        typeWindowActive(false);
        newContentButton();
    }
    public void cancelPhase()
    {
        typeWindowActive(false);
    }
    public void contentButtonUpdate(GameObject button)
    {
        int index = button.transform.GetSiblingIndex();
        print(phaseScrollView.content.transform.childCount);

        Phase fase = (Phase)targetStory.phases[index];
        contentButtonUpdate(fase, button);
    }
    public void contentButtonUpdate(Phase fase)
    {
        int index = targetStory.phases.IndexOf(fase);
        GameObject button = phaseScrollView.content.GetChild(index).gameObject;
        contentButtonUpdate(fase, button);
    }
    public void contentButtonUpdate(Phase fase, GameObject button)
    {
        button.transform.GetChild(0).Find("Name").GetComponent<Text>().text = fase.name;
        button.transform.GetChild(0).Find("Type").GetComponent<Text>().text = "";
        button.transform.GetChild(0).Find("BG").GetComponent<Text>().text = fase.comic.toString();
        button.transform.GetChild(0).Find("Line").GetComponent<Text>().text = fase.messages.Count.ToString() + " Line";
    }
    public void editPhase()
    {
        int phaseIndex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex();
        Phase fase = (Phase)targetStory.phases[phaseIndex];
        editPhase(fase);
    }
    public void deletePhase()
    {
        int phaseIndex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex();
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        Vector3 newpos = newPhaseButton.GetComponent<RectTransform>().localPosition;
        newpos.y += 250;
        newPhaseButton.GetComponent<RectTransform>().localPosition = newpos;
        for (int i = phaseIndex + 1; i < phaseScrollView.content.childCount; i++)
        {
            var newButton = phaseScrollView.content.GetChild(i);
            newpos = newButton.GetComponent<RectTransform>().localPosition;
            newpos.y += 250;
            newButton.GetComponent<RectTransform>().localPosition = newpos;

        }

        targetStory.phases.RemoveAt(phaseIndex);
        // contentResize();

    }
    public void editPhase(Phase fase)
    {
        phaseCreator.gameObject.SetActive(true);
        phaseCreator.loadPhase(fase);
        gameObject.SetActive(false);
    }
    public void playScene()
    {
        targetStory.name = storyNameField.text;
        StoryManager.storyType = storyDataType.String;
        StoryManager.stringOrPath = targetStory.toJson();
        SceneManager.LoadScene("Player");
    }

    void saveStory(string path)
    {
        targetStory.name = storyNameField.text;
        var sr = File.CreateText(path);
        sr.Write(targetStory.toJson());
        sr.Close();
    }
    public void OpenFileBrowser(bool save)
    {
        if (save) OpenFileBrowser(FileBrowserMode.Save);
        else OpenFileBrowser(FileBrowserMode.Load);
    }
    public void OpenFileBrowser(FileBrowserMode fileBrowserMode)
    {
        // Create the file browser and name it
        GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, this.transform);
        fileBrowserObject.name = "FileBrowser";
        // Set the mode to save or load
        FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        if (fileBrowserMode == FileBrowserMode.Save)
            fileBrowserScript.SaveFilePanel(this, "saveStory", storyNameField.text, "json");
        else
            fileBrowserScript.OpenFilePanel(this, "loadStory", "json");
    }
    public void loadStory(string path)
    {
        resetPhase();
        targetStory = new Story(path);
        print(targetStory.toJson());
        storyNameField.text = targetStory.name;
        foreach (var item in targetStory.phases)
        {
            newContentButton();
        }

    }

    /* ====END OF===== */
    /* =Main Function= */
    /* =============== */

    /* =============== */
    /* =LOOK AND FEEL= */
    /* =============== */
    public void bundleChange()
    {
        comicDropdown.options.Clear();
        if (ComicManager.streamContent(bundleDropdown.captionText.text))
            activeComics = ComicManager.getComics(ComicManager.readStreamBundles(Path.Combine(Application.streamingAssetsPath, bundleDropdown.captionText.text)));
        else
            activeComics = ComicManager.getComics(ComicManager.readStreamBundles(Path.Combine(Application.persistentDataPath, bundleDropdown.captionText.text)));
        addDropdownOptions(comicDropdown, activeComics);
        comicDropdown.value = 0;
        comicDropdown.captionText.text = comicDropdown.options[0].text;
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
    void newContentButton()
    {
        GameObject newButton = Object.Instantiate(phaseButton, phaseScrollView.content);
        newButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => editPhase());
        newButton.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => deletePhase());
        resetAddPhaseButton();
        contentButtonUpdate(newButton);
        // contentResize();
    }
    void resetAddPhaseButton()
    {
        newPhaseButton.transform.SetAsLastSibling();
    }

    public void resetPhase()
    {
        var children = new List<GameObject>();
        foreach (Transform child in phaseScrollView.content.transform) children.Add(child.gameObject);
        children.ForEach(child =>
        {
            if (!child.name.Contains("Add")) Destroy(child);
        });
        targetStory.phases.Clear();
        resetAddPhaseButton();
    }
    /* =====EMD OF==== */
    /* =LOOK AND FEEL= */
    /* =============== */

    /* =============== */
    /* ==STATIC-ABLE== */
    /* =============== */

    void addDropdownOptions(Dropdown DD, string[] optionsArray)
    {
        foreach (string comic in optionsArray)
        {
            DD.options.Add(new Dropdown.OptionData(Path.GetFileName(comic)));
        }
        // var comicPath = "jar:file://" + Application.dataPath + "!/assets/Comic/";
        // var comicDirectories = new DirectoryInfo(comicPath).GetDirectories();
        // foreach (DirectoryInfo dir in comicDirectories) comicDropdown.options.Add(new Dropdown.OptionData(dir.Name));
    }
    /* ====END OF===== */
    /* ==STATIC-ABLE== */
    /* =============== */


}
