using System.Collections;
using UnityEngine;

public class BuoyWeapon : WeaponUsage
{
    [SerializeField] float _distanceRebound = 15f;
    private GameObject _parent;
    Coroutine _coroutineMove;

    public override void WeaponCollide()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject objective = null;
        for (int i = 0; i < gameObjects.Length && objective == null; i++) 
        {
            float distanceSquared = _distanceRebound * _distanceRebound;
            float distanceEnemyWeapon = (gameObjects[i].transform.position - transform.position).sqrMagnitude;
            if (distanceEnemyWeapon < distanceSquared && objective != this)
            {
                objective = gameObjects[i];
            }
        }
        RestartLifetime(objective.transform);
    }

    public override void Initialize(GameObject parent)
    {
        _parent = parent;
    }

    public override void Use()
    {
        transform.parent = null;
        Vector3 direction = _parent.transform.forward;
        _coroutineMove = StartCoroutine(Move(direction));
    }

    private IEnumerator Move(Vector3 direction)
    {
        float timer = 0f;
        while (timer < lifetime)
        {
            transform.position += direction * speed * Time.deltaTime;
            yield return null;
            timer += Time.deltaTime;
        }
        Destroy(gameObject);
    }

    private IEnumerator MoveTowards(Transform enemy)
    {
        Vector3 forward = transform.forward;
        float timer = 0f;
        while (timer < lifetime)
        {
            if (enemy != null) 
            { 
                transform.position += (enemy.position - transform.position).normalized * speed * Time.deltaTime;
            } else
            {
                transform.position += forward * speed * Time.deltaTime;
            }

            yield return null;
            timer += Time.deltaTime;
        }
        Destroy(gameObject);
    }

    public void RestartLifetime(Transform objective)
    {
        if (_coroutineMove != null)
        {
            StopCoroutine(_coroutineMove);
            _coroutineMove = null;
        }
        _coroutineMove = StartCoroutine(MoveTowards(objective));
    }

}
