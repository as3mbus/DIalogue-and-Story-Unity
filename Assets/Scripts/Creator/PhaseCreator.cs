using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using as3mbus.Story;

public class PhaseCreator : MonoBehaviour
{
    public Camera cam;
    public InputField messageField;
    public Dropdown characterDDown, pageDDown, lineDDown;
    public StoryCreatorControl storyController;
    public Toggle baloonToggle, colorToggle, pickerToggle;
    public ToggleGroup fadeToggle;
    public Phase targetPhase;
    public Slider shakeSlider;
    public Text shakeText;
    public SpriteRenderer backgroundSprite;
    public GameObject TextBaloon, pickerPanel;
    public CUIColorPicker picker;
    public int currentLine = 0;
    // Use this for initialization
    void Start()
    {
        targetPhase = new Phase();
        picker.Color=Color.black;
    }
    void Update(){
        cam.backgroundColor = picker.Color;
        pickerToggle.graphic.color=picker.Color;
    }
    public void colorToggled()
    {
        pickerPanel.SetActive(colorToggle.isOn&& pickerToggle.isOn);
        pickerToggle.gameObject.SetActive(colorToggle.isOn);
    }
    public void pickerToggled()
    {
        pickerPanel.SetActive(pickerToggle.isOn);
    }
    public void newLine()
    {
        targetPhase.newLine();
        lineDDown.options.Add(new Dropdown.OptionData("Line " + targetPhase.messages.Count));
        lineDDown.value++;
    }
    public void deleteLine()
    {
        if (targetPhase.messages.Count == 0)
            return;
        lineDDown.value--;
        targetPhase.deleteLine(lineDDown.value);
        lineDDown.options.RemoveAt(targetPhase.messages.Count);
        if (targetPhase.messages.Count <= 0)
            lineDDown.captionText.text = "";
    }
    public void addLine()
    {
        if (lineDDown.value >= targetPhase.messages.Count - 1)
            newLine();
        else
            insertLine();
    }
    public void insertLine()
    {
        if (targetPhase.messages.Count == 0)
            lineDDown.captionText.text = "Line 1";
        targetPhase.insertLine(lineDDown.value + 1);
        lineDDown.options.Add(new Dropdown.OptionData("Line " + targetPhase.messages.Count));
        lineDDown.value++;
    }
    public void changeLine()
    {
        if (targetPhase.messages.Count <= 1)
            return;
        targetPhase.UpdateLine(characterDDown.captionText.text, messageField.text, pageDDown.value, cam.orthographicSize, cam.transform.position, currentLine);
        currentLine = lineDDown.value;
        loadLine(currentLine);
    }
    public void savePhase()
    {
        lineDDown.value = -1;
        storyController.gameObject.SetActive(true);
        storyController.contentPhaseButtonUpdate(targetPhase);
        this.gameObject.SetActive(false);
    }
    public void loadLine(int index)
    {
        if (targetPhase.messages.Count > 0)
        {
            characterDDown.value = characterDDown.options.IndexOf(characterDDown.options.Find(x => x.text == targetPhase.characters[index]));
            messageField.text = targetPhase.messages[index];
            pageDDown.value = targetPhase.pages[index];
            cam.transform.position = targetPhase.paths[index];
            cam.orthographicSize = targetPhase.zooms[index];
            lineDDown.captionText.text = "Line " + (index + 1);
        }
        else
        {
            lineDDown.value = -1;
            lineDDown.captionText.text = "";
        }
    }
    public void loadPhase(Phase fase)
    {
        targetPhase = fase;
        for (int i = 0; i < fase.messages.Count; i++)
            lineDDown.options.Add(new Dropdown.OptionData("Line " + (i + 1)));
        addDropdownOption(pageDDown, fase.comic.pagename.ToArray());
        pageDDown.value = 0;
        pageDDown.captionText.text = pageDDown.options[0].text;
        loadLine(0);
    }
    void OnEnable()
    {
        activeCamDrag(true);
    }
    void OnDisable()
    {
        resetInterface();
        activeCamDrag(false);
    }
    public void activeCamDrag(bool active){
        cam.GetComponent<MouseCamControlPan>().enabled = active;
    }
    public void resetCam()
    {
        cam.transform.position = new Vector3(0, 0, -10);
        cam.orthographicSize = 5;
    }
    public void addDropdownOption(Dropdown dropdown, string[] options)
    {
        foreach (string option in options)
        {
            dropdown.options.Add(new Dropdown.OptionData(option));
        }
    }
    public void changeShake(){
        shakeText.text = "Shake : "+shakeSlider.value;
    }
    public void pageChange()
    {
        backgroundSprite.sprite = targetPhase.comic.pages[pageDDown.value];
    }

    void resetInterface()
    {
        pageDDown.ClearOptions();
        pageDDown.value = 0;
        lineDDown.captionText.text = "";
        messageField.text = "";
        lineDDown.ClearOptions();
        lineDDown.value = 0;
        lineDDown.captionText.text = "";
        resetCam();
    }
    public void toggleBaloon()
    {
        TextBaloon.transform.localPosition = new Vector3(0,0,8);
        TextBaloon.transform.localScale = Vector3.one;
        TextBaloon.SetActive(baloonToggle.isOn);
    }
            // Debug.Log(fadeToggle.ActiveToggles().FirstOrDefault().GetComponentInChildren<Text>().text);

}
