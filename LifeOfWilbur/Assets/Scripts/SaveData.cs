using UnityEngine;

public class SaveData
{
    public static SaveData Instance
    {
        get
        {
            return _instance ?? (_instance = new SaveData());
        }
    }

    public int HighestUnlockedLevel
    {
        get
        {
            return PlayerPrefs.GetInt(HIGHEST_UNLOCKED_LEVEL_KEY, 0);
        }
        
        set
        {
            PlayerPrefs.SetInt(HIGHEST_UNLOCKED_LEVEL_KEY, value);
        }
    }

    public bool UnlockedSpeedRunMode
    {
        get
        {
            return PlayerPrefs.GetInt(UNLOCKED_SPEEDRUN_MODE_KEY, 0) != 0;
        }

        set
        {
            PlayerPrefs.SetInt(UNLOCKED_SPEEDRUN_MODE_KEY, value ? 1 : 0);
        }
    }

    private static SaveData _instance;
    
    private const string HIGHEST_UNLOCKED_LEVEL_KEY = "HIGHEST_UNLOCKED_LEVEL";
    private const string UNLOCKED_SPEEDRUN_MODE_KEY = "UNLOCKED_SPEEDRUN_MODE";
}
