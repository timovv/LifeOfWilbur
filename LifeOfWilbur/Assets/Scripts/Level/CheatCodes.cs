using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines cheat codes for testers to get rhugh levels faster. The cheat codes are as follows:
///     Press P to go to the next room automatically
/// </summary>
public class CheatCodes : MonoBehaviour
{
    private bool _isSkipped;

    // Update is called once per frame
    void Update()
    {
        // Go to next level if "P" pressed
        if (Input.GetKeyDown(KeyCode.P) && !_isSkipped)
        {
            _isSkipped = true;

            // Re-enables movement/physcis in case player was in dialogue which locks them
            CharacterController2D.MovementDisabled = false; // enable Wilbur's movement
            TimeTravelController.TimeTravelDisabled = false; // enable Time Travel
            LevelReset.ResetDisabled = false; // enable resetting level
            Physics2D.autoSimulation = true; // enable physcis

            // Goes to next level
            StartCoroutine(GoToNextScene());
        }
    }

    /// <summary>
    /// Goes to the next room
    /// </summary>
    IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(0.7f); // Needed to prevent error (trying to delete something which does not exist) if P is spammed
        LifeOfWilbur.LevelController.NextRoom();
    }
}
