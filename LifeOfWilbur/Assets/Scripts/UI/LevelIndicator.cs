using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIndicator : MonoBehaviour
{
    //TODO: Potentially delete the animator

    //public Animator _animator;
    public GameObject _levelIndicatorPanel;

    // Start is called before the first frame update
    void Start()
    {
        _levelIndicatorPanel.SetActive(true);
        //_animator.SetBool("IsHidden", false);
    }

   public void Hide()
    {
        Debug.Log("Hide Run");
       // _animator.SetBool("IsHidden", true);
    }


    public void HidePanel()
    {
        Debug.Log("Hide Panel Run");
        //StartCoroutine(WaitAndHidePanel());
    }


    public IEnumerator WaitAndHidePanel()
    {
        Debug.Log("Hide wait");
        yield return new WaitForSeconds(15);
        _levelIndicatorPanel.SetActive(false);
    }

}
