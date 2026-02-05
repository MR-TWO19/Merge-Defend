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
    private Coroutine runWavesCoroutine;

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
        runWavesCoroutine = StartCoroutine(RunWaves());
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
            }
            // Wait a few seconds between waves
                yield return new WaitForSeconds(wave.SpawnInterval);
        }
        isSpawning = false;
        runWavesCoroutine = null;
    }

    private void SpawnEnemy(CharacterInfo data)
    {
        Transform SpawnPoint = BattleManager.Ins.HomeEnemy.PosSpawnCharater.transform;
        Vector3 spawnPos = SpawnPoint.position;

        // random offset along right vector between -spawnOffset and +spawnOffset (same row)
        float lateral = Random.Range(-spawnOffset, spawnOffset);
        spawnPos += SpawnPoint.right * lateral;
        spawnPos.y = SpawnPoint.position.y; // keep same height

        GameObject prefab = GameConfig.Ins.GetEnemyData(data.CharId).Prefab;

        var go = GameObject.Instantiate(prefab, spawnPos, SpawnPoint.rotation);
        var enemy = go.GetComponent<Character>();
        if (enemy != null)
        {
            CharacterData characterData = GameConfig.Ins.GetEnemyData(data.CharId);

            CharacterInfo characterInfo = new()
            {
                CharId = data.CharId,
                Health = data.Health + characterData.Health,
                Damage = data.Damage + characterData.Damage,
                Speed = data.Speed + characterData.Speed
            };

            enemy.SetUp(characterInfo);
            activeEnemys.Add(enemy);
        }
    }

    public void RemoveHero(Character enemy)
    {
        if (enemy == null) return;
        if (activeEnemys.Contains(enemy)) activeEnemys.Remove(enemy);
        GameObject.Destroy(enemy.gameObject, 2);
    }

    public void ResetChar()
    {
        if (runWavesCoroutine != null)
        {
            StopCoroutine(runWavesCoroutine);
            runWavesCoroutine = null;
        }
        isSpawning = false;

        foreach (var item in activeEnemys)
        {
            GameObject.Destroy(item.gameObject);
        }

        activeEnemys.Clear();
    }

    public void StartBoss(int level)
    {
       CharacterData characterData = GameConfig.Ins.GetRondomBossData();

         CharacterInfo characterInfo = new()
         {
              CharId = characterData.id,
              Health = characterData.Health * level,
              Damage = characterData.Damage * level,
              Speed = characterData.Speed
         };

        Transform SpawnPoint = BattleManager.Ins.HomeEnemy.PosSpawnCharater.transform;
        Vector3 spawnPos = SpawnPoint.position;

        var go = GameObject.Instantiate(characterData.Prefab, spawnPos, SpawnPoint.rotation);
        Vector3 euler = go.transform.localEulerAngles;
        go.transform.localEulerAngles = new Vector3(euler.x, -180f, euler.z);
        var enemy = go.GetComponent<Character>();
        enemy.SetUp(characterInfo);
        activeEnemys.Add(enemy);
    }
}
