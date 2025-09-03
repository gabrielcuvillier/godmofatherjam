using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    [SerializeField] ObjectWeapon _objectWeapon;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            _objectWeapon?.AddObjectToInventory();
            Destroy(collision.gameObject);
        }
    }
}
