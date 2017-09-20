using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseCreator : MonoBehaviour {
    public Camera cam;
    public InputField messageField;
    public Dropdown characterDDown, pageDDown, lineDDown;
    public StoryCreatorControl storyController;
    public Text statusText;
    public Phase targetPhase;
    public int currentLine = 0;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        statusText.text = "View\n\nX " + cam.transform.position.x.ToString("0.00") + "\n\nY " + cam.transform.position.y.ToString("0.00") + "\n\nZ " + cam.orthographicSize.ToString("0.00");
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
    public void insertLine()
    {
        if (targetPhase.messages.Count <= 0)
            lineDDown.captionText.text = "Line 1";
        targetPhase.insertLine(lineDDown.value);
        lineDDown.options.Add(new Dropdown.OptionData("Line " + targetPhase.messages.Count));
        lineDDown.value++;
    }
    public void changeLine()
    {
        if (targetPhase.messages.Count <= 1)
            return;
        targetPhase.UpdateLine(characterDDown.captionText.text, messageField.text, int.Parse(pageDDown.captionText.text), cam.orthographicSize, cam.transform.position, currentLine);
        currentLine = lineDDown.value;
        loadLine(currentLine);
    }
    public void savePhase()
    {
        lineDDown.value=-1;
        resetInterface();
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
            pageDDown.value = pageDDown.options.IndexOf(pageDDown.options.Find(x => x.text == targetPhase.pages[index].ToString()));
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
        loadLine(0);
        cam.GetComponent<MouseCamControlPan>().enabled = true;
    }
    public void resetCam()
    {
        cam.transform.position = new Vector3(0, 0, -10);
        cam.orthographicSize = 5;
        cam.GetComponent<MouseCamControlPan>().enabled = false;
    }
    void resetInterface()
    {
        messageField.text = "";
        lineDDown.ClearOptions();
        lineDDown.value = 0;
        lineDDown.captionText.text = "";
        resetCam();
    }
}
