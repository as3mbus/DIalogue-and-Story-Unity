using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCreatorControl : MonoBehaviour
{
    public Camera cam;
    public InputField messageField;
    public Dropdown characterDDown, pageDDown, lineDDown;
    public Text statusText;
    public Dialogue targetDialogue;
    public int currentLine = 0;
    // Use this for initialization
    void Start()
    {
        targetDialogue = new Dialogue();
        targetDialogue.comic = new Comic("sample comic");
        newLine();
    }

    // Update is called once per frame
    void Update()
    {
        statusText.text = "View\n\nX " + cam.transform.position.x.ToString("0.00") + "\n\nY " + cam.transform.position.y.ToString("0.00") + "\n\nZ " + cam.orthographicSize.ToString("0.00");
    }

    public void newLine()
    {
        if (currentLine > 0)
            targetDialogue.UpdateLine(characterDDown.captionText.text, messageField.text, int.Parse(pageDDown.captionText.text), cam.orthographicSize, cam.transform.position, currentLine);
        Debug.Log(targetDialogue.toJson());
        targetDialogue.newLine();
        lineDDown.options.Add(new Dropdown.OptionData("Line " + targetDialogue.messages.Count));
		lineDDown.value=currentLine+1;
    }
    public void deleteLine()
    {
        targetDialogue.deleteLine(currentLine);
        Debug.Log(targetDialogue.toJson());
        currentLine--;
        lineDDown.value = currentLine;
    }
    public void insertLine()
    {
        if (currentLine > 0)
            targetDialogue.UpdateLine(characterDDown.captionText.text, messageField.text, int.Parse(pageDDown.captionText.text), cam.orthographicSize, cam.transform.position, currentLine);
        Debug.Log(targetDialogue.toJson());
        currentLine++;
        targetDialogue.insertLine(currentLine);
        lineDDown.options.Add(new Dropdown.OptionData("Line " + targetDialogue.messages.Count));
        lineDDown.value = currentLine;
    }
    public void changeLine()
    {

    }

}
