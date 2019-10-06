using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class IndicatorHighlight : MonoBehaviour
{
    /// <summary>
    /// True to highlight in the future, false to highlight if it is in the past.
    /// </summary>
    public bool _highlightInFuture;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        var outline = GetComponent<Outline>();
        if(_highlightInFuture != TimeTravelController.IsInPast)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }
}
