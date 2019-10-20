using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For any "death" objects - put this onto Camera and drag the camera onto Wilbur's "death" object scripts
/// </summary>
public class ScreenShake : MonoBehaviour
{
    /// <summary>
    /// Level of screenshake (recommend 0.1-0.2)
    /// </summary>
    public float _intensity;

    /// <summary>
    /// The position of the player on death
    /// </summary>
    private Transform _target;

    /// <summary>
    /// The initial vector position of the player on death
    /// </summary>
    private Vector3 _initialPosition;

    /// <summary>
    /// Duration of shake (recommend 0.2)
    /// </summary>
    private float _pendingShakeDuration = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        // Assigns the death location/vector of the platform
        _target = GetComponent<Transform>();
        _initialPosition = _target.localPosition;
    }

    private void Update()
    {
        if (_pendingShakeDuration > 0)
        {
            // Starts DoShake of rotine
            StartCoroutine(DoShake());
        }
    }

    public void Shake(float duration)
    {
        if (duration > 0)
        {
            _pendingShakeDuration += duration;
        }
    }

    /// <summary>
    /// The routine of shaking the screen
    /// </summary>
    IEnumerator DoShake()
    {
        //Do shake
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + _pendingShakeDuration)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(-1*_intensity, _intensity), 
                Random.Range(-1 * _intensity, _intensity), 
                _initialPosition.z);

            _target.localPosition = randomPoint;
            yield return null;
        }

        //Reset shake properties
        _pendingShakeDuration = 0f;
        _target.localScale = _initialPosition;
    }
}
