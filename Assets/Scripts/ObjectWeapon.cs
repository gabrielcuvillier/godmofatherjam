using UnityEngine;

public class ObjectWeapon : MonoBehaviour
{
    [SerializeField] Item _item;
    [SerializeField] Inventory _inventory;

    ObjectSpawnPoint _spawnPoint;

    public void SetSpawnPoint(ObjectSpawnPoint spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }

    public void AddObjectToInventory()
    {
        if (_spawnPoint != null && _item != null) 
        { 
            _spawnPoint.IsPointAvailable = true;
            _inventory?.AddItem(_item);
        }
    }
}
