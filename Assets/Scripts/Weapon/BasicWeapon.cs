using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BasicWeapon : WeaponUsage
{
    private GameObject _parent;
    Coroutine _coroutineMove;

    public override void WeaponCollide()
    {
        if (_coroutineMove != null) 
        {
            StopCoroutine(_coroutineMove);
            _coroutineMove = null;
        }
        Destroy(gameObject);
    }

    public override void Initialize(GameObject parent)
    {
        _parent = parent;
    }

    public override void Use()
    {
        _isThrown = true;
        transform.parent = null;
        Vector3 direction = _parent.transform.forward;
        _coroutineMove = StartCoroutine(Move(direction));
    }

    private IEnumerator Move(Vector3 direction)
    {
        float timer = 0f;

        Vector3 randomRotation = RandomVector();

        while (timer < lifetime)
        {
            transform.position += direction * speed * Time.deltaTime;
            transform.eulerAngles += randomRotation * Time.deltaTime * _speedRotation;
            yield return null;
            timer += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
