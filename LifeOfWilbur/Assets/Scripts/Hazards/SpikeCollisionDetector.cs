﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach on (player) objects to reset level on collisions with spikes.
/// </summary>
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
