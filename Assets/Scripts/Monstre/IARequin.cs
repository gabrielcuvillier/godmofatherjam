using Unity.VisualScripting;
using UnityEngine;

public class IARequin : MonoBehaviour
{
    // fonctionnement de l'ia du requin
    // avance jusqu'au player (en fonction de sa range)
    // il charge en ligne droite une fois dans la range de x temps
    // il a un cooldown entre chaque charge

    [Header("Requin settings")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float chargeSpeed = 5f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float chargeDuration = 2f;
    [SerializeField] private float chargeCooldown = 3f;

    private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }
    private Vector3 targetForCharge;
    private Rigidbody rb;
    private PlayerBounds playerBounds;
    private enum EState
    {
        Chase,
        Charge,
        Cooldown
    }
    private EState currentState = EState.Chase;

    private float chargeTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (target != null)
        {
            playerBounds = target.GetComponent<PlayerBounds>();
        }
    }

    private void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case EState.Chase:
                ChaseTarget(distanceToTarget);
                break;
            case EState.Charge:
                ChargeAtTarget();
                break;
            case EState.Cooldown:
                Cooldown();
                break;
        }

        UpdateRotation();

        // Debug
        Debug.DrawLine(transform.position, target.position, Color.red);
        Debug.DrawLine(transform.position, targetForCharge, Color.blue);
        Debug.DrawRay(transform.position, GetDirectionToTarget() * 2f, Color.green);

        // Draw attack range circle
        int segments = 32;
        float angleStep = 360f / segments;
        Vector3 prevPoint = transform.position + new Vector3(range, 0, 0);
        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 nextPoint = transform.position + new Vector3(Mathf.Cos(angle) * range, 0, Mathf.Sin(angle) * range);
            Debug.DrawLine(prevPoint, nextPoint, Color.yellow);
            prevPoint = nextPoint;
        }
    }

    private void ChaseTarget(float distanceToTarget)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);

        if (distanceToTarget <= range)
        {
            currentState = EState.Charge;
            targetForCharge = target.position;
            chargeTimer = 0f;
        }
    }

    private void ChargeAtTarget()
    {
        chargeTimer += Time.deltaTime;
        Vector3 direction = (targetForCharge - transform.position).normalized;
        rb.MovePosition(transform.position + direction * chargeSpeed * Time.deltaTime);
    }

    private void Cooldown()
    {
        chargeTimer += Time.deltaTime;
        if (chargeTimer >= chargeCooldown)
        {
            currentState = EState.Chase;
        }
    }

    public void Death()
    {
        WaveManager.Instance.EnemyKilled();
        Destroy(gameObject);
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
            switch (currentState)
            {
                case EState.Chase:
                    return (target.position - transform.position).normalized;
                case EState.Charge:
                    return (targetForCharge - transform.position).normalized;
                case EState.Cooldown:
                    return (targetForCharge - transform.position).normalized;
            }
            return (target.position - transform.position).normalized;
        }
        return Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(playerBounds);
            playerBounds?.AffectBounds(transform);
            currentState = EState.Cooldown;
            chargeTimer = 0f;
        }
    }

}
