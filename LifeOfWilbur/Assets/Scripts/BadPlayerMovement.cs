using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// we're not actually using this, please

public class BadPlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

        // Todo should use Input.GetAxis();
        if(Input.GetKey(KeyCode.D)) {
            velocity += Vector2.right * 5 * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.A)) {
            velocity += Vector2.left * 5 * Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            velocity += Vector2.up * 5;
        }

        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
