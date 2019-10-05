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
    public static Boolean isOpen;

    private int dialogueQuoteIndex = 0;
    private Queue<Quote> quoteQueue;

    public void Start() {
        quoteQueue = new Queue<Quote>();
    }

    public void StartDialogue(Dialogue dialogue) {
        isOpen = true;
        dialogueQuoteIndex = 0;
        StartCoroutine(startDialogueRoutine(dialogue));
    }

    IEnumerator startDialogueRoutine(Dialogue dialogue) {
        animator.SetBool("isOpen", true);
        quoteQueue.Clear();

        yield return new WaitForSeconds(0.2f);
        continueButtonField.text = "Continue";

        foreach (Quote quote in dialogue.quoteList) {
            quoteQueue.Enqueue(quote);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        dialogueQuoteIndex += 1;
        if (quoteQueue.Count > 0) {
            Quote quote = quoteQueue.Dequeue();
            
            nameTextField.text = quote.name;
            animator.SetBool("isWilbur", quote.name == "CubWilbur");


            StartCoroutine(TypeDialogueAnimation(quote.quote, dialogueQuoteIndex));
            
            if (quoteQueue.Count == 0) {
                continueButtonField.text = "Close";
            }
        } else {
            EndDialogue();
        }
    }

    IEnumerator TypeDialogueAnimation(string text, int quotePrintingIndex) {
        dialogueTextField.text = "";
        foreach (char character in text.ToCharArray()) {
            if (dialogueQuoteIndex == quotePrintingIndex) {
                dialogueTextField.text += character;
                yield return null;
            }
        }
    }

    private void EndDialogue() {
        continueButtonField.text = "";
        dialogueTextField.text = "";
        nameTextField.text = "";
        animator.SetBool("isOpen", false);
        isOpen = false;
    }
}
