using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IAController : MonoBehaviour
{
    private enum EIAState
    {
        Idle,
        Chase,
        Attack
    }

    [SerializeField] private EIAState currentState;
    private Rigidbody rb;

    [SerializeField] private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    [Header("IA Settings")]
    [SerializeField] private float attackRange;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackDamage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        switch (currentState)
        {
            case EIAState.Idle:
                // TODO
                break;
            case EIAState.Chase:
                Move();
                break;
            case EIAState.Attack:
                Attack();
                break;
        }
    }

    private void Move()
    {
        Vector3 direction = GetDirectionToTarget();
        if (direction != Vector3.zero)
        {
            rb.MovePosition(rb.position + direction * movementSpeed * Time.deltaTime);
            Debug.Log("Moving towards target");
        }
    }

    private void Attack()
    {

    }

    private Vector3 GetDirectionToTarget()
    {
        if (target != null)
        {
            return (target.position - transform.position).normalized;
        }
        return Vector3.zero;
    }

}
