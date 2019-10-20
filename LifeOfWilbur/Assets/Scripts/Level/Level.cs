using System;


/// <summary>
/// Defines what a level is. A level is made up of a number of rooms. This is used to set room names from Unity.
/// </summary>
[Serializable]
public struct Level
{
    public Room[] _rooms;
}
