using System.Collections.Generic;
// class that handles json key value
public class JsonKey
{
    public string objectName;
    public string[] elementsKeys;
    public List<JsonKey> elementsjsonKey;

    public JsonKey(string objName, string[] elmtKeys, List<JsonKey> elmtjsKeys)
    {
        this.objectName = objName;
        this.elementsKeys = elmtKeys;
        this.elementsjsonKey = elmtjsKeys;
        // Debug.Log(elementsKeys[0]);
    }
    override public string ToString()
    {
        string keyString = objectName + " containing \n";
        foreach (string key in elementsKeys)
            keyString += key + "\n";
        if (elementsjsonKey != null)
            foreach (JsonKey jskey in elementsjsonKey)
                keyString += jskey.ToString();
        return keyString;
    }
    public bool equal(JsonKey obj)
    {
        if (this.objectName == obj.objectName)
            if (this.elementsKeys.Length == obj.elementsKeys.Length)
            {
                for (int i = 0; i < this.elementsKeys.Length; i++)
                    if (this.elementsKeys[i] != obj.elementsKeys[i])
                        return false;
                if (this.elementsjsonKey == null && obj.elementsjsonKey == null)
                    return true;
                if (this.elementsjsonKey.Count == obj.elementsjsonKey.Count)
                    for (int i = 0; i < this.elementsjsonKey.Count; i++)
                        if (!this.elementsjsonKey[i].equal(obj.elementsjsonKey[i]))
                            return false;
                return true;
            }
        return false;
    }
}