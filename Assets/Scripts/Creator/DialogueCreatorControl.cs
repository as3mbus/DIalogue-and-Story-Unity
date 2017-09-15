using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCreatorControl : MonoBehaviour
{
    public Camera cam;
    public InputField messageField;
    public Dropdown characterDDown, pageDDown, lineDDown;
    public StoryCreatorControl storyController;
    public Text statusText;
    public Dialogue targetDialogue;
    public int currentLine = -1;
    // Use this for initialization
    void Start()
    {
        // targetDialogue = new Dialogue();
        // targetDialogue.comic = new Comic("sample comic");
        // newLine();
    }

    // Update is called once per frame
    void Update()
    {
        statusText.text = "View\n\nX " + cam.transform.position.x.ToString("0.00") + "\n\nY " + cam.transform.position.y.ToString("0.00") + "\n\nZ " + cam.orthographicSize.ToString("0.00");
    }

    public void newLine()
    {
        if (targetDialogue.messages.Count > 0)
            targetDialogue.UpdateLine(characterDDown.captionText.text, messageField.text, int.Parse(pageDDown.captionText.text), cam.orthographicSize, cam.transform.position, currentLine);
        targetDialogue.newLine();
        lineDDown.options.Add(new Dropdown.OptionData("Line " + targetDialogue.messages.Count));
        lineDDown.value = currentLine + 1;
    }
    public void deleteLine()
    {
        if (targetDialogue.messages.Count == 0)
            return;
        targetDialogue.deleteLine(currentLine);
        if (currentLine > 0)
            currentLine--;
        lineDDown.value = currentLine;
        lineDDown.options.RemoveAt(targetDialogue.messages.Count);
        if (targetDialogue.messages.Count > 0)
            loadLine(currentLine);
        else
            lineDDown.captionText.text = "";
    }
    public void insertLine()
    {
        if (targetDialogue.messages.Count > 0)
        {
            targetDialogue.UpdateLine(characterDDown.captionText.text, messageField.text, int.Parse(pageDDown.captionText.text), cam.orthographicSize, cam.transform.position, currentLine);
            currentLine++;
        }
        else
        {
            lineDDown.captionText.text = "Line 1";
        }
        targetDialogue.insertLine(currentLine);
        lineDDown.options.Add(new Dropdown.OptionData("Line " + targetDialogue.messages.Count));
        lineDDown.value = currentLine;
    }
    public void changeLine()
    {
        targetDialogue.UpdateLine(characterDDown.captionText.text, messageField.text, int.Parse(pageDDown.captionText.text), cam.orthographicSize, cam.transform.position, currentLine);
        currentLine = lineDDown.value;
        Debug.Log(targetDialogue.toJson());
        loadLine(currentLine);
    }
    public void loadLine(int index)
    {
        if (targetDialogue.messages.Count > 0)
        {
            characterDDown.value = characterDDown.options.IndexOf(characterDDown.options.Find(x => x.text == targetDialogue.characters[index]));
            messageField.text = targetDialogue.messages[index];
            pageDDown.value = pageDDown.options.IndexOf(pageDDown.options.Find(x => x.text == targetDialogue.pages[index].ToString()));
            cam.transform.position = targetDialogue.paths[index];
            cam.orthographicSize = targetDialogue.zooms[index];
            lineDDown.value = index;
            lineDDown.captionText.text = "Line " + (index + 1);
        }
        else
        {
            lineDDown.value = -1;
            lineDDown.captionText.text = "";
        }
    }
    public void loadDialogue(Dialogue dialog)
    {
        targetDialogue = dialog;
        for (int i = 0; i < dialog.messages.Count; i++)
            lineDDown.options.Add(new Dropdown.OptionData("Line " + (i + 1)));
        loadLine(0);

    }
    public void saveDialogue()
    {
        if (targetDialogue.messages.Count > 0)
            targetDialogue.UpdateLine(characterDDown.captionText.text, messageField.text, int.Parse(pageDDown.captionText.text), cam.orthographicSize, cam.transform.position, currentLine);
        resetInterface();
        storyController.gameObject.SetActive(true);
        storyController.contentDialogueButtonUpdate(targetDialogue);
        this.gameObject.SetActive(false);
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
        lineDDown.value = -1;
        lineDDown.captionText.text = "";
    }
}
