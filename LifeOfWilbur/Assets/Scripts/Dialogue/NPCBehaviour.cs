using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public GameObject _player;

    private Transform _playerPosition;
    private Transform _NPCPosition;
    private SpriteRenderer _NPCSprite;

    // Start is called before the first frame update
    void Start()
    {
        _NPCPosition = gameObject.transform;
        _playerPosition = _player.transform;
        _NPCSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAt();
    }

    private void LookAt()
    {
        float xPos = _playerPosition.position.x - _NPCPosition.position.x;
        float yPos = _playerPosition.position.y - _NPCPosition.position.y;

        Vector2 compare = new Vector2(xPos, yPos).normalized;
        
        //Swap NPC to look at player at all times
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
