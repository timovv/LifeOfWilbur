using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilburThought : MonoBehaviour
{
    private bool playerHasRead;
    public DialogueTrigger dialogueTrigger;

    // Player has entered collision area and is now in range for thought prompt
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !playerHasRead && !DialogueController.Instance.IsOpen) {
            dialogueTrigger.TriggerDialogue();
            playerHasRead = true;
        }
    }
}
