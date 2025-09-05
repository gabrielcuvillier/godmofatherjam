using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [Header("Wave Config")]
    [SerializeField] private int skeletonPerWave = 5;
    [SerializeField] private int poulpePerWave = 5;
    [SerializeField] private int requinPerWave = 5;
    [SerializeField] private int skeletonPerWaveIncrease = 2;
    [SerializeField] private int poulpePerWaveIncrease = 2;
    [SerializeField] private int requinPerWaveIncrease = 2;
    [SerializeField] private int maxEnemies = 20;
    [SerializeField] private int currentEnemies = 0;
    private List<int> enemyTypes; // 0 = Skeleton, 1 = Poulpe, 2 = Requin

    [Header("Ref Prefabs Enemy")]
    [SerializeField] private GameObject enemySkeletonPrefab;
    [SerializeField] private GameObject enemyPoulpePrefab;
    [SerializeField] private GameObject enemyRequinPrefab;

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
            waveUIText.text = $"{currentWave + 1}";
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

    private void SpawnEnemy(GameObject prefab)
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

        if (prefab != null && spawnPoints.Count > 0)
        {
            currentEnemies++;
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            IABase iaSkeleton = enemy.GetComponent<IABase>();
            if (iaSkeleton == null)
            {
                IAPoulpe iaPoulpe = enemy.GetComponent<IAPoulpe>();
                if (iaPoulpe != null)
                {
                    iaPoulpe.Target = playerTransform;
                }
                else
                {
                    IARequin iaRequin = enemy.GetComponent<IARequin>();
                    if (iaRequin != null)
                    {
                        iaRequin.Target = playerTransform;
                    }
                }
            }
            else
            {
                if (currentSpawnPointIndex == 0)
                {
                    iaSkeleton.Target = chest0Transform;
                }
                else if (currentSpawnPointIndex == 1)
                {
                    iaSkeleton.Target = chest1Transform;
                }
                else if (currentSpawnPointIndex == 2)
                {
                    iaSkeleton.Target = chest2Transform;
                }
            }
        }
    }

    private void GenerateListEnemy()
    {
        enemyTypes = new List<int>();
        for (int i = 0; i < skeletonPerWave; i++)
        {
            enemyTypes.Add(0);
        }
        for (int i = 0; i < poulpePerWave; i++)
        {
            enemyTypes.Add(1);
        }
        for (int i = 0; i < requinPerWave; i++)
        {
            enemyTypes.Add(2);
        }

        // Shuffle the list
        for (int i = 0; i < enemyTypes.Count; i++)
        {
            int temp = enemyTypes[i];
            int randomIndex = Random.Range(i, enemyTypes.Count);
            enemyTypes[i] = enemyTypes[randomIndex];
            enemyTypes[randomIndex] = temp;
        }

        // Increase for next wave
        skeletonPerWave += skeletonPerWaveIncrease;
        poulpePerWave += poulpePerWaveIncrease;
        if (skeletonPerWave + poulpePerWave > maxEnemies)
        {
            int total = skeletonPerWave + poulpePerWave;
            float ratioSkeleton = (float)skeletonPerWave / total;
            skeletonPerWave = Mathf.RoundToInt(maxEnemies * ratioSkeleton);
            poulpePerWave = maxEnemies - skeletonPerWave;
        }
    }

    private IEnumerator SpawnEnemiesWave()
    {
        GenerateListEnemy();
        for (int i = 0; i < enemyTypes.Count; i++)
        {
            GameObject enemyPrefab;
            if (enemyTypes[i] == 0)
            {
                enemyPrefab = enemySkeletonPrefab;
            }
            else if (enemyTypes[i] == 2)
            {
                enemyPrefab = enemyRequinPrefab;
            }
            else
            {
                enemyPrefab = enemyPoulpePrefab;
            }
            SpawnEnemy(enemyPrefab);
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
