using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The trigger which begins the dialogue conversation on the DialogueController
/// This class is required to allow the text to be set in the DialogueTrigger componenet in unity.
/// </summary>
public class DialogueTrigger : MonoBehaviour {

    /// <summary>
    /// The dialogue conversation which needs to be printed
    /// </summary>
    public Dialogue _dialogue;

    /// <summary>
    /// Starts the dialogue context
    /// </summary>
    public void TriggerDialogue() {
        DialogueController.Instance.StartDialogue(_dialogue);
    }
}
