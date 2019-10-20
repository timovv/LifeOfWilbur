using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the direction which Wilbur faces. The NPC will always face towards the player.
/// </summary>
public class NPCBehaviour : MonoBehaviour
{

    /// <summary>
    /// The player which the NPC must face towards
    /// </summary>
    public GameObject _player;

    /// <summary>
    /// Information about the player position and the direction the player is facing
    /// </summary>
    private Transform _playerPosition;

    /// <summary>
    /// Information about the npc position and the direction the npc is facing
    /// </summary>
    private Transform _NPCPosition;

    /// <summary>
    /// The sprite's renderer for the npc
    /// </summary>
    private SpriteRenderer _NPCSprite;

    // Start is called before the first frame update
    void Start()
    {
        // Assigns the transform and spriterenderer fields to the current gameobjects and the npc's object component
        _NPCPosition = gameObject.transform;
        _playerPosition = _player.transform;
        _NPCSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the sprite needs to be flipped every update
        LookAt();
    }

    private void LookAt()
    {
        // determine the relative position between the player and the npc
        float xPos = _playerPosition.position.x - _NPCPosition.position.x;
        float yPos = _playerPosition.position.y - _NPCPosition.position.y;
        Vector2 compare = new Vector2(xPos, yPos).normalized;
        

        // Swap NPC to look at player at all times
        if (compare.x >= -1.0f && compare.x <= 0)
        {
            _NPCSprite.flipX = false;
        }
        else if (compare.x <= 1.0f && compare.x > 0) 
        {
            _NPCSprite.flipX = true;
        }
    }
}
