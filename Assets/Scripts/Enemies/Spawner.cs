using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{

    [SerializeField] private WaveContainer waveContainer;
    [SerializeField] private UnityEvent onAllWavesCleared;
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
        if(waveIndex + 1 >= waveContainer.Waves.Length)
        {
            Debug.Log("All waves cleared!");
            // There are no more waves left
            return;
        }
        float exitTime = waveContainer.Waves[waveIndex].WaveExitTime;
        waveIndex++;
        StartCoroutine(StartWave(exitTime));
    }

    private IEnumerator StartWave(float delay)
    {
        yield return new WaitForSeconds(delay);

        Wave currentWave = waveContainer.Waves[waveIndex];
        for (int e = 0; e < currentWave.WaveData.Length; e++)
        {
            WaveData currentCluster = currentWave.WaveData[e];
            for (int i = 0; i < currentCluster.Count; i++)
            {
                SpawnEnemy(currentCluster.Enemy);
                if(i != currentCluster.Count - 1)
                {
                    yield return new WaitForSeconds(currentCluster.SpawnInterval);
                }
                else
                {
                    yield return new WaitForSeconds(e == currentWave.WaveData.Length - 1 ? currentWave.WaveExitTime : currentCluster.SpawnPause);
                }
            }
        }        
        EndWave();
    }
}
