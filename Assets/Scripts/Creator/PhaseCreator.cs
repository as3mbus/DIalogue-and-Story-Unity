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
    public StoryCreator storyController;
    public Toggle baloonToggle, colorToggle, pickerToggle;
    public ToggleGroup fadeToggle;
    public Phase targetPhase;
    public Slider shakeSlider, durationSlider;
    public Text shakeText, durationText;
    public SpriteRenderer backgroundSprite;
    public GameObject TextBaloon, pickerPanel;
    public CUIColorPicker picker;
    public int currentLine = 0;
    // Use this for initialization
    void Start()
    {
        //add listener to change background color based on color picker
        picker.SetOnValueChangeCallback((color) =>
            {
                changeBGColor(color);
            });

    }

    //change background color and toggle color to certain color
    public void changeBGColor(Color color)
    {
        cam.backgroundColor = color;
        pickerToggle.graphic.color = color;
    }
    void Update()
    {

    }

    #region main-function


    // handler for addLine button to either add/insert line
    public void addLine()
    {
        if (lineDDown.value >= targetPhase.messages.Count - 1)
            newLine();
        else
            insertLine();
    }
    //Create New Line at last inded 
    public void newLine()
    {
        targetPhase.newLine();
        lineDDown.options.Add(new Dropdown.OptionData("Line " + targetPhase.messages.Count));
        lineDDown.value++;
    }
    //Insert New Line at current index +1
    public void insertLine()
    {
        if (targetPhase.messages.Count == 0)
            lineDDown.captionText.text = "Line 1";
        targetPhase.insertLine(lineDDown.value + 1);
        lineDDown.options.Add(new Dropdown.OptionData("Line " + targetPhase.messages.Count));
        lineDDown.value++;
    }

    //delete currently selected line 
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

    //handler for changing selected line(Line Dropdown)
    public void changeLine()
    {
        if (targetPhase.messages.Count <= 1)
            return;
        saveLine();
        currentLine = lineDDown.value;
        loadLine(currentLine);
    }

    //save value for selected line
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
    
    // Save Active phase into the story and change canvas to story creator
    public void savePhase()
    {
        saveLine();
        lineDDown.value = 0;
        storyController.gameObject.SetActive(true);
        storyController.contentButtonUpdate(targetPhase);
        this.gameObject.SetActive(false);
    }

    //load line by index into the interface
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
            print("color change called");
            changeBGColor(targetPhase.bgcolor[index]);
            picker.Color = (targetPhase.bgcolor[index]);

            foreach (Toggle togle in fadeToggle.GetComponentsInChildren<Toggle>())
                if (togle.GetComponentInChildren<Text>().text.ToLower() == targetPhase.fademode[index].ToString("g").ToLower())
                    togle.isOn = true;
                else
                    togle.isOn = false;
            durationSlider.value = targetPhase.duration[index];


        }
        else
        {
            lineDDown.value = -1;
            lineDDown.captionText.text = "";
        }

    }

    //load phases into the phase creator and load first line
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
    #endregion

    #region look-and-feel
    //handle color picker toggle  
    public void pickerToggled()
    {
        pickerPanel.SetActive(!pickerToggle.isOn);
    }
    //set cam control to enable on UI enable 
    void OnEnable()
    {
        cam.GetComponent<MouseCamControlPan>().enabled = true;
    }
    //disable camera control and reset interface on disable
    void OnDisable()
    {
        resetInterface();
        cam.GetComponent<MouseCamControlPan>().enabled = false;
    }
    //reset camera position and size
    public void resetCam()
    {
        cam.transform.position = new Vector3(0, 0, -10);
        cam.orthographicSize = 5;
    }
    //handle shake slider value change 
    public void changeShake()
    {
        shakeText.text = shakeSlider.value.ToString();
    }
    //handle duration slider value change 
    public void changeDuration()
    {
        durationText.text = "Duration : " + durationSlider.value.ToString();
    }
    //handle page dropdown change to change the comic as well
    public void changePage()
    {
        backgroundSprite.sprite = targetPhase.comic.pages[pageDDown.value];
    }
    //reset interface value to empty state for next call 
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
    //handle Text Baloon display/hide and control
    public void toggleBaloon()
    {
        TextBaloon.transform.localPosition = new Vector3(0, 0, 8);
        TextBaloon.transform.localScale = Vector3.one;
        TextBaloon.SetActive(baloonToggle.isOn);
    }

    #endregion

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
