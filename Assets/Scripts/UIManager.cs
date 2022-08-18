using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;

    private float _maxHealth = 100f;
    private float _health;


    private void Start()
    {
        _health = _maxHealth;
        ModifyHealth(0f);
    }

    private void ModifyHealth(float amount)
    {
        _health = Mathf.Clamp(_health + amount, 0f, _maxHealth);
        _healthText.text = $"{Mathf.Ceil(_health)}";
    }

    public void Damage()
    {
        ModifyHealth(-10f);
    }
    public void Heal()
    {
        ModifyHealth(10f);
    }
}
