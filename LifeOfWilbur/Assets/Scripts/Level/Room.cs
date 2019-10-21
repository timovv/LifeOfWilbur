using System;

/// <summary>
/// Defines the makeup of a Room. A room consists of a readable name which is displayed to the user on room load, a file name and a boolean for if playable in speedrun mode. 
/// </summary>
[Serializable]
public struct Room
{
    /// <summary>
    /// The readable name of the room, to be displayed on room loading
    /// </summary>
    public string _readableName;

    /// <summary>
    /// The file name of the room's scene
    /// </summary>
    public string _sceneName;

    /// <summary>
    /// Whether the room will appear in the speedrun game mode
    /// </summary>
    public bool _playInSpeedRunMode;
}