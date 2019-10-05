using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelController : MonoBehaviour
{
    /// <summary>
    /// Whether the level is currently in the past or in the future.
    /// </summary>
    public bool _isInPast = false;

    private bool _isTransitioning = false;


    private GameObject[] pastObjects;
    private GameObject[] futureObjects;
    private GameObject[] timeTravellingObjects;

    private GameObject[] futureTimeTravellingObjects = new GameObject[0];

    // Start is called before the first frame update
    void Start()
    {
        if (pastObjects == null)
        {
            pastObjects = GameObject.FindGameObjectsWithTag("PastOnly");
        }

        if (futureObjects == null)
        {
            futureObjects = GameObject.FindGameObjectsWithTag("FutureOnly");
        }

        if(timeTravellingObjects == null)
        {
            timeTravellingObjects = GameObject.FindGameObjectsWithTag("TimeTravelling");
        }

        StartCoroutine(UpdateTimeTravelState(_isInPast, fade: false));
    }

    // Update is called once per frame
    void Update()
    {
        // do time travel when they press x
        if (!_isTransitioning && Input.GetKeyDown(KeyCode.X))
        {
            _isInPast = !_isInPast;
            StartCoroutine(UpdateTimeTravelState(_isInPast, fade: true));
        }
    }

    private IEnumerator UpdateTimeTravelState(bool past, bool fade)
    {
        _isTransitioning = true;

        var fadeInOut = GetComponent<FadeInOut>();
        // Fade out for transition
        if (fade)
        {
            yield return StartCoroutine(fadeInOut.FadeOutToBlack());
        }

        // 1. If we are going to the future, save the state of all time travelling objects; if we are going to the future, restore them.
        if(past)
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

        foreach (var x in pastObjects)
        {
            x.SetActive(past);
        }

        foreach (var x in futureObjects)
        {
            x.SetActive(!past);
        }

        // 3. Fast-forward physics if we are going into the future to simulate time-travel effect
        if(!past)
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
