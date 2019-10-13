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
    /// Allow time travel to be disabled
    /// </summary>
    public static bool TimeTravelDisabled { get; set; } = false; 

    /// <summary>
    /// Objects that should only appear in the past.
    /// </summary>
    private GameObject[] _pastOnlyObjects;

    /// <summary>
    /// Objects that should only appear in the future.
    /// </summary>
    private GameObject[] _futureOnlyObjects;

    /// <summary>
    /// Objects that should time travel, as described above.
    /// </summary>
    private GameObject[] _timeTravellingObjects;

    /// <summary>
    /// Copies of time travelling objects for the future.
    /// </summary>
    private GameObject[] _futureTimeTravellingObjects = new GameObject[0];

    // Start is called before the first frame update
    void Start()
    {
        IsInPast = false;

        if (_pastOnlyObjects == null)
        {
            _pastOnlyObjects = GameObject.FindGameObjectsWithTag("PastOnly");
        }

        if (_futureOnlyObjects == null)
        {
            _futureOnlyObjects = GameObject.FindGameObjectsWithTag("FutureOnly");
        }

        if(_timeTravellingObjects == null)
        {
            _timeTravellingObjects = GameObject.FindGameObjectsWithTag("TimeTravelling");
        }

        StartCoroutine(UpdateTimeTravelState(IsInPast));
    }

    // Update is called once per frame
    void Update()
    {
        // do time travel when they press x
        // TODO: use Input.GetButton instead of checking keys like this
        if (!_isTransitioning && !TimeTravelDisabled && Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(UpdateTimeTravelState(!IsInPast));
        }
    }

    private IEnumerator UpdateTimeTravelState(bool transitioningToPast)
    {
        _isTransitioning = true;

        var fadeInOut = GetComponent<FadeInOut>();

        fadeInOut.FadeOutToBlack();
        yield return new WaitForSeconds(fadeInOut._fadeDurationSeconds);

        // 1. If we are going to the future, save the state of all time travelling objects; if we are going to the future, restore them.
        if (transitioningToPast)
        {
            IsInPast = true;
            // Destroy future objects
            foreach (var toDestroy in _futureTimeTravellingObjects)
            {
                Destroy(toDestroy);
            }

            // re-enable past objects, which will be where they were in the past
            foreach (var objectToRestore in _timeTravellingObjects)
            {
                objectToRestore.SetActive(true);
            }

            // set background color
            Camera.main.backgroundColor = new Color(0.51f, 0.69f, 0.87f, 1f);
        } 
        else
        {
            IsInPast = false;
            _futureTimeTravellingObjects = new GameObject[_timeTravellingObjects.Length];

            for (int i = 0; i < _timeTravellingObjects.Length; ++i)
            {
                GameObject objectToSave = _timeTravellingObjects[i];

                GameObject cloneForFuture = Instantiate(objectToSave);
                _futureTimeTravellingObjects[i] = cloneForFuture;

                // disable past object.
                objectToSave.SetActive(false);
            }

            // set background color
             Camera.main.backgroundColor = new Color(0.60f, 0.63f, 0.82f, 1f);
        }


        // 2. Disable and enable objects that only appear in past and future.

        foreach (var x in _pastOnlyObjects)
        {
            x.SetActive(transitioningToPast);
        }

        foreach (var x in _futureOnlyObjects)
        {
            x.SetActive(!transitioningToPast);
        }

        // 3. Fast-forward physics if we are going into the future to simulate time-travel effect
        if(!transitioningToPast)
        {
            // Disable player movement so they don't speed away somewhere.
            CharacterController2D.MovementDisabled = true;
            Time.timeScale = 100;
            // Since timescale is 100x, 10 seconds is only 0.1secs which is not long!
            yield return new WaitForSeconds(10);
            Time.timeScale = 1;
            CharacterController2D.MovementDisabled = false;
        }

        fadeInOut.FadeInFromBlack();
        yield return new WaitForSeconds(fadeInOut._fadeDurationSeconds);

        _isTransitioning = false;
    }
}
