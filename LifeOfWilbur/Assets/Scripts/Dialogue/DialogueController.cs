using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the dialogue conversation and its display on the dialogue window in the game.
/// 
/// Singleton is used to ensure a singular popup window exsits and seamless conversation.
/// 
/// Allows for the quotes to be displayed and allows the player to request the next line in the conversation.
///  - Dialogue quotes are displayed character by character to provide an animated effect to the user and aid readability.
///  - Sets the quote-speaker's name on the dialogue window, and sets the int associated with each character to display image 
/// 
/// </summary>
public class DialogueController : MonoBehaviour
{

    /// <summary>
    /// Holds the singleton instance of the DialogueController class
    /// </summary>
    public static DialogueController Instance { get; private set; }

    /// <summary>
    /// Boolean storing the state of the DialogueWindow popup. True if it is showing and false if it is not showing.
    /// </summary>
    public Boolean IsOpen { get; private set; }

    /// <summary>
    /// The GUI entry holding the current dialogue quote spoken by a character
    /// </summary>
    public TextMeshProUGUI _dialogueTextField;

    /// <summary>
    /// The speaker of the current dialogue quote
    /// </summary>
    public TextMeshProUGUI _nameTextField;

    /// <summary>
    /// The text field in the GUI inside the continue button. This allows the text to be reassigned to "close" when no more slides exist
    /// </summary>
    public TextMeshProUGUI _continueButtonTextField;

    /// <summary>
    /// The animator used animate the appearance of the DialogueWindow
    /// </summary>
    public Animator _animator;

    /// <summary>
    /// Dictionary storing the mapping between input quotes speakers to number. The number is used in the animator to control which speaker animation plaus
    /// </summary>
    private Dictionary<string, int> _characterMapper;

    /// <summary>
    /// The queue of quotes to play in the current conversation
    /// </summary>
    private Queue<Quote> _quoteQueue;

    /// <summary>
    /// Stores the state of if a line of text is currently being printed in the TypeDialogueAnimation routine
    /// </summary>
    private bool _textAnimating;

    //DialogCamera object
    private DialogCamera _dialogCamera;
    public Transform _futureFocusObject;
    public Transform _pastFocusObject;

    private bool _isBold;

    // Singleton design pattern used for DialogueManager because only one dialogueWindow can be open at a time
    void Awake()
    {
        Instance = this;
    }

    // Populates the dictionary with the preset mappings of character to integers.
    public void Start()
    {
        //Create main camera dialog object with object of focus
        _dialogCamera = gameObject.AddComponent<DialogCamera>();
        _dialogCamera.initialize(_futureFocusObject, _pastFocusObject);

        _quoteQueue = new Queue<Quote>();
        _characterMapper = new Dictionary<string, int> {
            { "Wilbur", 1 }, //Polar bear
            { "Iris", 2 }, //Fox
            { "Simon", 3 }, //Tern
            { "Willy", 4 }, //Whale
        };
    }

    // If player presses c button, will display the next sentence
    void Update()
    {
        if (IsOpen && Input.GetKeyDown(KeyCode.C))
        {
            DisplayNextSentence();
        }
    }

    // Adds all quotes to the queue and opens dialogueWindow
    public void StartDialogue(Dialogue dialogue)
    {
        _dialogCamera.ZoomInFocus();

        _quoteQueue.Clear();
        foreach (Quote quote in dialogue._quoteList)
        {
            _quoteQueue.Enqueue(quote);
        }
        StartCoroutine(StartDialogueRoutine(dialogue));
    }

    /// <summary>
    /// Opens and populates the dialogueWindow and disables movement and physcis to prevent character from moving during speed-up
    /// </summary>
    /// <param name="dialogue">The dialogue instance which needs to be printed</param>
    /// <returns></returns>
    IEnumerator StartDialogueRoutine(Dialogue dialogue)
    {
        _animator.SetBool("isOpen", true);

        Debug.Log("Poped up!!");
        FindObjectOfType<AudioManager>().Pause("SnowWalkTrimmed"); //Pause Sound movement
        //CharacterController2D.MovementDisabled = true; // disable Wilbur's movement
        TimeTravelController.TimeTravelDisabled = true; // disable Time Travel
        LevelReset.ResetDisabled = true; // disable resetting level
        Physics2D.autoSimulation = false; // disable physics
        

        _isBold = false;

        // Waits 0.2f seconds to ensure the dialogueWindowOpen animation has completed before populating the dialogueWindow 
        yield return new WaitForSeconds(0.2f);
        _continueButtonTextField.text = "Press C to continue";
        DisplayNextSentence();
    }

