using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelIndicator : MonoBehaviour
{
    private const double FADETIME_COEFFICIENT = 0.5;

    public GameObject _levelIndicatorPanel;
    public TextMeshProUGUI _levelText;
    public CanvasGroup _canvasGroup;

    void Awake()
    {
        //Ensure that the panel is activated and is visible at startup
        _levelIndicatorPanel.SetActive(true);
        _canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Method thats starts the fading of the canvas group. Reduces the alpha of the canvas group based on the game
    /// time. Note there is a fade time coefficient that can be modified to change the speed of the fade.
    /// </summary>
    /// <returns></returns>
    IEnumerator StartFade()
    {
        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha -= (float)(Time.deltaTime / FADETIME_COEFFICIENT);
            yield return null; //Waiting for the next frame
        }
        _canvasGroup.interactable = false; //Ensure that everything in the canvas is no longer interactable
        yield return null;
    }


    public IEnumerator SetUpPanel(string levelText)
    {
        Debug.Log("Setup Panel called!");
        SetLevelText(levelText); //Preset the text for the levels
        yield return new WaitForSecondsRealtime(3); //Display the text for a bit and then start the fade and the hiding of the panel
        StartCoroutine(StartFade());
        StartCoroutine(HidePanel());
    }

    /// <summary>
    /// Deactivate the panel from the scene. It is already faded out at this point, this coroutine ensures that the panel
    /// is no longer present in the scene
    /// </summary>
    /// <returns></returns>
    private IEnumerator HidePanel()
    {
        yield return new WaitForSecondsRealtime(5); // Wait for the fade animation to finish before deactivating the panel
        _levelIndicatorPanel.SetActive(false);
    }

    /// <summary>
    /// Setting the text of the level. This was made private as to abstract this from other methods being able to set the text,
    /// without ensuruing that the panel will fade and hide. 
    /// </summary>
    /// <param name="levelText"></param>
    private void SetLevelText(string levelText)
    {
        _levelText.SetText(levelText);
    }


    public void TogglePanelVisibility(bool value)
    {
        Debug.Log("Visibility called");
        _levelIndicatorPanel.SetActive(value);
    }

}
