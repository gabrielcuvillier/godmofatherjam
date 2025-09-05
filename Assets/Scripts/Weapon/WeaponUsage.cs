using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponUsage : MonoBehaviour
{
    public event System.Action OnWeaponUsed;
    [SerializeField] UnityEvent OnWeaponUsedEvent;

    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float lifetime;
    [SerializeField] protected float _speedRotation = 10f;

    protected bool _isThrown = false;
    public bool IsThrown => _isThrown;
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

    protected float RandomRotation()
    {
        return Random.Range(0, 360);
    }

    protected Vector3 RandomVector()
    {
        Vector3 rotation;
        rotation.x = Random.Range(0, 360);
        rotation.y = Random.Range(0, 360);
        rotation.z = Random.Range(0, 360);
        return rotation;
    }
}
