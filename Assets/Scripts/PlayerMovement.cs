using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _staminaText;
    [SerializeField] private Image _staminaBarImage;
    [SerializeField] private Image _staminaBarValueImage;

    [Header("Misc")]
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _maxStamina;
    private float _stamina;

    private bool _hasDashed;

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
            case MovementType.rbMovement:
                if (Input.GetKeyDown(KeyCode.LeftShift))
                    _hasDashed = true;
                break;
        }
    }

    private void FixedUpdate()
    {
        _stamina = Mathf.MoveTowards(_stamina, _maxStamina, Time.fixedDeltaTime);

        _rigidbody.simulated = currentMovementType == MovementType.rbMovement;
        if (currentMovementType != MovementType.rbMovement)
            return;

        Vector2 movement = new Vector2
        {
            x = (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f),
            y = (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f),
        }.normalized;
        _rigidbody.velocity = Vector2.MoveTowards(_rigidbody.velocity, movement * _speed, _acceleration * Time.fixedDeltaTime);

        if (_hasDashed)
        {
            if (_stamina >= 1f && movement != Vector2.zero)
            {
                _rigidbody.velocity += movement * _dashForce;
                _stamina -= 1f;
            }
            _hasDashed = false;
        }

        if (_rigidbody.velocity != Vector2.zero)
            transform.right = _rigidbody.velocity;

        _staminaText.text = $"{System.Math.Round(_stamina, 1)}";

        _staminaBarValueImage.rectTransform.localScale = new Vector3(
            _staminaBarImage.rectTransform.localScale.x * (_stamina / _maxStamina),
            _staminaBarValueImage.rectTransform.localScale.y,
            _staminaBarValueImage.rectTransform.localScale.z
        );
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