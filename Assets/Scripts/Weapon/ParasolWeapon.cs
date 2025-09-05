using System.Collections;
using UnityEngine;

public class ParasolWeapon : WeaponUsage
{
    private GameObject _parent;
    Coroutine _coroutineMove;

    public override void Select()
    {
    }

    public override void Use()
    {
        _isThrown = true;
        transform.parent = null;
        Vector3 direction = _parent.transform.forward;
        _coroutineMove = StartCoroutine(Move(direction));
    }

    public override void WeaponCollide()
    {
        if (_isThrown && _coroutineMove != null)
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

    private IEnumerator Move(Vector3 direction)
    {
        float timer = 0f;
        while (timer < lifetime)
        {
            transform.position += direction * speed * Time.deltaTime;
            Vector3 rotationVector = transform.eulerAngles;
            rotationVector.x = transform.localEulerAngles.x + Time.deltaTime * _speedRotation;
            transform.localEulerAngles = rotationVector;
            yield return null;
            timer += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
