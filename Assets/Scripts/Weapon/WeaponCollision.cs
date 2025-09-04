using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    [SerializeField] WeaponUsage _weaponToDestroy;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SetDamage(collision.gameObject.GetComponent<Health>());
            _weaponToDestroy.WeaponCollide();
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            _weaponToDestroy.DestroyWeapon();
        }
    }
    private void SetDamage(Health health)
    {
        health.TakeDamage(_weaponToDestroy.Damage);
    }
}
