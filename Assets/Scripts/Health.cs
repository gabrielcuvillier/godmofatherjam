using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    float _currentHealth;

    [SerializeField] float _maxHealth = 3f;

    //Old health value / New health value
    public event Action<float,float> OnHealthChange;
    public event Action OnDeath;

    [SerializeField] UnityEvent _onHealthChangeEvent;
    [SerializeField] UnityEvent _onDeathEvent;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage (float damage)
    {
        float newHealth = Mathf.Max(_currentHealth - damage, 0f);
        OnHealthChange?.Invoke(_currentHealth, newHealth);
        _onHealthChangeEvent.Invoke();
        _currentHealth = newHealth;

        if (_currentHealth < 0f)
        {
            OnDeath?.Invoke();
            _onDeathEvent.Invoke();
        }
        Debug.Log($"Damage on {gameObject.name}, current health : {_currentHealth}");
    }

    public void Heal(float healValue)
    {
        float newHealth = Mathf.Min(_currentHealth + healValue, _maxHealth);
        OnHealthChange?.Invoke(_currentHealth, newHealth);
        _onHealthChangeEvent.Invoke();
        _currentHealth = newHealth;
    }
}
