using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelIndicator : MonoBehaviour
{
    //TODO: Potentially delete the animator

    //public Animator _animator;
    public GameObject _levelIndicatorPanel;
    public TextMeshProUGUI _levelText;

    // Start is called before the first frame update
    void Start()
    {
        _levelIndicatorPanel.SetActive(true);
        //_animator.SetBool("IsHidden", false);
    }


    public IEnumerator SetUpPanel(string levelText)
    {
        SetLevelText(levelText);
        yield return new WaitForSeconds(15);
        HidePanel();
    }

    private void HidePanel()
    {
        _levelIndicatorPanel.SetActive(false);
    }

    private void SetLevelText(string levelText)
    {
        _levelText.SetText(levelText);
    }

}
