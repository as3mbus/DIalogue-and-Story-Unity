using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using as3mbus.Story;
using GracesGames;

public class StoryCreator : MonoBehaviour
{
    //getter and setter for target story 
    public Story targetStory
    {
        get { return _targetStory; }
        set
        {
            resetPhase();
            _targetStory = value;
            loadStory();
        }
    }
    Story _targetStory;
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
        //create/load new/existing story based on static class story manager 
        _targetStory = new Story(StoryManager.storyType);
        loadStory();
        // print(Comic.listComicsJson());
        // Comic.writeComicsJson();
        // loadComics();
        resetAddPhaseButton();
        //list and write streaming asset bundle data 
        DataManager.listStreamingAssetsBundleJson(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        //read streaming asset bundle data 
        DataManager.readStreamingAssetsBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        //add options of bundle in streaming assets 
        addDropdownOptions(bundleDropdown, DataManager.streamingAssetsBundleList.ToArray());
        //set to load 1st available bundle 
        bundleDropdown.value = 0;
        bundleDropdown.captionText.text = bundleDropdown.options[0].text;
    }

    #region main function

    //handle new phase button 
    //disable any interactive on story creator interface 
    public void newPhases()
    {
        typeWindowActive(true);
    }

    //handle create phase button inside phase panel
    // adding new phase button and enable story creator interface 
    public void createPhase()
    {
        _targetStory.phases.Add(new Phase(phaseNameField.text, bundleDropdown.captionText.text, comicDropdown.captionText.text));
        typeWindowActive(false);
        newContentButton();
    }
    //handle cancel button inside phase panel
    //enable sstory creator interface
    public void cancelPhase()
    {
        typeWindowActive(false);
    }
    //update content button value (gameobject) 
    public void contentButtonUpdate(GameObject button)
    {
        int index = button.transform.GetSiblingIndex();
        print(phaseScrollView.content.transform.childCount);

        Phase fase = _targetStory.phases[index];
        contentButtonUpdate(fase, button);
    }
    //update content button value of a phase 
    public void contentButtonUpdate(Phase fase)
    {
        int index = _targetStory.phases.IndexOf(fase);
        GameObject button = phaseScrollView.content.GetChild(index).gameObject;
        contentButtonUpdate(fase, button);
    }
    //update content button value based on a phase 
    public void contentButtonUpdate(Phase fase, GameObject button)
    {
        button.transform.GetChild(0).Find("Name").GetComponent<Text>().text = fase.name;
        button.transform.GetChild(0).Find("Type").GetComponent<Text>().text = "";
        button.transform.GetChild(0).Find("BG").GetComponent<Text>().text = fase.comic.toString();
        button.transform.GetChild(0).Find("Line").GetComponent<Text>().text = fase.Lines.Count.ToString() + " Line";
    }
    //phase content button handler
    //get button index and throw editPhase(Phase)
    public void editPhase()
    {
        int phaseIndex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex();
        Phase fase =_targetStory.phases[phaseIndex];
        editPhase(fase);
    }
    //phase content button handler
    //display phase editor and edit phase with it's interfacce  
    public void editPhase(Phase fase)
    {
        phaseCreator.gameObject.SetActive(true);
        phaseCreator.loadPhase(fase);
        gameObject.SetActive(false);
    }
    //handle X button on phase content button
    //destroy and delete phase 
    public void deletePhase()
    {
        int phaseIndex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex();
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        Vector3 newpos = newPhaseButton.GetComponent<RectTransform>().localPosition;
        newpos.y += 250;
        newPhaseButton.GetComponent<RectTransform>().localPosition = newpos;

        //DELETE THIS LATER
        // for (int i = phaseIndex + 1; i < phaseScrollView.content.childCount; i++)
        // {
        //     var newButton = phaseScrollView.content.GetChild(i);
        //     newpos = newButton.GetComponent<RectTransform>().localPosition;
        //     newpos.y += 250;
        //     newButton.GetComponent<RectTransform>().localPosition = newpos;

        // }

        _targetStory.phases.RemoveAt(phaseIndex);
        // contentResize();

    }

    //Handle play Button
    //open player scene and play the active story content
    public void playScene()
    {
        _targetStory.name = storyNameField.text;
        StoryManager.storyType = storyDataType.Story;
        StoryManager.stori = _targetStory;
        StoryManager.nextScene = "Creator";
        StoryManager.skipable = true;
        SceneManager.LoadScene("Player");
    }
    //handle save button on simple file manager 
    //save story into a complete filename path 
    void saveStory(string path)
    {
        _targetStory.name = storyNameField.text;
        var sr = File.CreateText(path);
        sr.Write(_targetStory.toJson());
        sr.Close();
    }
    //Handle Save and Load Button
    //call file browser 
    public void OpenFileBrowser(bool save)
    {
        if (save) OpenFileBrowser(FileBrowserMode.Save);
        else OpenFileBrowser(FileBrowserMode.Load);
    }
    //call file browser based on Simple File Manager 
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
            fileBrowserScript.OpenFilePanel(this, "readStory", "json");
    }
    //handle load button on Simple File Manager
    //set target story to loaded story 
    void readStory(string path)
    {
        targetStory = new Story(path);
    }
    //load story content to interface
    void loadStory()
    {
        storyNameField.text = _targetStory.name;
        foreach (var item in _targetStory.phases)
        {
            newContentButton();
        }
    }
    //handle bundle selection change
    //add option of comic inside bandle into the comic dropdown 
    public void bundleChange()
    {
        comicDropdown.options.Clear();
        if (DataManager.isStreamingAssetsContent(bundleDropdown.captionText.text))
            activeComics = ComicManager.getComics(DataManager.readAssetsBundles(Path.Combine(Application.streamingAssetsPath, bundleDropdown.captionText.text)));
        else
            activeComics = ComicManager.getComics(DataManager.readAssetsBundles(Path.Combine(Application.persistentDataPath, bundleDropdown.captionText.text)));
        addDropdownOptions(comicDropdown, activeComics);
        comicDropdown.value = 0;
        comicDropdown.captionText.text = comicDropdown.options[0].text;
    }
    //display/hide phase panel and disable/enable story interface
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
    //handle add phase button 
    //create phase content button and reset add phase button position 
    void newContentButton()
    {
        GameObject newButton = Object.Instantiate(phaseButton, phaseScrollView.content);
        newButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => editPhase());
        newButton.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => deletePhase());
        resetAddPhaseButton();
        contentButtonUpdate(newButton);
        // contentResize();
    }
    //place add phase button to last sibling (last place) 
    void resetAddPhaseButton()
    {
        newPhaseButton.transform.SetAsLastSibling();
    }

    //Empty phase and reset add phase button position 
    public void resetPhase()
    {
        var children = new List<GameObject>();
        foreach (Transform child in phaseScrollView.content.transform) children.Add(child.gameObject);
        children.ForEach(child =>
        {
            if (!child.name.Contains("Add")) DestroyImmediate(child);
        });
        _targetStory.phases.Clear();
        resetAddPhaseButton();
    }

    #endregion

    #region Look and Feel


    #endregion
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
