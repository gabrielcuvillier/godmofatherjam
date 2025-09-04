using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponUsage : MonoBehaviour
{
    public event Action OnWeaponUsed;
    [SerializeField] UnityEvent OnWeaponUsedEvent;

    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float lifetime;

    public int Damage => damage;

    public virtual void Select() { }
    public abstract void Use();
}
