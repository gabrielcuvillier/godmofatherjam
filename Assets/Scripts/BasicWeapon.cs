using System.Collections;
using UnityEngine;

public class BasicWeapon : WeaponUsage
{
    private GameObject _parent;
    [SerializeField] private Rigidbody rb;
    Coroutine _coroutineMove;

    public override void DestroyWeapon()
    {
        if (_coroutineMove != null) 
        {
            StopCoroutine(_coroutineMove);
            _coroutineMove = null;
        }
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
}
