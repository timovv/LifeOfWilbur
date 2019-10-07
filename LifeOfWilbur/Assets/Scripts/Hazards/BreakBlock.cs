using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the behaviour and render animator controller for a breakable block.
/// 
/// The breakable block is a hazard mechanic; when the player walks over the top of the block,
/// the block will break within a set timer given from the field "delayTime"
/// 
/// - animator: handles setting of the boolean "IsBreak" in order to tell the animator controller
///     that the player has walked over a breakable block, and a timer should start to "break" 
/// - delayTime: sets the length of how long the breakable block will dissapear once the player steps on the block
/// 
/// </summary>
public class BreakBlock : MonoBehaviour
{
    /// <summary>
    /// gets the animator to handle the animator controller states
    /// </summary>
    public Animator _animator;

    /// <summary>
    /// time till break block on player collision
    /// </summary>
    public float _delayTime;

    //On player collision of the top of the breakable block 
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //If collision is Player -> start breaking
        if (collision.gameObject.tag == "Player")
        {
            _animator.SetBool("IsBreak", true);
            StartCoroutine(HideObject(gameObject, _delayTime));
        }
    }

    //Hide the object upon delayTime number of seconds
    IEnumerator HideObject(GameObject gameObject, float delayTime)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
    }
}
