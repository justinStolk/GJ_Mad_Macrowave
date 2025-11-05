using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{

    [SerializeField] private Wave[] waves;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private UnityEvent<Enemy> onEnemySpawned;

    private ushort waveIndex;
    private ushort dataIndex;
    private ushort enemyIndex;

    private void SpawnEnemy()
    {
        Wave current = waves[waveIndex];
        if(current.WaveData[dataIndex].Count <= enemyIndex)
        {
            enemyIndex = 0;
            dataIndex++;
            if(current.WaveData.Length <= dataIndex)
            {
                dataIndex = 0;
                EndWave();
            }
        }
        Enemy spawnedEnemy = Instantiate(current.WaveData[dataIndex].Enemy, transform.position, transform.rotation);
        onEnemySpawned?.Invoke(spawnedEnemy);
    }

    private void EndWave()
    {
        waveIndex++;
        if(waveIndex >= waves.Length)
        {
            // There are no more waves left
            return;
        }
        StartWave(timeBetweenWaves);
    }

    private IEnumerator StartWave(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemy();
    }
}
