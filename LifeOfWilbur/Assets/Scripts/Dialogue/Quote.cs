using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Quote is a singular line spoken during a dialogue conversation.
/// 
/// It holds the speaker and the comment spoken by the speaker.
/// </summary>
[System.Serializable]
public class Quote {

    /// <summary>
    /// The speaker of the quote
    /// </summary>
    public string _name;


    /// <summary>
    /// The line which is spoken by the speaker
    /// </summary>
    [TextArea(3, 5)]
    public string _quote;

}
