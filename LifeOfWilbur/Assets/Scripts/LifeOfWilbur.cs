using UnityEngine;

public static class LifeOfWilbur
{
    public static GameController GameController 
    { 
        get 
        {
            return GameObject.Find("GameController")?.GetComponent<GameController>();
        }
    }

    public static ILevelController LevelController
    {
        get
        {
            if(GameController != null)
            {
                return GameController;
            } 
            else
            {
                return GameObject.Find("TestLevelController").GetComponent<TestLevelController>();
            }
        }
    }
}
