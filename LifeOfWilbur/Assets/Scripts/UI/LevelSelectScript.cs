using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelectScript : MonoBehaviour
{
    public GameObject _levelHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectLevel(""); 
    }

    private void SelectLevel(string levelName)
    {
        SceneManager.LoadScene(1);

        //Setting the GamerTime back to 0 in the case of restarting the game
        GameTimer.ElapsedTimeSeconds = 0;
    }
}
