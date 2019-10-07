using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The conversation which is had with the player.
/// </summary>
[System.Serializable]
public class Dialogue {

    /// <summary>
    /// Dialogue is made up of list of quotes. This allows the conversation text to be set nicely in Unity
    /// </summary>
    public Quote[] _quoteList;

}
