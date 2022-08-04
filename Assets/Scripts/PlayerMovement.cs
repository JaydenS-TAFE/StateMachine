using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;

    public enum MovementType
    {
        oldMovement,
        newMovement,
        rbMovement,
    }
    public MovementType currentMovementType;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch (currentMovementType)
        {
            case MovementType.oldMovement: OldMovement(); break;
            case MovementType.newMovement: NewMovement(); break;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.simulated = currentMovementType == MovementType.rbMovement;
        if (currentMovementType != MovementType.rbMovement)
            return;

        Vector2 movement = new Vector2
        {
            x = (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f),
            y = (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f),
        }.normalized;
        _rigidbody.velocity = Vector2.MoveTowards(_rigidbody.velocity, movement * _speed, _acceleration * Time.deltaTime);

        if (_rigidbody.velocity != Vector2.zero)
            transform.right = _rigidbody.velocity;
    }

    private void OldMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector2 position = transform.position;
            position.y += _speed * Time.deltaTime;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector2 position = transform.position;
            position.y -= _speed * Time.deltaTime;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector2 position = transform.position;
            position.x += _speed * Time.deltaTime;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector2 position = transform.position;
            position.x -= _speed * Time.deltaTime;
            transform.position = position;
        }
    }
    private void NewMovement()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(xAxis, yAxis).normalized;

        move *= _speed * Time.deltaTime;
        transform.position += (Vector3)move;
    }

    
}