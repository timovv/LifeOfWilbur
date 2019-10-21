using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach on (player) objects to reset level on collisions with spikes.
/// </summary>
public class SpikeCollisionDetector : MonoBehaviour
{
    public Animator _animator;

    public ScreenShake screenShake;
    public float duration;

    /// <summary>
    /// If spike or water in collision regions, will shake screen, and kill player through PauseMovementOnDeath routine
    /// </summary>
    /// <param name="otherObj">The object which Wilbur collides with</param>
    void OnTriggerEnter2D(Collider2D otherObj)
    {
        if (otherObj.name == "Spikes" || otherObj.name == "Water")
        {
            screenShake.Shake(duration);
            FindObjectOfType<AudioManager>().Play("DeathSFX");
            _animator.SetTrigger("isDeath");
            LifeOfWilbur.LevelController.ResetLevel();
        }
    }
}
