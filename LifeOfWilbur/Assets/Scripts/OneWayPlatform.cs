using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One way platform script that allows Wilbur to jump through the one way platform onto the top.
/// Script required to ignore one way platform's collisions with Wilbur's box collider and prevents Wilbur from clipping and getting stuck in the middle of the platform.
/// Also allows for fall through platform on key press functionality.
/// </summary>
public class OneWayPlatform : MonoBehaviour
{

    /// <summary>
    /// The one way platform's collider which Wilbur jumps and lands onto
    /// </summary>
    public BoxCollider2D _selfCollider;

    /// <summary>
    /// The one way platform's effector which allows the directional collisions
    /// </summary>
    public PlatformEffector2D _platformEffector;

    // Start is called before the first frame update
    void Start()
    {
        // Informs physics engine to ignore collisions between Wilbur's box colliders and the one way platform's colliders. This prevents Wilbur from getting stuck in platform
        Physics2D.IgnoreCollision(_selfCollider, GameObject.Find("OldWilburPlaceholder").GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(_selfCollider, GameObject.Find("YoungWilburPlaceholder").GetComponent<BoxCollider2D>());
    }

    private void Update()
    {
        // If down or s is pressed, flips the platform effector. This allows Wilbur to fall through the spike
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            _platformEffector.rotationalOffset = 180f;
        }
        // If down or s key is released, straightens the platform effector to 0. This sets the effector back to default one way platform behaviour.
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            _platformEffector.rotationalOffset = 0;
        }
    }
}
