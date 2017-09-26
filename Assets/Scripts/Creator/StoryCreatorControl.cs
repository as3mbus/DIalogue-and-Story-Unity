using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using as3mbus.Story;

public class StoryCreatorControl : MonoBehaviour
{
    Story targetStory;
    public GameObject phasePanel, phaseButton, newPhaseButton;
    public ScrollRect phaseScrollView;
    public PhaseCreator phaseCreator;
    public InputField storyNameField;
    public Dropdown comicDropdown;

    // Use this for initialization
    void Start()
    {
        targetStory = new Story();
        // print(Comic.listComicsJson());
        // Comic.writeComicsJson();
        // loadComics();
        insertComicData(ComicManager.readComicsBundleList(Path.Combine(Application.streamingAssetsPath, "streamBundle.json")));
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void newPhases()
    {
        typeWindowActive(true);
    }
    public void newPhase()
    {
        targetStory.phases.Add(new Phase(phasePanel.GetComponentInChildren<InputField>().text, comicDropdown.GetComponentInChildren<Dropdown>().captionText.text));
        typeWindowActive(false);
        print(targetStory.toJson());
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
    void insertDropdownOptions( Dropdown DD, string[] optionsArray)
    {
        foreach (string comic in optionsArray)
        {
            DD.options.Add(new Dropdown.OptionData(comic));
        }
        // var comicPath = "jar:file://" + Application.dataPath + "!/assets/Comic/";
        // var comicDirectories = new DirectoryInfo(comicPath).GetDirectories();
        // foreach (DirectoryInfo dir in comicDirectories) comicDropdown.options.Add(new Dropdown.OptionData(dir.Name));
    }

    void contentResize()
    {
        Vector2 contentSize = phaseScrollView.content.sizeDelta;
        contentSize.y = 250 * (targetStory.phases.Count + 1) + 50;
        phaseScrollView.content.sizeDelta = contentSize;
    }
    void newContentButton()
    {
        GameObject newButton = Object.Instantiate(phaseButton, phaseScrollView.content);
        Vector3 newpos = newButton.GetComponent<RectTransform>().localPosition;
        newpos.y -= 250 * (targetStory.phases.Count - 1);
        newButton.GetComponent<RectTransform>().localPosition = newpos;
        newButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => editPhase());
        newButton.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => deletePhase());

        contentButtonUpdate(newButton);
        newpos.y -= 250;
        newPhaseButton.GetComponent<RectTransform>().localPosition = newpos;
        contentResize();
    }
    void contentButtonUpdate(GameObject button)
    {
        int index = button.transform.GetSiblingIndex() - 1;
        Phase fase = (Phase)targetStory.phases[index];
        button.transform.GetChild(0).Find("Name").GetComponent<Text>().text = fase.name;
        button.transform.GetChild(0).Find("Type").GetComponent<Text>().text = "";
        button.transform.GetChild(0).Find("BG").GetComponent<Text>().text = fase.comic.toString();
        button.transform.GetChild(0).Find("Line").GetComponent<Text>().text = fase.messages.Count.ToString() + " Line";
    }
    public void contentPhaseButtonUpdate(Phase fase)
    {
        int index = targetStory.phases.IndexOf(fase) + 1;
        GameObject button = phaseScrollView.content.GetChild(index).gameObject;

        button.transform.GetChild(0).Find("Name").GetComponent<Text>().text = fase.name;
        button.transform.GetChild(0).Find("Type").GetComponent<Text>().text = "";
        button.transform.GetChild(0).Find("BG").GetComponent<Text>().text = fase.comic.toString();
        button.transform.GetChild(0).Find("Line").GetComponent<Text>().text = fase.messages.Count.ToString() + " Line";

    }
    public void editPhase()
    {
        int phaseIndex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex() - 1;
        Phase fase = (Phase)targetStory.phases[phaseIndex];
        editPhase(fase);
    }
    public void deletePhase()
    {
        int phaseIndex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex() - 1;
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
        contentResize();

    }
    public void editPhase(Phase fase)
    {
        phaseCreator.loadPhase(fase);
        phaseCreator.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void playScene()
    {
        SceneManager.LoadScene("Player");
    }
}
