using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quote {

    public string name;

    [TextArea(3, 5)]
    public string quote;

}
