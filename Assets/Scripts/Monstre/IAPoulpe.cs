using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
public class IAPoulpe : MonoBehaviour
{
    // ne peux pas aller sur le sable et tire depuis l'eau
    public enum EIAState
    {
        Chase,
        Attack,
        Dead
    }

    private Rigidbody rb;
    private EncreUI targetEncreUI;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shotPoint;
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
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float countOfEncreToShoot = 10; // max 248

    private float attackCooldown;
    private float attackTimer;
    private enum TerrainType
    {
        Sand,
        Water
    }
    [SerializeField] private TerrainType currentTerrain = TerrainType.Water;

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
            targetEncreUI = target.transform.GetComponent<EncreUI>();
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

        if (currentTerrain == TerrainType.Sand)
        {
            return;
        }

        Vector3 direction = GetDirectionToTarget();
        if (direction != Vector3.zero)
        {
            rb.MovePosition(rb.position + direction * movementSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        if (target != null && attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            Debug.Log("Poulpe Attack!");
            LancerProjectile();

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

    private void LancerProjectile()
    {
        if (bulletPrefab != null && shotPoint != null && target != null)
        {
            Vector3 direction = GetDirectionToTarget();
            if (direction == Vector3.zero) return;
            GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, Quaternion.LookRotation(direction));
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (bulletController != null)
            {
                bulletController.Damage = (int)countOfEncreToShoot;
                bulletController.Initialize(direction);
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
