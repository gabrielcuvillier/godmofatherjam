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
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Ref UI")]
    [SerializeField] private TextMeshProUGUI waveUIText;

    [Header("Targets")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform chestTransform;

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
        UpdateWaveUI();
    }

    private void SpawnEnemy()
    {
        if (enemySimplePrefab != null && spawnPoints.Count > 0)
        {
            currentEnemies++;
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            IAController ia = Instantiate(enemySimplePrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<IAController>();
            ia.Target = chestTransform;
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
