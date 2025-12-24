using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private bool playOnAwake;
    [SerializeField] private WaveContainer waveContainer;
    [SerializeField] private UnityEvent onWaveStarted;
    [SerializeField] private UnityEvent onAllWavesCleared;
    [SerializeField] private Transform endpoint;

    private ushort waveIndex;
    //private NavMeshPath path;

    private void Awake()
    {
        //path = new NavMeshPath();
        if (playOnAwake)
        {
            StartCoroutine(StartWave(3f));
        }
    }
    
    public void BeginSpawning()
    {
        StartCoroutine(StartWave(1f));
    }

    public bool EvaluateEndpointAccessability()
    {
        if (endpoint == null) return false;

        NavMeshPath newPath = new();
        NavMesh.CalculatePath(transform.position, endpoint.position, NavMesh.AllAreas, newPath);
        return newPath.status == NavMeshPathStatus.PathComplete;
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

        onWaveStarted?.Invoke();
        Wave currentWave = waveContainer.Waves[waveIndex];
        for (int e = 0; e < currentWave.SpawnData.Length; e++)
        {
            SpawnData currentCluster = currentWave.SpawnData[e];
            for (int i = 0; i < currentCluster.Count; i++)
            {
                SpawnEnemy(currentCluster.Enemy);
                if(i != currentCluster.Count - 1)
                {
                    yield return new WaitForSeconds(currentCluster.SpawnInterval);
                }
                else
                {
                    yield return new WaitForSeconds(e == currentWave.SpawnData.Length - 1 ? currentWave.WaveExitTime : currentCluster.SpawnPause);
                }
            }
        }        
        EndWave();
    }
}
