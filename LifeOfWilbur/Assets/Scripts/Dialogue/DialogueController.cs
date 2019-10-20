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

    /// <summary>
    /// Instance of DialogCamera object, used to move/transform the camera on dialogue starting and ending
    /// </summary>
    private DialogCamera _dialogCamera;

    /// <summary>
    /// Additional offset for the camera to make when starting conversation. This iss used if default transform position clips notable objects
    /// </summary>
    public Vector3 _offsetPosition = new Vector3(0,0,0);

    /// <summary>
    /// Position of the object which needs to be put into focus in the future
    /// </summary>
    public Transform _futureFocusObject;

    /// <summary>
    /// Position of the object which needs to be put into focus in the past
    /// </summary>
    public Transform _pastFocusObject;

    /// <summary>
    /// Maintains the state of if dialogue is bolding the words being printed
    /// </summary>
    private bool _isBold;

    // Singleton design pattern used for DialogueManager because only one dialogueWindow can be open at a time
    void Awake()
    {
        Instance = this;
    }

    // Populates the dictionary with the preset mappings of character to integers.
    public void Start()
    {
        // Create main camera dialog object with object of focus
        _dialogCamera = gameObject.AddComponent<DialogCamera>();
        _dialogCamera.initialize(_futureFocusObject, _pastFocusObject, _offsetPosition);

        _quoteQueue = new Queue<Quote>();

        // Initialises the mapper of character names to integers. This is used to set the profile animation in the dialogue window
        InitaliseCharacterMapper();
    }


    /// <summary>
    /// Initialises the mapper of character names to integers. This is used to set the profile animation in the dialogue window
    /// </summary>
    private void InitaliseCharacterMapper()
    {
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

    /// <summary>
    /// Adds all quotes to the queue and opens dialogueWindow
    /// </summary>
    /// <param name="dialogue">The dialogue conversation which is displayed to the player</param>
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
        IsOpen = true;
        _animator.SetBool("isOpen", true);

        FindObjectOfType<AudioManager>().Pause("SnowWalkTrimmed"); //Pause Sound movement
        //CharacterController2D.MovementDisabled = true; // disable Wilbur's movement
        TimeTravelController.TimeTravelDisabled = true; // disable Time Travel
        LevelReset.ResetDisabled = true; // disable resetting level
        Physics2D.autoSimulation = false; // disable physics        

        // Waits 0.2f seconds to ensure the dialogueWindowOpen animation has completed before populating the dialogueWindow 
        yield return new WaitForSeconds(0.2f);
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
        else if (_quoteQueue.Count > 0)
        {
            /// Takes next quote out of the queue. This is what is currently being printed
            Quote quote = _quoteQueue.Dequeue();

            // Sets the name property and changes the image animation for the quote speaker
            _nameTextField.text = quote._name;
            _characterMapper.TryGetValue(quote._name, out int characterInteger);
            _animator.SetInteger("characterInteger", characterInteger);
            _animator.SetBool("isFuture", quote._isFuture);

            // Sets the dialogue through the animation
            StartCoroutine(TypeDialogueAnimation(quote._quote));
        }
        else
        {
            // No more quotes to display so ends the dialogue
            EndDialogue();
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
                PrintNextCharacter(character);
                yield return null; // Wait one frame and then continue
            }
            else
            {
                PrintEntireQuote(text); // Print entire conversation because user wants to go to next slide
                break;
            }
        }
        _textAnimating = false;
    }

    /// <summary>
    /// Prints the next singular character onto the dialogue window
    /// </summary>
    /// <param name="c">The character to add to the dialogue window</param>
    private void PrintNextCharacter(char c)
    {
        if (c == '*')
        {
            // If bold then will print opening and closing bold tag. This is to avoid text unclosed tag syntax errors
            _isBold = !_isBold;
            if (_isBold)
            {
                _dialogueTextField.text += "<b></b>";
            }
        }
        else
        {
            // If bold then must remove </b>, add character and re-add </b> to avoid text unclosed tag syntax errors
            _dialogueTextField.text = _isBold ? RemoveClosingBTagFromDialogue() + c + "</b>" : _dialogueTextField.text + c;
        }
    }

    /// <summary>
    /// Returns dialogue text with the last 4 characters removed. The four characters represent the </b> in the text.
    /// </summary>
    /// <returns>String without </b></returns>
    private string RemoveClosingBTagFromDialogue()
    {
        return _dialogueTextField.text.Substring(0, _dialogueTextField.text.Length - 4);
    }

    /// <summary>
    /// Prints the enture line onto the dialogue window
    /// </summary>
    /// <param name="text">The line being printed to the dialogue window</param>
    private void PrintEntireQuote(string text)
    {
        // If _textAnimating variable set to false, will print the entire line at once and end animation
        string formattedText = "";
        _isBold = false;
        foreach (char fullTextCharacter in text.ToCharArray())
        {
            if (fullTextCharacter == '*')
            {
                // Prints correct <b> or </b> tag depending on _isBold state
                formattedText += _isBold ? "</b>" : "<b>";
                _isBold = !_isBold;
            }
            else
            {
                formattedText += fullTextCharacter;
            }
        }
        _dialogueTextField.text = formattedText;
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
        _dialogueTextField.text = "";
        _nameTextField.text = "";
        _animator.SetBool("isOpen", false);
        IsOpen = false;
    }
}
