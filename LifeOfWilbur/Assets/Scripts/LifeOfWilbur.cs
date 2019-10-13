using UnityEngine;

public static class LifeOfWilbur
{
    public static GameController GameController 
    { 
        get 
        {
            return GameObject.Find("GameController").GetComponent<GameController>();
        }
    }
}
