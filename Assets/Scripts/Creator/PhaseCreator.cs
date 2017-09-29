using System;
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
    public Slider shakeSlider, durationSlider;
    public Text shakeText,durationText;
    public SpriteRenderer backgroundSprite;
    public GameObject TextBaloon, pickerPanel;
    public CUIColorPicker picker;
    public int currentLine = 0;
    // Use this for initialization
    void Start()
    {

        picker.SetOnValueChangeCallback((color) =>
            {
                cam.backgroundColor = color;
                pickerToggle.graphic.color = color;
            });
        picker.Color = Color.black;

    }
    void Update()
    {

    }

    /* =============== */
    /* =Main Function= */
    /* =============== */
    public void newLine()
    {
        targetPhase.newLine();
        lineDDown.options.Add(new Dropdown.OptionData("Line " + targetPhase.messages.Count));
        lineDDown.value++;
    }
    public void insertLine()
    {
        if (targetPhase.messages.Count == 0)
            lineDDown.captionText.text = "Line 1";
        targetPhase.insertLine(lineDDown.value + 1);
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
    public void changeLine()
    {
        if (targetPhase.messages.Count <= 1)
            return;
        saveLine();
        currentLine = lineDDown.value;
        loadLine(currentLine);
    }
    public void saveLine()
    {
        if (targetPhase.messages.Count == 0) return;
        targetPhase.UpdateLine(
            characterDDown.captionText.text,
            messageField.text, pageDDown.value,
            cam.orthographicSize,
            cam.transform.position,
            shakeSlider.value,
            baloonToggle.isOn ? TextBaloon.transform.localPosition : Vector3.zero,
            baloonToggle.isOn ? TextBaloon.transform.localScale.x : 0,
            Phase.parseFadeMode(fadeToggle.ActiveToggles().FirstOrDefault().GetComponentInChildren<Text>().text),
            colorToggle.isOn ? picker.Color : Color.black,
            durationSlider.value,
            currentLine);
    }
    public void savePhase()
    {
        saveLine();
        lineDDown.value = 0;
        storyController.gameObject.SetActive(true);
        storyController.contentButtonUpdate(targetPhase);
        this.gameObject.SetActive(false);
    }
    public void loadLine(int index)
    {
        if (targetPhase.messages.Count > 0)
        {
            lineDDown.captionText.text = lineDDown.options[index].text;
            characterDDown.value = characterDDown.options.IndexOf(characterDDown.options.Find(x => x.text == targetPhase.characters[index]));
            messageField.text = targetPhase.messages[index];
            pageDDown.value = targetPhase.pages[index];
            cam.transform.position = targetPhase.paths[index];
            cam.orthographicSize = targetPhase.zooms[index];
            shakeSlider.value = targetPhase.shake[index];
            if (Mathf.Abs(targetPhase.baloonpos[index].x)
            + Mathf.Abs(targetPhase.baloonpos[index].y) != 0)
            {
                baloonToggle.isOn = true;
                TextBaloon.transform.localPosition = targetPhase.baloonpos[index];
                TextBaloon.transform.localScale = new Vector2(targetPhase.baloonsize[index], targetPhase.baloonsize[index]);
            }
            else baloonToggle.isOn = false;
            shakeSlider.value = targetPhase.shake[index];
            foreach (Toggle togle in fadeToggle.GetComponentsInChildren<Toggle>())
                if (togle.GetComponentInChildren<Text>().text.ToLower() == targetPhase.fademode[index].ToString("g").ToLower())
                    togle.isOn = true;
            picker.Color = targetPhase.bgcolor[index];
            durationSlider.value = targetPhase.duration[index];


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
    /* ====END OF===== */
    /* =Main Function= */
    /* =============== */

    /* =============== */
    /* =LOOK AND FEEL= */
    /* =============== */

    public void colorToggled()
    {
        pickerPanel.SetActive(colorToggle.isOn && !pickerToggle.isOn);
        pickerToggle.gameObject.SetActive(colorToggle.isOn);
    }
    public void pickerToggled()
    {
        pickerPanel.SetActive(!pickerToggle.isOn);
    }
    void OnEnable()
    {
        cam.GetComponent<MouseCamControlPan>().enabled = true;
    }
    void OnDisable()
    {
        resetInterface();
        cam.GetComponent<MouseCamControlPan>().enabled = false;
    }
    public void resetCam()
    {
        cam.transform.position = new Vector3(0, 0, -10);
        cam.orthographicSize = 5;
    }

    public void changeShake()
    {
        shakeText.text = shakeSlider.value.ToString();
    }
    public void changeDuration()
    {
        durationText.text = "Duration : " +durationSlider.value.ToString();
    }
    public void changePage()
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
        TextBaloon.transform.localPosition = new Vector3(0, 0, 8);
        TextBaloon.transform.localScale = Vector3.one;
        TextBaloon.SetActive(baloonToggle.isOn);
    }

    /* ====END OF===== */
    /* =LOOK AND FEEL= */
    /* =============== */

    /* =============== */
    /* ==STATIC-ABLE== */
    /* =============== */


    public void addDropdownOption(Dropdown dropdown, string[] options)
    {
        foreach (string option in options)
            dropdown.options.Add(new Dropdown.OptionData(option));
    }

    /* ====END OF===== */
    /* ==STATIC-ABLE== */
    /* =============== */

}
