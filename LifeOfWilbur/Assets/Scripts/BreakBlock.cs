using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    public Animator animator;
    public float delayTime; //Num seconds

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Wilbur")
        {
            animator.SetBool("IsBreak", true);
            StartCoroutine(HideObject(gameObject, delayTime));
        }
    }

    IEnumerator HideObject(GameObject gameObject, float delayTime)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
    }
}
