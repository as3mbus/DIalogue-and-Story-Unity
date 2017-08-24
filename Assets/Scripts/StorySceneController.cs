using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class StorySceneController : MonoBehaviour {
	private static JsonData jsonStory;
	// Use this for initialization
	void Start () {
		TextAsset teksJson;
		teksJson = Resources.Load("StoryScene") as TextAsset;
		jsonStory = JsonMapper.ToObject(teksJson.ToString());
		// for (int i = 0; i < jsonStory["Quest"].Count; i++)
        // {
        //     if ((int)jsonStory["Quest"][i]["id"] == id)
        //     {
        //         Quest items = new Quest((int)jsonStory["Quest"][i]["id"],
        //             jsonStory["Quest"][i]["type"].ToString(),
        //             (int)jsonStory["Quest"][i]["idType"],
        //             jsonStory["Quest"][i]["name"].ToString(),
        //             (int)jsonStory["Quest"][i]["goal"],
        //             jsonStory["Quest"][i]["reward"]["type"].ToString(),
        //             (int)jsonStory["Quest"][i]["reward"]["quantity"],
        //             (bool)jsonStory["Quest"][i]["finished"],
        //             (bool)jsonStory["Quest"][i]["available"],
        //             jsonStory["Quest"][i]["quicktype"].ToString(),
        //             (int)jsonStory["Quest"][i]["cp"]
        //             );
        //         return items;
        //     }
        // }
        // return null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
