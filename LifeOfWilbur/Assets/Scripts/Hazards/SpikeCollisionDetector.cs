using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikeCollisionDetector : MonoBehaviour
{
    public event Action TouchedSpike; // Access with Find and GetComponent

    void OnTriggerEnter2D(Collider2D otherObj)
    {
        if (otherObj.name == "Spikes")
        {
            TouchedSpike?.Invoke(); // Call listeners (if any)
            StartCoroutine(ResetScene());
        }
    }

    IEnumerator ResetScene()
    {
        FadeInOut script = GameObject.Find("LevelController").GetComponent<FadeInOut>();

        script.FadeOutToBlack();
        yield return new WaitForSeconds(script._fadeDurationSeconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
