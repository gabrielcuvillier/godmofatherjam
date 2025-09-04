using System.Collections;
using UnityEngine;

public class BasicWeapon : WeaponUsage
{
    [SerializeField] private Transform shotPoint;
    [SerializeField] private Rigidbody rb;
    Vector3 direction;

    private void Awake()
    {
        direction = shotPoint.forward;
    }
    public override void Use()
    {
        Move();
        StartCoroutine(DestroyAfterTime());
    }

    public void Move()
    {
        rb.linearVelocity = direction * speed;
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
