using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class Dialogue
{
    public List<string> character=new List<string>();
    public List<string> message=new List<string>();
    public Dialogue(JsonData chara,JsonData msg){
        for (int i = 0; i < chara.Count; i++)
        {
            this.character.Add(chara[i].ToString());
            this.message.Add(msg[i].ToString());
        }
    }
}