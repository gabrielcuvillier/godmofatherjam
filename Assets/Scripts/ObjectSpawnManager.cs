using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectSpawnManager : MonoBehaviour
{
    [SerializeField] Inventory _inventory;
    [SerializeField] ObjectSpawnPoint[] _spawnPoints;
    [SerializeField] ObjectWeapon[] _objectsPool;
    [SerializeField] float _minSpawnCooldown = 3f;
    [SerializeField] float _maxSpawnCooldown = 5f;
    [SerializeField] int _nbObjectsSpawnedOnInit = 2;

    float _spawnCooldownDuration;
    float _currentCooldownDuration = 0f;
    
    private void Awake()
    {
        _spawnCooldownDuration = Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
    }

    private void Start()
    {
        for (int i = 0; i < _nbObjectsSpawnedOnInit && i < _spawnPoints.Length; i++)
        {
            SpawnObject();
        }
    }

    private void Update()
    {
        _currentCooldownDuration += Time.deltaTime;

        if (_currentCooldownDuration > _spawnCooldownDuration) 
        {
            SpawnObject();
            _currentCooldownDuration = 0f;
        }
    }

    public void SpawnObject()
    {
        List<ObjectSpawnPoint> _spawnPointsAvailable = new List<ObjectSpawnPoint>();

        foreach (ObjectSpawnPoint spawnPoint in _spawnPoints)
        {
            if (spawnPoint.IsPointAvailable)
            {
                _spawnPointsAvailable.Add(spawnPoint);
            }
        }

        if (_spawnPointsAvailable.Count > 0 && _objectsPool.Length > 0)
        {
            int indexSpawnPoint = Random.Range(0,_spawnPointsAvailable.Count);
            ObjectSpawnPoint spawnPoint = _spawnPointsAvailable[indexSpawnPoint];
            spawnPoint.IsPointAvailable = false;

            int indexObjectSpawned = Random.Range(0, _objectsPool.Length);
            ObjectWeapon objectSpawnedInstance = _objectsPool[indexObjectSpawned];
            ObjectWeapon objectSpawned = Instantiate(objectSpawnedInstance, spawnPoint.transform.position, Quaternion.identity);
            objectSpawned.SetSpawnPoint(spawnPoint);
            objectSpawned.SetInventory(_inventory);
        }
    }
}
