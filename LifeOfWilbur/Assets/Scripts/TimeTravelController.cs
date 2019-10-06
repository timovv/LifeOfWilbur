using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelController : MonoBehaviour
{
    public bool isInPast = false;

    private GameObject[] pastObjects;
    private GameObject[] futureObjects;

    // Start is called before the first frame update
    void Start()
    {
        if(pastObjects == null) {
            pastObjects = GameObject.FindGameObjectsWithTag("PastOnly");
        }

        if(futureObjects == null) {
            futureObjects = GameObject.FindGameObjectsWithTag("FutureOnly");
        }

        UpdateObjects(isInPast);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) {
            isInPast = !isInPast;
            UpdateObjects(isInPast);
        }
    }

    private void UpdateObjects(bool past) {
        foreach(var x in pastObjects) {
            x.SetActive(past);
        }

        foreach(var x in futureObjects) {
            x.SetActive(!past);
        }
    }
}
