using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float _intensity; //Level of screenshake (recommend 0.1-0.3)

    private Transform _target;
    private Vector3 _initialPosition;
    private float _pendingShakeDuration = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _target = GetComponent<Transform>();
        _initialPosition = _target.localPosition;
    }

    private void Update()
    {
        if (_pendingShakeDuration > 0)
        {
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
