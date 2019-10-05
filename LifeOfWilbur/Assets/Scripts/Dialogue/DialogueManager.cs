using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager Instance { get; private set; }
    public Boolean IsOpen { get; private set; }

    public Text dialogueTextField;
    public Text nameTextField;
    public Text continueButtonTextField;
    public Animator animator;

    private Queue<Quote> quoteQueue;

    // Singleton design pattern used for DialogueManager because only one dialogueWindow can be open at a time
    void Awake() {
        Instance = this;
    }

    public void Start() {
        quoteQueue = new Queue<Quote>();
    }

    void Update() {
        if (IsOpen && Input.GetKeyDown(KeyCode.C)) {
            DisplayNextSentence();
        }
    }

    // Adds all quotes to the queue and opens dialogueWindow
    public void StartDialogue(Dialogue dialogue) {
        quoteQueue.Clear();
        foreach (Quote quote in dialogue.quoteList) {
            quoteQueue.Enqueue(quote);
        }

        StartCoroutine(StartDialogueRoutine(dialogue));
    }

    // Opens and populates the dialogueWindow
    IEnumerator StartDialogueRoutine(Dialogue dialogue) {
        animator.SetBool("isOpen", true);

        // Waits 0.2f seconds to ensure the dialogueWindowOpen animation has completed before populating the dialogueWindow 
        yield return new WaitForSeconds(0.2f);
        continueButtonTextField.text = "Press C to continue";
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (quoteQueue.Count > 0) {
            Quote quote = quoteQueue.Dequeue();
            
            // Sets the name property and changes the image animation for the quote speaker
            nameTextField.text = quote.name;
            animator.SetBool("isWilbur", quote.name == "Cub Wilbur" || quote.name == "Adult Wilbur");

            // Sets the dialogue through the animation
            StartCoroutine(TypeDialogueAnimation(quote.quote, quoteQueue.Count));

            // Sets the "Continue" button text to "Close" if it is on the last quote
            if (quoteQueue.Count == 0) {
                continueButtonTextField.text = "Press C to continue";
            }
            IsOpen = true;
        } else {
            // No more quotes to display so ends the dialogue
            EndDialogue();
        }
    }

    // Coroutine which animates the appearance of the quote text
    IEnumerator TypeDialogueAnimation(string text, int quotePrintingIndex) {
        dialogueTextField.text = "";
        foreach (char character in text.ToCharArray()) {
            // Will only animate the next character if the user is still on the same quote slide
            if (quoteQueue.Count == quotePrintingIndex) {
                dialogueTextField.text += character;
                yield return null;
            }
        }
    }

    // Unassigns all text entries in the dialogueWindow and hides it
    private void EndDialogue() {
        continueButtonTextField.text = "";
        dialogueTextField.text = "";
        nameTextField.text = "";
        animator.SetBool("isOpen", false);
        IsOpen = false;
    }
}
