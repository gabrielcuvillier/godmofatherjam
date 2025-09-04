using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBounds : MonoBehaviour
{
    [SerializeField] private float forceBound = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AffectBounds(Transform other)
    {
        Debug.Log("PlayerBound");
        Vector3 direction = (other.position - transform.position).normalized;
        rb.AddForce(-direction * forceBound, ForceMode.Impulse);
    }
}
