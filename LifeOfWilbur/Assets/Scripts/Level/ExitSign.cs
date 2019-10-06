using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Behaviour for an exit sign.
/// When both past and future Wilburs (GameObjects tagged with Player) are at the exit sign,
/// the script will trigger a scene change.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class ExitSign : MonoBehaviour
{
    /// <summary>
    /// True if the old Wilbur is at the exit.
    /// </summary>
    public static bool IsOldWilburAtExit { get; private set; }

    /// <summary>
    /// True if the younger Wilbur is at the exit.
    /// </summary>
    public static bool IsYoungWilburAtExit { get; private set; }

    /// <summary>
    /// Name for the next scene.
    /// </summary>
    public string _nextSceneName;

    private GameObject _youngWilburIndicator;
    private GameObject _oldWilburIndicator;

    // Start is called before the first frame update
    void Start()
    {
        IsOldWilburAtExit = false;
        IsYoungWilburAtExit = false;

        _youngWilburIndicator = transform.Find("YoungWilburIndicator").gameObject;
        _oldWilburIndicator = transform.Find("OldWilburIndicator").gameObject;
        _youngWilburIndicator.SetActive(false);
        _oldWilburIndicator.SetActive(false);
    }

    /// <summary>
    /// Updates player-at-exit states when they enter/exit the collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(TimeTravelController.IsInPast)
            {
                IsYoungWilburAtExit = true;
                _youngWilburIndicator.SetActive(true);
            }
            else
            {
                IsOldWilburAtExit = true;
                _oldWilburIndicator.SetActive(true);
            }
        }

        if(IsYoungWilburAtExit && IsOldWilburAtExit)
        {
            // we're done
            StartCoroutine(ExitLevel());
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (TimeTravelController.IsInPast)
            {
                IsYoungWilburAtExit = false;
                _youngWilburIndicator.SetActive(false);
            }
            else
            {
                IsOldWilburAtExit = false;
                _oldWilburIndicator.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Coroutine to fade out level and transition to the specified scene.
    /// </summary>
    /// <returns>Enumerator for coroutine</returns>
    private IEnumerator ExitLevel()
    {
        yield return StartCoroutine(FindObjectOfType<FadeInOut>().FadeOutToBlack());
        SceneManager.LoadScene(_nextSceneName, LoadSceneMode.Single);
    }
}
