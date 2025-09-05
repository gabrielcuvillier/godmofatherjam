using UnityEngine;
using UnityEngine.VFX;

public class WeaponCollision : MonoBehaviour
{
    [SerializeField] WeaponUsage _weaponToDestroy;
    [SerializeField] ParticleSystem _visualEffectSand;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _weaponToDestroy.IsThrown)
        {
            SetDamage(collision.gameObject.GetComponent<Health>());
            _weaponToDestroy.WeaponCollide();
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            _weaponToDestroy.DestroyWeapon();
            if (collision.contacts.Length > 0)
            {
                Instantiate(_visualEffectSand, collision.contacts[0].point, Quaternion.identity);
            }
        }
    }
    private void SetDamage(Health health)
    {
        health.TakeDamage(_weaponToDestroy.Damage);
    }
}