    /// <summary>
    /// Displays the next quote and populates the necessary text information
    /// </summary>
    public void DisplayNextSentence()
    {
        _isBold = false;
        if (_textAnimating)
        {
            // If already printing a line then sets it to false - this makes TypeDialogueAnimation routine print entire line
            _textAnimating = false;
        }
        else
        {
            if (_quoteQueue.Count > 0)
            {
                Quote quote = _quoteQueue.Dequeue();

                // Sets the name property and changes the image animation for the quote speaker
                _nameTextField.text = quote._name;
                int characterInteger;
                _characterMapper.TryGetValue(quote._name, out characterInteger);
                _animator.SetInteger("characterInteger", characterInteger);
                _animator.SetBool("isFuture", quote._isFuture);

                // Sets the dialogue through the animation
                StartCoroutine(TypeDialogueAnimation(quote._quote));

                // Sets the "Continue" button text to "Close" if it is on the last quote
                if (_quoteQueue.Count == 0)
                {
                    _continueButtonTextField.text = "Press C to close";
                }
                IsOpen = true;
            }
            else
            {
                // No more quotes to display so ends the dialogue
                EndDialogue();
            }
        }
    }

    /// <summary>
    /// Coroutine which animates the appearance of the quote text
    /// </summary>
    /// <param name="text">The line to print onto the DialogueWindow</param>
    /// <param name="quotePrintingIndex">The current line in the conversation that is being printed</param>
    /// <returns></returns>
    IEnumerator TypeDialogueAnimation(string text)
    {
        _textAnimating = true;
        _dialogueTextField.text = "";
        foreach (char character in text.ToCharArray())
        {
            if (_textAnimating == true)
            {
                if (character == '*')
                {
                    _isBold = !_isBold;
                    if (_isBold == true)
                    {
                        _dialogueTextField.text += "<b></b>";
                    }
                }
                else
                {
                    if (_isBold)
                    {
                        _dialogueTextField.text = _dialogueTextField.text.Substring(0, _dialogueTextField.text.Length - 4);
                        _dialogueTextField.text += character + "</b>";
                    }
                    else
                    {
                        _dialogueTextField.text += character;
                    }
                }
                yield return null;
            }
            else
            {
                // If _textAnimating variable set to false, will print the entire line at once and end animation
                string formattedText = "";
                _isBold = false;
                foreach (char fullTextCharacter in text.ToCharArray())
                {
                    if (fullTextCharacter == '*')
                    {
                        if (_isBold)
                        {
                            formattedText += "</b>";
                        }
                        else
                        {
                            formattedText += "<b>";
                        }
                        _isBold = !_isBold;
                    }
                    else
                    {
                        formattedText += fullTextCharacter;
                    }
                }
                _dialogueTextField.text = formattedText;
                break;
            }
        }
        _textAnimating = false;
    }

    /// <summary>
    /// Unassigns all text entries in the dialogueWindow and hides it. Restarts the movement and the physcis
    /// </summary>
    private void EndDialogue()
    {
        _dialogCamera.ZoomOutFocus();

        //CharacterController2D.MovementDisabled = false; // enable Wilbur's movement
        TimeTravelController.TimeTravelDisabled = false; // enable Time Travel
        LevelReset.ResetDisabled = false; // enable resetting level
        Physics2D.autoSimulation = true; // enable physcis
        _continueButtonTextField.text = "";
        _dialogueTextField.text = "";
        _nameTextField.text = "";
        _animator.SetBool("isOpen", false);
        IsOpen = false;
    }
}
