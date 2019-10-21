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
            StartCoroutine(PauseMovementOnDeath());
            LifeOfWilbur.LevelController.ResetLevel();
        }
    }

    /// <summary>
    /// Pauses movement, kills, and then reenables. This is so Wilbur can't keep moving after he dies.
    /// </summary>
    IEnumerator PauseMovementOnDeath()
    {
        DisableMovement();
        yield return new WaitForSeconds(0.1f);
        EnableMovement();
    }


    /// <summary>
    /// Disables movement and physics
    /// </summary>
    void DisableMovement()
    {
        CharacterController2D.MovementDisabled = true; // disable Wilbur's movement
        TimeTravelController.TimeTravelDisabled = true; // disable Time Travel
        LevelReset.ResetDisabled = true; // disable resetting level
        Physics2D.autoSimulation = false; // disable physics
    }

    /// <summary>
    /// Ren-enables movement and physics
    /// </summary>
    void EnableMovement()
    {
        CharacterController2D.MovementDisabled = false; // enable Wilbur's movement
        TimeTravelController.TimeTravelDisabled = false; // enable Time Travel
        LevelReset.ResetDisabled = false; // enable resetting level
        Physics2D.autoSimulation = true; // enable physcis
    }
}
