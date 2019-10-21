using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script for the level selecter screen in story mode
///     Controls the zooming in/out of a level and starting the level
///     Controls the navigation back to the menu scene
/// </summary>
public class LevelSelectScript : MonoBehaviour
{
    public Camera _mainCamera;
    public float _smoothTime;
    public float _newOrthographic;

    private Vector3 _originalPosition;
    private float _originalOrthographic;

    public GameObject[] _levelButtons;

    private void Start()
    {
        // Stores the default position of the camera as reference point to go back to during animations
        _originalPosition = _mainCamera.transform.position;
        _originalOrthographic = _mainCamera.orthographicSize;
    }

    private void Update()
    {
        for(int i = 0; i < _levelButtons.Length; ++i)
        {
            var button = _levelButtons[i].GetComponent<Button>();
            var sprite = _levelButtons[i].transform.Find("Text").transform.Find("LevelBase").GetComponent<SpriteRenderer>();
            var animator = _levelButtons[i].transform.Find("Text").transform.Find("LevelBase").GetComponent<Animator>();

            if(SaveData.Instance.HighestUnlockedLevel >= i + 1)
            {
                button.interactable = true;
                sprite.enabled = true;
                animator.enabled = true;
            }
            else
            {
                button.interactable = false;
                sprite.enabled = false;
                animator.enabled = false;
            }
        }
    }

    /// <summary>
    /// Loads the game in story mode when user clicks play button
    /// </summary>
    /// <param name="levelName"></param>
    public void PlayLevel(string levelName)
    {
        LifeOfWilbur.GameController.StartGameAt(GameMode.Story, levelName);
        GameTimer.ElapsedTimeSeconds = 0;
    }

    /// <summary>
    /// When user clicks on a level, will zoom camera into the target transform positions
    /// </summary>
    /// <param name="target">The position to place the camera</param>
    public void SelectLevel(Transform target)
    {
        //Selected animation
        Animator animator = target.GetComponent<Animator>();
        animator.SetBool("isSelected", true);

        //Resize camera on click
        StartCoroutine(resizeRoutine(target));
    }

    /// <summary>
    /// When user clicks back from a level to deselct it, will zoom camera back to stored default positions
    /// </summary>
    /// <param name="target">The level being unselected</param>
    public void DeselectLevel(Transform target)
    {
        //Selected animation
        Animator animator = target.GetComponent<Animator>();
        animator.SetBool("isSelected", false);

        StartCoroutine(deresizeRoutine()); 
    }

    /// <summary>
    /// Controls the loading of the menu scene if back button pressed
    /// </summary>
    public void GoBackButtonClick()
    {
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// Zooms into the level
    /// </summary>
    /// <param name="target">The position of the level on the screen</param>
    /// <returns></returns>
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
    /// <summary>
    /// Zooms out of the level
    /// </summary>
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
