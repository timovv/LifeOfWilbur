using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCamera : MonoBehaviour
{
    private float _zoomDuration = 0.25f; //Time of zoom in

    private float _zoomAmount = 2.5f; //Zoom in amount
    private float _originalZoomAmount;

    private Vector3 _zoomInPosition;
    private Vector3 _originalPosition;
    private Vector3 _offsetPosition = new Vector3(0,0,0);

    private Camera _camera;
    private Transform _futureFocusObject; //Future wilbur
    private Transform _pastFocusObject; //Past wilbur

    public void initialize(Transform futureFocusObject, Transform pastFocusObject, Vector3 offsetPosition)
    {
        _camera = Camera.main;
        _originalPosition = _camera.transform.position;
        _originalZoomAmount = _camera.orthographicSize;
        _offsetPosition = offsetPosition;

        _futureFocusObject = futureFocusObject;
        _pastFocusObject = pastFocusObject;
    }

    //Camera zoom in 
    public void ZoomInFocus()
    {
        if (_futureFocusObject.gameObject.activeInHierarchy)
        {
            _zoomInPosition = _futureFocusObject.position;
        } else
        {
            _zoomInPosition = _pastFocusObject.position;
        }

        StartCoroutine(resizeRoutine(_zoomInPosition + _offsetPosition, _originalPosition, _zoomAmount, _originalZoomAmount));
    }

    //Camera zoom out
    public void ZoomOutFocus()
    {
        StartCoroutine(resizeRoutine(_originalPosition, _zoomInPosition + _offsetPosition, _originalZoomAmount, _zoomAmount));
    }

    private IEnumerator resizeRoutine(Vector3 toPosition, Vector3 fromPosition, float toZoom, float fromZoom)
    {
        float elapsed = 0;
        Vector3 targetPosition = new Vector3(toPosition.x, toPosition.y, -10); //Position to goto

        while (elapsed <= _zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _zoomDuration);

            _camera.transform.position = Vector3.Lerp(fromPosition, targetPosition, t); //x,y position of camera change
            _camera.orthographicSize = Mathf.Lerp(fromZoom, toZoom, t);//"z" position of camera change
            yield return null;
        }
    }
}
