using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    
    private bool inRange;
    private bool playerHasRead;
    public DialogueTrigger dialogueTrigger;
    public Animator animator;


    private void Start() {
        playerHasRead = false;
        animator.SetBool("showToolTip", false);
    }

    // Opens the dialogueWindow if player in range (collision detection), user pressing "X" and dialogueWindow is not already open
    void Update() {
        if (!playerHasRead && inRange && Input.GetKeyDown(KeyCode.C) && !DialogueController.Instance.IsOpen) {
            dialogueTrigger.TriggerDialogue();
            playerHasRead = true;
            animator.SetBool("showToolTip", false);
        }
    }

    // Player has entered collision area and is now in range for starting dialogue
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            animator.SetBool("showToolTip", !playerHasRead);
            inRange = true;
        }
    }

    // Player has exited collision area and is not in range for starting dialogue
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            animator.SetBool("showToolTip", false);
            inRange = false;
        }
        
    }
}
