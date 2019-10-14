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

    void OnTriggerEnter2D(Collider2D otherObj)
    {
        if (otherObj.name == "Spikes")
        {
            screenShake.Shake(duration);
            _animator.SetTrigger("isDeath");
            StartCoroutine(PauseMovementOnDeath());
            FadeInOut script = GameObject.Find("LevelController").GetComponent<FadeInOut>();
            script.ReloadCurrentScene();
        }
    }

    IEnumerator PauseMovementOnDeath()
    {
        DisableMovement();
        yield return new WaitForSeconds(1.0f);
        EnableMovement();
    }


    void DisableMovement()
    {
        CharacterController2D.MovementDisabled = true; // disable Wilbur's movement
        TimeTravelController.TimeTravelDisabled = true; // disable Time Travel
        LevelReset.ResetDisabled = true; // disable resetting level
        Physics2D.autoSimulation = false; // disable physics
    }

    void EnableMovement()
    {
        CharacterController2D.MovementDisabled = false; // enable Wilbur's movement
        TimeTravelController.TimeTravelDisabled = false; // enable Time Travel
        LevelReset.ResetDisabled = false; // enable resetting level
        Physics2D.autoSimulation = true; // enable physcis
    }
}
