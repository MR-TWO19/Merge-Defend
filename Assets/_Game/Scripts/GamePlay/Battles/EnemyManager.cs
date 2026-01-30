using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Character> activeEnemys = new List<Character>();

    [Header("Spawn Settings")]
    public float spawnOffset = 2f; 
    public float spawnForwardRandom = 0.5f;

    private List<Wave> Waves;
    private bool isSpawning = false;
    private int currentWave = 0;

    public void SetUp(List<Wave> waves)
    {
        Waves = waves;
        StartWaves();
    }

    public void StartWaves()
    {
        if (isSpawning) return;
        // If we've finished all waves, restart from beginning
        if (currentWave >= Waves.Count) currentWave = 0;
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        isSpawning = true;
        foreach (var wave in Waves)
        {
            foreach (var enemyLevel in wave.Enemies)
            {
                for (int i = 0; i < enemyLevel.total; i++)
                {
                    SpawnEnemy(enemyLevel.enemyData);
                    yield return new WaitForSeconds(enemyLevel.SpawnInterval);
                }
                yield return new WaitForSeconds(wave.SpawnInterval);
            }
            // Wait a few seconds between waves
        }
        isSpawning = false;
    }

    private void SpawnEnemy(CharacterInfo data)
    {
        Transform SpawnPoint = BattleManager.Ins.HomeEnemy.transform;
        Vector3 spawnPos = SpawnPoint.position;

        // random offset along right vector between -spawnOffset and +spawnOffset
        float lateral = Random.Range(-spawnOffset, spawnOffset);
        spawnPos += SpawnPoint.right * lateral;

        // add small forward/backward jitter
        float forwardJitter = Random.Range(-spawnForwardRandom, spawnForwardRandom);
        spawnPos += SpawnPoint.forward * forwardJitter;

        GameObject prefab = GameConfig.Ins.GetEnemyData(data.HeroId).Prefab;

        var go = GameObject.Instantiate(prefab, spawnPos, SpawnPoint.rotation);
        var enemy = go.GetComponent<Character>();
        if (enemy != null)
        {
            enemy.SetUp(data);
            activeEnemys.Add(enemy);
        }
    }
}
