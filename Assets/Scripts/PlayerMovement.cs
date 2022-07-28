using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float speed;
    public float acceleration;

    public enum movementType
    {
        oldMovement,
        newMovement,
        rbMovement,
    }
    public movementType currentMovementType;

    private void Update()
    {
        switch (currentMovementType)
        {
            case movementType.oldMovement: OldMovement(); break;
            case movementType.newMovement: NewMovement(); break;
        }
    }

    private void FixedUpdate()
    {
        rigidbody.simulated = currentMovementType == movementType.rbMovement;
        if (currentMovementType != movementType.rbMovement)
            return;

        Vector2 movement = new Vector2
        {
            x = (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f),
            y = (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f),
        }.normalized;
        rigidbody.velocity = Vector2.MoveTowards(rigidbody.velocity, movement * speed, acceleration * Time.deltaTime);

        if (rigidbody.velocity != Vector2.zero)
            transform.right = rigidbody.velocity;
    }

    private void OldMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector2 position = transform.position;
            position.y += speed * Time.deltaTime;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector2 position = transform.position;
            position.y -= speed * Time.deltaTime;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector2 position = transform.position;
            position.x += speed * Time.deltaTime;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector2 position = transform.position;
            position.x -= speed * Time.deltaTime;
            transform.position = position;
        }
    }
    private void NewMovement()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(xAxis, yAxis).normalized;

        move *= speed * Time.deltaTime;
        transform.position += (Vector3)move;
    }

    
}