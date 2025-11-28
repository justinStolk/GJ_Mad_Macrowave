using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{

    [SerializeField] private WaveContainer wave;
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private float timeBetweenSpawns = 2f;
    [SerializeField] private UnityEvent<Enemy> onEnemySpawned;
    [SerializeField] private Transform endpoint;

    private ushort waveIndex;
    private NavMeshPath path;

    private void Awake()
    {
        path = new NavMeshPath();
        StartCoroutine(StartWave(3f));
    }

    public bool EvaluateEndpointAccessability()
    {
        if (endpoint == null) return false;


        return NavMesh.CalculatePath(transform.position, endpoint.position, NavMesh.AllAreas, path);
    }

    private void SpawnEnemy(Enemy template)
    {
        Instantiate(template, transform.position, transform.rotation);
    }

    private void EndWave()
    {
        waveIndex++;
        if(waveIndex >= wave.WaveData.Length)
        {
            Debug.Log("All waves cleared!");
            // There are no more waves left
            return;
        }
        StartCoroutine(StartWave(timeBetweenWaves));
    }

    private IEnumerator StartWave(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int e = 0; e < wave.WaveData[waveIndex].Count; e++)
        {
            SpawnEnemy(wave.WaveData[waveIndex].Enemy);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }        
        EndWave();
    }
}
