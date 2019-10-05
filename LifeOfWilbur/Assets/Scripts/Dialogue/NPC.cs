using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    
    private bool inRange;
    public DialogueTrigger dialogueTrigger;

    // Update is called once per frame
    void Update() {
        if (inRange && Input.GetKeyDown(KeyCode.X) && !DialogueManager.isOpen) {
            dialogueTrigger.TriggerDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            inRange = false;
        }
        
    }
}
