using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{

    public BoxCollider2D _selfCollider;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("OldWilburPlaceholder").GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(_selfCollider, GameObject.Find("OldWilburPlaceholder").GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(_selfCollider, GameObject.Find("YoungWilburPlaceholder").GetComponent<BoxCollider2D>());
    }
}
