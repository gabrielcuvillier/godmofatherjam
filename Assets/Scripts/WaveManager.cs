using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [Header("Wave Config")]
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private int maxEnemies = 20;
    [SerializeField] private int currentEnemies = 0;

    [Header("Ref Prefabs Enemy")]
    [SerializeField] private GameObject enemySimplePrefab;


    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints0;
    [SerializeField] private List<Transform> spawnPoints1;
    [SerializeField] private List<Transform> spawnPoints2;
    private int currentSpawnPointIndex = 0;

    [Header("Ref UI")]
    [SerializeField] private TextMeshProUGUI waveUIText;

    [Header("Targets")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform chest0Transform;
    [SerializeField] private Transform chest1Transform;
    [SerializeField] private Transform chest2Transform;

    private int currentWave = 0;
    public int CurrentWave
    {
        get { return currentWave; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateWaveUI();
        SetSpawnPointIndexRandom();
        StartCoroutine(SpawnEnemiesWave());
    }

    private void UpdateWaveUI()
    {
        if (waveUIText != null)
        {
            waveUIText.text = $"{currentWave}";
        }
    }

    private void NextWave()
    {
        currentWave++;
        SetSpawnPointIndexRandom();
        UpdateWaveUI();
    }

    public void SetSpawnPointIndex(int index)
    {
        currentSpawnPointIndex = index;
    }

    private void SetSpawnPointIndexRandom()
    {
        currentSpawnPointIndex = Random.Range(0, 3);
    }

    private void SpawnEnemy()
    {
        List<Transform> spawnPoints = spawnPoints0;
        if (currentSpawnPointIndex == 1)
        {
            spawnPoints = spawnPoints1;
        }
        else if (currentSpawnPointIndex == 2)
        {
            spawnPoints = spawnPoints2;
        }

        if (enemySimplePrefab != null && spawnPoints.Count > 0)
        {
            currentEnemies++;
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            IAController ia = Instantiate(enemySimplePrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<IAController>();

            if (currentSpawnPointIndex == 0)
            {
                ia.Target = chest0Transform;
            }
            else if (currentSpawnPointIndex == 1)
            {
                ia.Target = chest1Transform;
            }
            else if (currentSpawnPointIndex == 2)
            {
                ia.Target = chest2Transform;
            }
        }
    }

    private IEnumerator SpawnEnemiesWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }

        while (currentEnemies > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(5f);
        NextWave();
        StartCoroutine(SpawnEnemiesWave());
    }

    public void EnemyKilled()
    {
        currentEnemies--;
    }

}
