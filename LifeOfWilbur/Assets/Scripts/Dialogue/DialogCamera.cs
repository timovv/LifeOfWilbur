using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCamera : MonoBehaviour
{
    /// <summary>
    /// The time taken to zoom in or zoom out
    /// </summary>
    private float _zoomDuration = 0.25f;

    /// <summary>
    /// The vertex multiplayer amount to zoom in and zoom out
    /// </summary>
    private float _zoomAmount = 2.5f; //Zoom in amount

    /// <summary>
    /// The original zoom level of the camera
    /// </summary>
    private float _originalZoomAmount;

    /// <summary>
    /// The position to zoom in to
    /// </summary>
    private Vector3 _zoomInPosition;

    /// <summary>
    /// The original position of the camera
    /// </summary>
    private Vector3 _originalPosition;

    /// <summary>
    /// Additional position offset to account for scene layout 
    /// </summary>
    private Vector3 _offsetPosition = new Vector3(0,0,0);

    /// <summary>
    /// The camera which must zoom in
    /// </summary>
    private Camera _camera;

    /// <summary>
    /// The position of future Wilbur
    /// </summary>
    private Transform _futureFocusObject;

    /// <summary>
    /// The position of past Wilbur
    /// </summary>
    private Transform _pastFocusObject; //Past wilbur

    /// <summary>
    /// Initialise the camera zoom in for dialogue. Must set futureFocusObject and pastFocusObject to zoom into the correct position. This should be set to the position of Wilbur.
    /// </summary>
    /// <param name="futureFocusObject">The position of future Wilbur</param>
    /// <param name="pastFocusObject">The position of past Wilbur</param>
    /// <param name="offsetPosition">Additional position offset to account for scene layout </param>
    public void initialize(Transform futureFocusObject, Transform pastFocusObject, Vector3 offsetPosition)
    {
        _camera = Camera.main;
        _originalPosition = _camera.transform.position;
        _originalZoomAmount = _camera.orthographicSize;
        _offsetPosition = offsetPosition;

        _futureFocusObject = futureFocusObject;
        _pastFocusObject = pastFocusObject;
    }

    /// <summary>
    /// Camera zoom in
    /// </summary> 
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

    /// <summary>
    /// Camera zoom out
    /// </summary>
    public void ZoomOutFocus()
    {
        StartCoroutine(resizeRoutine(_originalPosition, _zoomInPosition + _offsetPosition, _originalZoomAmount, _zoomAmount));
    }

    /// <summary>
    /// Routine to animate the zoom in/out of the camrea
    /// </summary>
    /// <param name="toPosition">the camera position to zoom in to</param>
    /// <param name="fromPosition">the original postion of the camera</param>
    /// <param name="toZoom">the zoom level at the end of the effect</param>
    /// <param name="fromZoom">the initial zoom level at the start of the effect</param>
    /// <returns></returns>
    private IEnumerator resizeRoutine(Vector3 toPosition, Vector3 fromPosition, float toZoom, float fromZoom)
    {
        float elapsed = 0;
        Vector3 targetPosition = new Vector3(toPosition.x, toPosition.y, -10); // Position to goto

        while (elapsed <= _zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _zoomDuration);

            _camera.transform.position = Vector3.Lerp(fromPosition, targetPosition, t); // x,y position of camera change
            _camera.orthographicSize = Mathf.Lerp(fromZoom, toZoom, t); // "z" position of camera change
            yield return null;
        }
    }
}
