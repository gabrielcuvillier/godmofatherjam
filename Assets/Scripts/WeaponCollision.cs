using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    [SerializeField] WeaponUsage _weaponToDestroy;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SetDamage(collision.gameObject.GetComponent<Health>());
            _weaponToDestroy.DestroyWeapon();
            Destroy(_weaponToDestroy);
        }
    }
    private void SetDamage(Health health)
    {
        health.TakeDamage(_weaponToDestroy.Damage);
    }
}
