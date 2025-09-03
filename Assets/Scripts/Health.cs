using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    float _currentHealth;

    [SerializeField] float _maxHealth = 3f;

    //Old health value / New health value
    public event Action<float,float> OnHealthChange;
    public event Action OnDeath;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage (float damage)
    {
        float newHealth = Mathf.Max(_currentHealth - damage, 0f);
        OnHealthChange?.Invoke(_currentHealth, newHealth);
        _currentHealth = newHealth;

        if (_currentHealth < 0f)
        {
            OnDeath?.Invoke();
        }
        Debug.Log($"Damage on {gameObject.name}, current health : {_currentHealth}");
    }

    public void Heal(float healValue)
    {
        float newHealth = Mathf.Min(_currentHealth + healValue, _maxHealth);
        OnHealthChange?.Invoke(_currentHealth, newHealth);
        _currentHealth = newHealth;
    }
}
