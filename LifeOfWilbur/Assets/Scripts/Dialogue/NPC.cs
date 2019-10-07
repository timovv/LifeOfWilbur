using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the logic behind the NPC sprites. Utilises collision detection and keypress checks to check if the tooltip and DialogueWindow should show
/// </summary>
public class NPC : MonoBehaviour {
    
    /// <summary>
    /// Whether the player is in range
    /// </summary>
    private bool _inRange;

    /// <summary>
    /// Whether the player has already read the dialogue
    /// </summary>
    private bool _playerHasRead;

    /// <summary>
    /// The trigger which begins the conversation on the DialogueWindow. Also holds the actual conversation set in Unity.
    /// </summary>
    public DialogueTrigger _dialogueTrigger;

    /// <summary>
    /// The animator used to control the display of the tooltip
    /// </summary>
    public Animator _animator;


    private void Start() {
        _playerHasRead = false;
        _animator.SetBool("showToolTip", false);
    }

    /// <summary>
    /// Opens the dialogueWindow if player in range (collision detection), user pressing "X" and dialogueWindow is not already open
    /// </summary>
    void Update() {
        if (!_playerHasRead && _inRange && Input.GetKeyDown(KeyCode.C) && !DialogueController.Instance.IsOpen) {
            _dialogueTrigger.TriggerDialogue();
            _playerHasRead = true;
            _animator.SetBool("showToolTip", false);
        }
    }

    /// <summary>
    /// Player has entered collision area and is now in range for starting dialogue
    /// </summary>
    /// <param name="other">The object which has entered the collision region</param>
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _animator.SetBool("showToolTip", !_playerHasRead);
            _inRange = true;
        }
    }

    /// <summary>
    /// Player has exited collision area and is now not in range for starting dialogue
    /// </summary>
    /// <param name="other">The object which has exited the collision region</param>
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _animator.SetBool("showToolTip", false);
            _inRange = false;
        }
        
    }
}
