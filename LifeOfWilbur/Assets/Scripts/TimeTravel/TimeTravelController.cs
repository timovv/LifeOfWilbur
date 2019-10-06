using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour that controls the time travel mechanic seen in Life of Wilbur.
/// 
/// The mechanic allows for objects to appear only in the past, only in the future, or to
/// be allowed to travel between past and future. How the time travel controller affects
/// objects in the scene is affected by their tag:
/// 
/// - Objects with the PastOnly tag will only appear while the level is in the past.
/// - Objects with the FutureOnly tag will only appear while the level is in the future.
/// - Objects with the TimeTravel tag will travel back and forward in time. When travelling to the future,
///     the object is copied, the original object is disabled, and the level is fast-forwarded to simulate
///     time travel. When returning to the past, the original object is reenabled and the copy destroyed.
///     This causes the object to appear to return to its original position before the time travel happened.
/// </summary>
public class TimeTravelController : MonoBehaviour
{
    /// <summary>
    /// Whether the level is currently in the past or in the future.
    /// </summary>
    public static bool IsInPast { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    private bool _isTransitioning = false;

    /// <summary>
    /// Objects that should only appear in the past.
    /// </summary>
    private GameObject[] pastOnlyObjects;

    /// <summary>
    /// Objects that should only appear in the future.
    /// </summary>
    private GameObject[] futureOnlyObjects;

    /// <summary>
    /// Objects that should time travel, as described above.
    /// </summary>
    private GameObject[] timeTravellingObjects;

    /// <summary>
    /// Copies of time travelling objects for the future.
    /// </summary>
    private GameObject[] futureTimeTravellingObjects = new GameObject[0];

    // Start is called before the first frame update
    void Start()
    {
        IsInPast = false;

        if (pastOnlyObjects == null)
        {
            pastOnlyObjects = GameObject.FindGameObjectsWithTag("PastOnly");
        }

        if (futureOnlyObjects == null)
        {
            futureOnlyObjects = GameObject.FindGameObjectsWithTag("FutureOnly");
        }

        if(timeTravellingObjects == null)
        {
            timeTravellingObjects = GameObject.FindGameObjectsWithTag("TimeTravelling");
        }

        StartCoroutine(UpdateTimeTravelState(IsInPast, fade: false));
    }

    // Update is called once per frame
    void Update()
    {
        // do time travel when they press x
        // TODO: use Input.GetButton instead of checking keys like this
        if (!_isTransitioning && Input.GetKeyDown(KeyCode.X))
        {
            IsInPast = !IsInPast;
            StartCoroutine(UpdateTimeTravelState(IsInPast, fade: true));
        }
    }

    private IEnumerator UpdateTimeTravelState(bool transitioningToPast, bool fade)
    {
        _isTransitioning = true;

        var fadeInOut = GetComponent<FadeInOut>();
        // Fade out for transition
        if (fade)
        {
            yield return StartCoroutine(fadeInOut.FadeOutToBlack());
        }

        // 1. If we are going to the future, save the state of all time travelling objects; if we are going to the future, restore them.
        if(transitioningToPast)
        {
            // Destroy future objects
            foreach (var toDestroy in futureTimeTravellingObjects)
            {
                Destroy(toDestroy);
            }

            // re-enable past objects, which will be where they were in the past
            foreach (var objectToRestore in timeTravellingObjects)
            {
                objectToRestore.SetActive(true);
            }
        } 
        else
        {
            futureTimeTravellingObjects = new GameObject[timeTravellingObjects.Length];

            for (int i = 0; i < timeTravellingObjects.Length; ++i)
            {
                GameObject objectToSave = timeTravellingObjects[i];

                GameObject cloneForFuture = Instantiate(objectToSave);
                futureTimeTravellingObjects[i] = cloneForFuture;

                // disable past object.
                objectToSave.SetActive(false);
            }
        }


        // 2. Disable and enable objects that only appear in past and future.

        foreach (var x in pastOnlyObjects)
        {
            x.SetActive(transitioningToPast);
        }

        foreach (var x in futureOnlyObjects)
        {
            x.SetActive(!transitioningToPast);
        }

        // 3. Fast-forward physics if we are going into the future to simulate time-travel effect
        if(!transitioningToPast)
        {
            Time.timeScale = 100;
            // Since timescale is 100x, 10 seconds is only 0.1secs which is not long!
            yield return new WaitForSeconds(10);
            Time.timeScale = 1;
        }

        if (fade)
        {
            yield return StartCoroutine(fadeInOut.FadeInFromBlack());
        }

        _isTransitioning = false;
    }
}
