using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    public float moveDistance = 1f; // Distance Pac-Man moves per key press


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.right; // Initial Direction to the right.
    }

    void Move()
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveDistance * Time.fixedDeltaTime));
    }

    private void FixedUpdate()
    {
    //    Move();
    }

    private void Update()
    {
        Vector2 newPosition = transform.position;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            newPosition += Vector2.up * moveDistance;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            newPosition += Vector2.down * moveDistance;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newPosition += Vector2.left * moveDistance;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            newPosition += Vector2.right * moveDistance;
        }

        transform.position = newPosition;
    }

  
}



/*
public class Player : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction.y == 0)
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction.y == 0)
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction.x == 0)
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction.x == 0)
        {
            direction = Vector2.right;
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = movement * speed;
    }
}

*/