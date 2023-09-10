using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;

    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public TextMeshProUGUI waveCountdownText;

    private int waveIndex = 0;
    public int numberEnemy = 0;

    private void Awake()
    {
        if(instance!=null)
        {
            Debug.Log("More than one WaveSpawner in screen");
            return;
        }
        instance = this;
    }

    private void Update() {
        if(countdown<=0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    public IEnumerator SpawnWave()
    {
        waveIndex++;
        PlayerStats.rounds++;
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.6f);
        }

        Debug.Log("Wave Incoming");
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Enemy enemy = newEnemy.GetComponent<Enemy>();
        if (enemy != null)
        {
            numberEnemy++;
        }
        else
        {
            Debug.LogError("EnemyPrefab does not contain the Enemy script.");
        }
        // GameObject newEnemy = (GameObject)Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        // Enemy enemy = newEnemy.GetComponent<Enemy>();
        // enemy.setNumber(numberEnemy);
        // numberEnemy++;
    }
}
