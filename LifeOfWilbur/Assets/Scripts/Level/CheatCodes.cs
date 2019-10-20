using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines cheat codes for testers to get rhugh levels faster. The cheat codes are as follows:
///     Press P to go to the next room automatically
/// </summary>
public class CheatCodes : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // Go to next level if "P" pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Re-enables movement/physcis in case player was in dialogue which locks them
            CharacterController2D.MovementDisabled = false; // enable Wilbur's movement
            TimeTravelController.TimeTravelDisabled = false; // enable Time Travel
            LevelReset.ResetDisabled = false; // enable resetting level
            Physics2D.autoSimulation = true; // enable physcis
            
            // Goes to next level
            LifeOfWilbur.LevelController.NextRoom();
        }
    }
}
