using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Level controller for testing individuals levels. Not used in actual gameplay.
/// </summary>
[RequireComponent(typeof(TimeTravelController))]
public class TestLevelController : MonoBehaviour, ILevelController
{
    public void Awake()
    {
        if(LifeOfWilbur.GameController != null)
        {
            // if there's an actual game controller, then don't worry about this.
            Destroy(gameObject);
        } 
        else
        {
            StartCoroutine(GetComponent<TransitionController>().FadeInFromBlack());
            GetComponent<TimeTravelController>().UpdateTimeTravelState(true);
        }
    }

    public void NextLevel()
    {
        Debug.Log("If this was a real playthrough, you would have completed this level!");
    }

    public void ResetLevel()
    {
        // no fancy transitions here, just reload the scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}