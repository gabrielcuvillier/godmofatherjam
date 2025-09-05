using NUnit.Framework;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
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
    private Animator animator;
    [SerializeField] private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    [Header("Audio")]
    [Tooltip("Boucle jouée quand l'ennemi se déplace")]
    [SerializeField] private AudioClip movementLoopClip;
    [Tooltip("Son joué quand l'ennemi se fait toucher")]
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private float maxDistanceForSound;
    [SerializeField] private float spatialBlendForSound = 1f;

    private AudioSource movementSource;
    private AudioSource sfxSource;
    private bool hitOverrideActive = false;


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
        animator = GetComponent<Animator>();

        attackCooldown = 1f / attackSpeed;
        attackTimer = 0f;

        movementSource = gameObject.AddComponent<AudioSource>();
        movementSource.playOnAwake = false;
        movementSource.loop = true;
        movementSource.clip = movementLoopClip;
        movementSource.spatialBlend = spatialBlendForSound;
        movementSource.maxDistance = maxDistanceForSound;
        movementSource.rolloffMode = AudioRolloffMode.Linear;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.clip = hitClip;
        sfxSource.spatialBlend = spatialBlendForSound;
        sfxSource.maxDistance = maxDistanceForSound;
        sfxSource.rolloffMode = AudioRolloffMode.Linear;
    }

    private void Start()
    {
        if (target != null)
        {
            targetHealth = target.transform.parent.GetComponent<Health>();
        }
        animator.SetBool("IsMoving", true);
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

    public void Hit()
    {
        animator.SetTrigger("Hit");
        if (hitClip != null)
            StartCoroutine(PlayHitAndReturn());
    }

    private IEnumerator PlayHitAndReturn()
    {
        hitOverrideActive = true;

        if (movementSource.isPlaying) movementSource.Pause();

        sfxSource.Stop();
        sfxSource.clip = hitClip;
        sfxSource.Play();

        yield return new WaitForSeconds(hitClip.length);

        hitOverrideActive = false;

        if (animator.GetBool("IsMoving") && movementLoopClip != null)
        {
            if (!movementSource.isPlaying)
                movementSource.UnPause();
        }
    }

    private void Move()
    {
        if (target != null && IsTargetInRange())
        {
            currentState = EIAState.Attack;
            animator.SetBool("IsMoving", false);
            return;
        }
        Vector3 direction = GetDirectionToTarget();
        if (direction != Vector3.zero)
        {
            float currentMovementSpeed = currentTerrain == TerrainType.Sand ? movementSpeedSand : movementSpeedWater;
            rb.MovePosition(rb.position + direction * currentMovementSpeed * Time.deltaTime);

            if (!animator.GetBool("IsMoving"))
            {
                animator.SetBool("IsMoving", true);
                UpdateMovementAudio(true);
            }
        }
    }

    private void UpdateMovementAudio(bool shouldMove)
    {
        if (hitOverrideActive) return;

        if (shouldMove && movementLoopClip != null)
        {
            if (!movementSource.isPlaying)
            {
                if (movementSource.time > 0f)
                    movementSource.UnPause();
                else
                    movementSource.Play();
            }
        }
        else
        {
            if (movementSource.isPlaying) movementSource.Pause();
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
                animator.SetBool("IsMoving", true);
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
