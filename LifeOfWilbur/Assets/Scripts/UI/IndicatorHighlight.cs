using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script controls the highlighting of the wilbur indicator in the bottom right hand side of the screen.
/// </summary>
[RequireComponent(typeof(Outline))]
public class IndicatorHighlight : MonoBehaviour
{
    /// <summary>
    /// True to highlight in the future, false to highlight if it is in the past.
    /// </summary>
    public bool _highlightInFuture;

    // Update is called once per frame
    void Update()
    {
        var outline = GetComponent<Outline>();
        
        // gets the time the player is currently in to know whether to enable/disable
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
