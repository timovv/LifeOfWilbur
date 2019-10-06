using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelReset : MonoBehaviour
{
    private bool disable;

    // Start is called before the first frame update
    void Start()
    {
        // Persist this script across the scene change
        DontDestroyOnLoad(gameObject);   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !disable) //TODO: change to button, not keycode
        {
            disable = true;
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
        }
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        // Need to persist the black canvas too
        GameObject timecontroller = GameObject.Find("TimeTravelController");
        DontDestroyOnLoad(timecontroller);
        FadeInOut script = timecontroller.GetComponent<FadeInOut>();

        yield return StartCoroutine(script.FadeOutToBlack());
        SceneManager.LoadScene(sceneIndex);
        yield return StartCoroutine(script.FadeInFromBlack());

        // Don't need anymore
        Destroy(timecontroller);
        Destroy(gameObject); 
    }
}
