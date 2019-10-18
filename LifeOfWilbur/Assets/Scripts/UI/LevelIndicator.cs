using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelIndicator : MonoBehaviour
{
    public GameObject _levelIndicatorPanel;
    public TextMeshProUGUI _levelText;
    public CanvasGroup _canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        //Ensure that the panel is activated and is visible at startup
        _levelIndicatorPanel.SetActive(true);
        _canvasGroup.alpha = 1;
    }

    IEnumerator StartFade()
    {
        while(_canvasGroup.alpha > 0) 
        {
            _canvasGroup.alpha -= Time.deltaTime  / 2;
            yield return null;
        }
        _canvasGroup.interactable = false;
        yield return null;
    }


    public IEnumerator SetUpPanel(string levelText)
    {
        SetLevelText(levelText);
        yield return new WaitForSeconds(12);
        StartCoroutine(StartFade());
        StartCoroutine(HidePanel());
    }

    private IEnumerator HidePanel()
    {
        yield return new WaitForSeconds(15);
        _levelIndicatorPanel.SetActive(false);
    }

    private void SetLevelText(string levelText)
    {
        _levelText.SetText(levelText);
    }

}
