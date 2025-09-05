using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    private int damage;
    public int Damage { get { return damage; } set { damage = value; } }
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 direction)
    {
        rb.linearVelocity = direction * speed;
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            SetDamage(other.GetComponent<EncreUI>());
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Debug.Log("Bullet hit: " + other.name);
            Destroy(gameObject);
        }
    }

    private void SetDamage(EncreUI encreUI)
    {
        encreUI.AddEncre(damage);
    }

}
