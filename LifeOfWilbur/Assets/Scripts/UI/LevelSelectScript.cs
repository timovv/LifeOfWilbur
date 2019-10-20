using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour
{
    public Camera _mainCamera;
    public float _smoothTime;
    public float _newOrthographic;

    private Vector3 _originalPosition;
    private float _originalOrthographic;

    private void Start()
    {
        _originalPosition = _mainCamera.transform.position;
        _originalOrthographic = _mainCamera.orthographicSize;
    }

    public void PlayLevel(string levelName)
    {
        LifeOfWilbur.GameController.StartGameAt(GameMode.Story, levelName);
        GameTimer.ElapsedTimeSeconds = 0;
    }

    public void SelectLevel(Transform target)
    {
        //Selected animation
        Animator animator = target.GetComponent<Animator>();
        animator.SetBool("isSelected", true);

        //Resize camera on click
        StartCoroutine(resizeRoutine(target));
    }

    //Duplicate code - help
    public void DeselectLevel(Transform target)
    {
        //Selected animation
        Animator animator = target.GetComponent<Animator>();
        animator.SetBool("isSelected", false);

        StartCoroutine(deresizeRoutine()); 
    }

    public void GoBackButtonClick()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private IEnumerator resizeRoutine(Transform target)
    {
        float elapsed = 0;
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10); //Position to goto

        while (elapsed <= _smoothTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _smoothTime);

            _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, targetPosition, t); //x,y position of camera change
            _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, _newOrthographic, t);//"z" position of camera change
            yield return null;
        }
    }

    private IEnumerator deresizeRoutine()
    {
        float elapsed = 0;

        while (elapsed <= _smoothTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _smoothTime);

            _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, _originalOrthographic, t);//"z" position of camera change
            _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, _originalPosition, t); //x,y position of camera change
            yield return null;
        }
    }
}
