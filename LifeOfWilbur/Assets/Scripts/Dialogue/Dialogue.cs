using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

    public string npcName;

    [TextArea(3,5)]
    public string[] sentenceList;


}
