using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
public class IABase : MonoBehaviour
{
    public enum EIAState
    {
        Chase,
        Attack,
        Dead
    }

    private Rigidbody rb;
    private Health targetHealth;
    [SerializeField] private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    [Header("IA Settings")]
    [SerializeField] private EIAState currentState;
    public EIAState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }
    [SerializeField] private float attackRange;
    [SerializeField] private float movementSpeedSand;
    [SerializeField] private float movementSpeedWater;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackDamage;

    private float attackCooldown;
    private float attackTimer;
    private enum TerrainType
    {
        Sand,
        Water
    }
    private TerrainType currentTerrain = TerrainType.Water;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        attackCooldown = 1f / attackSpeed;
        attackTimer = 0f;
    }

    private void Start()
    {
        if (target != null)
        {
            targetHealth = target.transform.parent.GetComponent<Health>();
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EIAState.Chase:
                Move();
                break;
            case EIAState.Attack:
                Attack();
                break;
        }

        UpdateRotation();

        // Debug 
        Debug.DrawLine(transform.position, target.position, Color.red);

        // Draw attack range circle
        int segments = 32;
        float angleStep = 360f / segments;
        Vector3 prevPoint = transform.position + new Vector3(attackRange, 0, 0);
        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 nextPoint = transform.position + new Vector3(Mathf.Cos(angle) * attackRange, 0, Mathf.Sin(angle) * attackRange);
            Debug.DrawLine(prevPoint, nextPoint, Color.yellow);
            prevPoint = nextPoint;
        }
    }

    public void Death()
    {
        WaveManager.Instance.EnemyKilled();
        Destroy(gameObject);
    }

    private void Move()
    {
        if (target != null && IsTargetInRange())
        {
            currentState = EIAState.Attack;
            return;
        }
        Vector3 direction = GetDirectionToTarget();
        if (direction != Vector3.zero)
        {
            float currentMovementSpeed = currentTerrain == TerrainType.Sand ? movementSpeedSand : movementSpeedWater;
            rb.MovePosition(rb.position + direction * currentMovementSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        if (target != null && attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            // Jouer l'attaque
            //Debug.Log($"{gameObject.name} attacks {target.name} for {attackDamage} damage.");
            targetHealth?.TakeDamage(attackDamage);

        }
        else
        {
            attackTimer += Time.deltaTime;

            if (!IsTargetInRange())
            {
                currentState = EIAState.Chase;
            }
        }
    }

    private void UpdateRotation()
    {
        if (target != null)
        {
            Vector3 direction = GetDirectionToTarget();
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                rb.MoveRotation(lookRotation);
            }
        }
    }

    private Vector3 GetDirectionToTarget()
    {
        if (target != null)
        {
            return (target.position - transform.position).normalized;
        }
        return Vector3.zero;
    }

    private bool IsTargetInRange()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= attackRange;
        }
        return false;
    }

    public void ChangeTerrainToSand()
    {
        currentTerrain = TerrainType.Sand;
    }


}
