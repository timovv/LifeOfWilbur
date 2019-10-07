using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the logic behind the internal monolgue. Utilises collision detection and keypress checks to check if the tooltip and DialogueWindow should show
/// </summary>
public class WilburThought : MonoBehaviour
{
    /// <summary>
    /// Whether the player has already read the dialogue
    /// </summary>
    private bool _playerHasRead;

    /// <summary>
    /// The trigger which begins the conversation on the DialogueWindow. Also holds the actual conversation set in Unity.
    /// </summary>
    public DialogueTrigger _dialogueTrigger;

    /// <summary>
    /// Player has entered collision area and is now in range for starting dialogue
    /// </summary>
    /// <param name="other">The object which has entered the collision region</param>
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !_playerHasRead && !DialogueController.Instance.IsOpen) {
            _dialogueTrigger.TriggerDialogue();
            _playerHasRead = true;
        }
    }
}
