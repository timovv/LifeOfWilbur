using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    
    private bool inRange;
    public DialogueTrigger dialogueTrigger;

    // Opens the dialogueWindow if player in range (collision detection), user pressing "X" and dialogueWindow is not already open
    void Update() {
        if (inRange && Input.GetKeyDown(KeyCode.X) && !DialogueManager.Instance.IsOpen) {
            dialogueTrigger.TriggerDialogue();
        }
    }

    // Player has entered collision area and is now in range for starting dialogue
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            inRange = true;
        }
    }

    // Player has exited collision area and is not in range for starting dialogue
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            inRange = false;
        }
        
    }
}
