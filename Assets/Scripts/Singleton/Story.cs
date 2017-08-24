using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story
{
    public ArrayList phase;
    
    public void loadStory(){
        TextAsset teksJson;
		teksJson = Resources.Load("StoryScene") as TextAsset;
		jsonStory = JsonMapper.ToObject(teksJson.ToString());
    }
    
}