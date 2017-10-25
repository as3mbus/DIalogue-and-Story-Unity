using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using as3mbus.Story;
using SimpleFileBrowser.Scripts.GracesGames;
using LitJson;
public class StoryCreator : MonoBehaviour
{
    //getter and setter for target story 
    public Story targetStory
    {
        get { return _targetStory; }
        set
        {
            resetStory();
            _targetStory = value;
            loadStory();
        }
    }
    Story _targetStory;
    public GameObject phasePanel, phaseButton, newPhaseButton;
    public ScrollRect phaseScrollView;
    public PhaseCreator phaseCreator;
    public InputField storyNameField, phaseNameField;
    public Dropdown comicBundleDropdown, comicDirectoryDropdown, bgmBundleDropdown, bgmDirectoryDropdown;
    public GameObject FileBrowserPrefab;
    string[] activeComics, activeBgm;

    void Awake()
    {
        //create/load new/existing story based on static class story manager 
        _targetStory = StoryManager.stori;
    }

    void OnEnable()
    {
        print(_targetStory.toJson());
    }

    // Use this for initialization
    void Start()
    {

        loadStory();
        newPhaseButton.transform.SetAsLastSibling();
        //list and write streaming asset bundle data 
        DataManager.listStreamingAssetBundleJson(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        //read streaming asset bundle data 
        DataManager.readStreamingAssetBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundles.json"));
        //add options of bundle in streaming assets 
        addDropdownOptions(comicBundleDropdown, DataManager.streamingAssetBundleList.ToArray());
        //set to load 1st available bundle 
        resetDropdownValue(comicBundleDropdown);
        // also applies to bgm dropdowns
        addDropdownOptions(bgmBundleDropdown, DataManager.streamingAssetBundleList.ToArray());
        resetDropdownValue(bgmBundleDropdown);

    }

    #region main function

    //handle new phase button 
    //disable any interactive on story creator interface 
    public void newPhases()
    {
        phasePanelActive(true);
        phasePanel.transform.GetChild(phasePanel.transform.childCount - 1).GetComponent<Button>().onClick.AddListener(() => createPhase());
    }

    //handle create phase button inside phase panel
    // adding new phase button and enable story creator interface 
    public void createPhase()
    {
        _targetStory.phases.Add(
            new Phase(
                phaseNameField.text,
                comicBundleDropdown.captionText.text,
                activeComics[comicDirectoryDropdown.value]
                )
            );
        phasePanelActive(false);
        newContentButton();
    }

    //update content button value (gameobject) 
    public void contentButtonUpdate(GameObject button)
    {
        int index = button.transform.GetSiblingIndex();
        // print(phaseScrollView.content.transform.childCount);

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
        button.transform.GetChild(0).Find("BundleFolder").GetComponent<Text>().text = fase.comic.bundleName + " - " + Path.GetFileName(fase.comic.comicDirectory);
        button.transform.GetChild(0).Find("Line").GetComponent<Text>().text = fase.Lines.Count.ToString() + " Line";
    }
    //phase content button handler
    //get button index and throw editPhase(Phase)
    public void editLines()
    {
        int phaseIndex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex();
        Phase fase = _targetStory.phases[phaseIndex];
        editLines(fase);
    }


    //phase content button handler
    //display phase editor and edit phase with it's interfacce  
    public void editLines(Phase fase)
    {
        fase.comic.loadAllPages();
        fase.loadBGM();
        phaseCreator.gameObject.SetActive(true);
        phaseCreator.loadPhase(fase);
        gameObject.SetActive(false);
    }
    public void editPhase()
    {
        int phaseIndex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex();
        Phase fase = _targetStory.phases[phaseIndex];
        loadPhasePanel(fase);
    }
    public void loadPhasePanel(Phase fase)
    {
        phasePanelActive(true);
        phaseNameField.text = fase.name;
        comicBundleDropdown.value = comicBundleDropdown.options.FindIndex(option => option.text == fase.comic.bundleName);
        comicDirectoryDropdown.value = comicDirectoryDropdown.options.FindIndex(option => option.text == Path.GetFileName(fase.comic.comicDirectory));
        bgmBundleDropdown.value = bgmBundleDropdown.options.FindIndex(option => option.text == fase.bgmAssetBundleName);
        bgmDirectoryDropdown.value = bgmDirectoryDropdown.options.FindIndex(option => option.text == Path.GetFileName(fase.bgmFileName));
        phasePanel.transform.GetChild(phasePanel.transform.childCount - 1).GetComponent<Button>().onClick.AddListener(delegate { updatePhase(fase); });


    }
    public void updatePhase(Phase fase)
    {
        fase.name = phaseNameField.text;
        fase.comic.bundleName = comicBundleDropdown.captionText.text;
        fase.comic.comicDirectory = comicDirectoryDropdown.captionText.text;
        fase.bgmAssetBundleName = bgmBundleDropdown.captionText.text;
        fase.bgmFileName = bgmDirectoryDropdown.captionText.text;
        contentButtonUpdate(fase);
        phasePanelActive(false);
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
        _targetStory.phases.RemoveAt(phaseIndex);

    }

    //Handle play Button
    //open player scene and play the active story content
    public void playScene()
    {
        _targetStory.name = storyNameField.text;
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
        fileBrowserScript.SetupFileBrowser(ViewMode.Portrait);
        if (fileBrowserMode == FileBrowserMode.Save)
            fileBrowserScript.SaveFilePanel(this, "saveStory", storyNameField.text, "json");
        else
            fileBrowserScript.OpenFilePanel(this, "readStory", "json");
    }
    //handle load button on Simple File Manager
    //set target story to loaded story 
    void readStory(string path)
    {
        targetStory = Story.parseJson(JsonMapper.ToObject(DataManager.readAssetsTextFile(path)));
    }
    //load story content to interface
    void loadStory()
    {
        storyNameField.text = _targetStory.name;
        foreach (var item in _targetStory.phases)
            newContentButton();
    }

    // 2 function below can be refactored !!!!!!
    //handle comic bundle selection change
    //add option of comic inside bandle into the comic dropdown 
    public void onComicBundleDropdownChange()
    {
        comicDirectoryDropdown.options.Clear();
        activeComics = Comic.getComics(
                                        DataManager.readAssetBundles(
                                            DataManager.bundlePath(
                                                comicBundleDropdown.captionText.text)
                                            )
                                        );
        addDropdownOptions(comicDirectoryDropdown, activeComics);
        resetDropdownValue(comicDirectoryDropdown);
    }
    //handle bgm bundle selection change
    //add option of bgm inside bandle into the bgm dropdown 
    public void onBgmBundleDropdownChange()
    {
        bgmDirectoryDropdown.options.Clear();
        activeBgm = DataManager.getAudio(
                                        DataManager.readAssetBundles(
                                            DataManager.bundlePath(
                                                bgmBundleDropdown.captionText.text)
                                            )
                                        );
        addDropdownOptions(bgmDirectoryDropdown, activeBgm);
        resetDropdownValue(bgmDirectoryDropdown);
    }
    // handle X button in phase panel
    //display/hide phase panel and disable/enable story interface
    public void phasePanelActive(bool mode)
    {
        phasePanel.SetActive(mode);
        var storySelectables = GetComponentsInChildren<Selectable>();
        GetComponentInChildren<InputField>().interactable = !mode;
        foreach (var button in storySelectables)
            button.interactable = !mode;
        phasePanel.transform.GetChild(phasePanel.transform.childCount - 1).GetComponent<Button>().onClick.RemoveAllListeners();
    }
    //handle add phase button 
    //create phase content button and reset add phase button position 
    void newContentButton()
    {
        GameObject newButton = Object.Instantiate(phaseButton, phaseScrollView.content, false);
        newButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => editLines());
        newButton.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => deletePhase());
        newButton.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => editPhase());
        newPhaseButton.transform.SetAsLastSibling();
        contentButtonUpdate(newButton);
        // contentResize();
    }

    //Empty phase and reset add phase button position 
    public void resetStory()
    {
        var children = new List<GameObject>();
        foreach (Transform child in phaseScrollView.content.transform) children.Add(child.gameObject);
        children.ForEach(child =>
        {
            if (!child.name.Contains("Add")) DestroyImmediate(child);
        });
        _targetStory.phases.Clear();
        newPhaseButton.transform.SetAsLastSibling();
    }

    #endregion

    #region Look and Feel


    #endregion
    /* =============== */
    /* ==STATIC-ABLE== */
    /* =============== */
    // add dropdown options from array
    void addDropdownOptions(Dropdown DD, string[] optionsArray)
    {
        foreach (string comic in optionsArray)
        {
            DD.options.Add(new Dropdown.OptionData(Path.GetFileName(comic)));
        }
    }

    //reset dropdown value to index 0 or empty it when there are no options
    void resetDropdownValue(Dropdown DD)
    {
        if (DD.options.Count > 0)
        {
            DD.value = 0;
            DD.captionText.text = DD.options[0].text;
        }
        else
            DD.captionText.text = "";
    }
    /* ====END OF===== */
    /* ==STATIC-ABLE== */
    /* =============== */


}
