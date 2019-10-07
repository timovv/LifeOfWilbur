using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikeCollisionDetector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D otherObj)
    {
        if (otherObj.name == "Spikes")
        {
            FadeInOut script = GameObject.Find("LevelController").GetComponent<FadeInOut>();
            script.ReloadCurrentScene();
        }
    }
}
