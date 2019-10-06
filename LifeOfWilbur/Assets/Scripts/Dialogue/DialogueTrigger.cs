using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;

    // Starts 
    public void TriggerDialogue() {
        DialogueController.Instance.StartDialogue(dialogue);
    }
}
