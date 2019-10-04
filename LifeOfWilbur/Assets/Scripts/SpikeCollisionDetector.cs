using System;
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
            SceneManager.LoadScene(0); // temp, subscribe from a level controller thing instead
        }
    }
}
