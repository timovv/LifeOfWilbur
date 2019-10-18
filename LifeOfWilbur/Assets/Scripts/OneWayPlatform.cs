using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{

    public BoxCollider2D _selfCollider;

    public PlatformEffector2D _platformEffector;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("OldWilburPlaceholder").GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(_selfCollider, GameObject.Find("OldWilburPlaceholder").GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(_selfCollider, GameObject.Find("YoungWilburPlaceholder").GetComponent<BoxCollider2D>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _platformEffector.rotationalOffset = 180f;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            _platformEffector.rotationalOffset = 0;
        }
    }
}
