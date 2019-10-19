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
    public BoxCollider2D _selfFloorCollider;

    /// <summary>
    /// The collider which is checked to ensure Wilbur is in range to fall through platform
    /// </summary>
    public BoxCollider2D _selfWilburInRangeCollider;

    /// <summary>
    /// Boolean maintaining whether Wilbur is in range to fall through platform
    /// </summary>
    private bool _inRange;

    /// <summary>
    /// The one way platform's effector which allows the directional collisions
    /// </summary>
    public PlatformEffector2D _platformEffector;

    /// <summary>
    /// Wilbur's for updating stored in fields for efficient referencing
    /// </summary>
    private GameObject _oldWilbur;
    private GameObject _youngWilbur;

    // Start is called before the first frame update
    void Awake()
    {
        _oldWilbur = GameObject.Find("OldWilburPlaceholder");
        _youngWilbur = GameObject.Find("YoungWilburPlaceholder");
    }

    private void Update()
    {
        // Informs physics engine to ignore collisions between Wilbur's box colliders and the one way platform's colliders. This prevents Wilbur from getting stuck in platform
        Physics2D.IgnoreCollision(_selfFloorCollider, _oldWilbur.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(_selfFloorCollider, _youngWilbur.GetComponent<BoxCollider2D>());
        
        // If down or s is pressed, flips the platform effector. This allows Wilbur to fall through the spike
        if (_inRange && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            StartCoroutine(FallThroughPlatform());
        }
        // If down or s key is released, straightens the platform effector to 0. This sets the effector back to default one way platform behaviour.
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            _platformEffector.rotationalOffset = 0;
        }
    }

    // Allows Wilbur to fall through platform
    private IEnumerator FallThroughPlatform()
    {
        // Disables Wilbur's circle collider so he doesn't get stuck in the platform
        Physics2D.IgnoreCollision(_selfFloorCollider, _oldWilbur.GetComponent<CircleCollider2D>(), true);
        Physics2D.IgnoreCollision(_selfFloorCollider, _youngWilbur.GetComponent<CircleCollider2D>(), true);
        
        // Rotates effector whcih allows Wilbur to fall through platform
        _platformEffector.rotationalOffset = 180f;

        // Waits 0.5 seconds and then re-enables the colliders for next use
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(_selfFloorCollider, _oldWilbur.GetComponent<CircleCollider2D>(), false);
        Physics2D.IgnoreCollision(_selfFloorCollider, _youngWilbur.GetComponent<CircleCollider2D>(), false);
    }

    // Player has entered collision area and is now in range for falling through platform
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = true;
        }
    }

    // Player has exited collision area and is now not in range for falling through platform
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = false;
        }

    }
}
