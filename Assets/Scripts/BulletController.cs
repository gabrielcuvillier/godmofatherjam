using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("TODO Enemy hit");
        }
    }

}
