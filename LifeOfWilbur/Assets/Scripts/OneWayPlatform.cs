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

    public BoxCollider2D _selfWilburInRangeCollider;

    private bool _inRange;

    /// <summary>
    /// The one way platform's effector which allows the directional collisions
    /// </summary>
    public PlatformEffector2D _platformEffector;

    public GameObject _oldWilbur;
    public GameObject _youngWilbur;

    // Start is called before the first frame update
    void Start()
    {
        _oldWilbur = GameObject.Find("OldWilburPlaceholder");
        _youngWilbur = GameObject.Find("YoungWilburPlaceholder");

        // Informs physics engine to ignore collisions between Wilbur's box colliders and the one way platform's colliders. This prevents Wilbur from getting stuck in platform
        Physics2D.IgnoreCollision(_selfFloorCollider, _oldWilbur.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(_selfFloorCollider, _youngWilbur.GetComponent<BoxCollider2D>());
    }

    private void Update()
    {
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

    private IEnumerator FallThroughPlatform()
    {
        Physics2D.IgnoreCollision(_selfFloorCollider, _oldWilbur.GetComponent<CircleCollider2D>(), true);
        Physics2D.IgnoreCollision(_selfFloorCollider, _youngWilbur.GetComponent<CircleCollider2D>(), true);
        _platformEffector.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(_selfFloorCollider, _oldWilbur.GetComponent<CircleCollider2D>(), false);
        Physics2D.IgnoreCollision(_selfFloorCollider, _youngWilbur.GetComponent<CircleCollider2D>(), false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = true;
            Debug.Log("in range");
        }
    }

    /// <summary>
    /// Player has exited collision area and is now not in range for starting dialogue
    /// </summary>
    /// <param name="other">The object which has exited the collision region</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("out of range");
            _inRange = false;
        }

    }
}
