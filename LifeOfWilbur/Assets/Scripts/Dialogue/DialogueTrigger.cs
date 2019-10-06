using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;

    // Starts 
    public void TriggerDialogue() {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
