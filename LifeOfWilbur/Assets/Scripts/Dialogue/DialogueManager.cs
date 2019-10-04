using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text dialogueTextField;
    public Text nameTextField;
    public Text continueButtonField;
    public Animator animator;

    private Queue<string> sentences;

    public void Start() {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue) {
        StartCoroutine(startDialogueRoutine(dialogue));
    }

    IEnumerator startDialogueRoutine(Dialogue dialogue) {
        animator.SetBool("isOpen", true);
        sentences.Clear();

        yield return new WaitForSeconds(0.2f);
        continueButtonField.text = "Continue";
        nameTextField.text = dialogue.npcName;

        foreach (string sentence in dialogue.sentenceList) {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (sentences.Count > 0) {
            string sentence = sentences.Dequeue();
            StartCoroutine(TypeDialogueAnimation(sentence));
            if (sentences.Count == 0) {
                continueButtonField.text = "Close";
            }
        } else {
            EndDialogue();
        }
    }

    IEnumerator TypeDialogueAnimation(string text) {
        dialogueTextField.text = "";
        foreach (char character in text.ToCharArray()) {
            dialogueTextField.text += character;
            yield return null;
        }
    }

    private void EndDialogue() {
        continueButtonField.text = "";
        dialogueTextField.text = "";
        nameTextField.text = "";
        animator.SetBool("isOpen", false);
    }
}
