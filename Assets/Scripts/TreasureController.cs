using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class Treasure : MonoBehaviour
{
    private Health health;

    [SerializeField] private Image healthBar;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = health.CurrentHealth / health.MaxHealth;
    }

}
