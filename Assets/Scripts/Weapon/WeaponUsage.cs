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

    protected bool _isThrown = false;
    public int Damage => damage;

    public virtual void Initialize(GameObject parent) { }
    public virtual void Select() { }
    public abstract void Use();
    public abstract void WeaponCollide();
    public virtual void DestroyWeapon()
    {
        if (_isThrown)
        {
            Destroy(gameObject);
        }
    }
}
