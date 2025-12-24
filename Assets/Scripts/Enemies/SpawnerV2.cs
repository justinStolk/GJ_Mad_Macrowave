using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SpawnerV2 : MonoBehaviour
{
    [SerializeField] private WaveContainer waveContainer;
    [SerializeField] private UnityEvent onWaveStarted;
    [SerializeField] private UnityEvent onAllWavesCleared;
    [SerializeField] private Transform endpoint;

    private bool isPaused;
    private bool hasStarted;

    private float spawnTimer;

    private Wave currentWave;
    private byte waveIndex;

    private WaveData currentWaveData;
    private byte waveDataIndex;

    private byte clusterIndex;

    // Update is called once per frame
    void Update()
    {
        // Do not update anything if the game is paused or if the player hasn't started yet.
        if (isPaused || !hasStarted) return;

        spawnTimer += Time.deltaTime;

        HandleCluster();
    }


    public void BeginSpawning()
    {
        currentWave = waveContainer.Waves[0];
        currentWaveData = currentWave.WaveData[0];
        onWaveStarted?.Invoke();
        hasStarted = true;
    }

    public IEnumerator EvaluateEndPointAccessability(NavMeshObstacle obstacle, Action<bool> onResultFound)
    {
        InputSystem.actions.FindAction("Positioning").Disable();

        float waitTime = obstacle.carvingTimeToStationary + 0.25f;
        
        yield return new WaitForSeconds(waitTime);

        NavMeshPath newPath = new();
        NavMesh.CalculatePath(transform.position, endpoint.position, NavMesh.AllAreas, newPath);

        bool result = newPath.status == NavMeshPathStatus.PathComplete;

        Debug.Log(result);
        obstacle.enabled = result;
        onResultFound?.Invoke(result);
    }

    private void SpawnNewEnemy(Enemy template)
    {
        Instantiate(template, transform.position, transform.rotation);
    }

    private void HandleCluster()
    {
        bool clusterIsFinished = clusterIndex == currentWaveData.Count;
        float waitTime = clusterIndex < currentWaveData.Count ? currentWaveData.SpawnInterval : currentWaveData.SpawnPause;
       
        if (spawnTimer < waitTime) return;

        spawnTimer = 0;

        if (clusterIsFinished)
        {
            clusterIndex = 0;
            waveDataIndex++;
            if(waveDataIndex < currentWave.WaveData.Length)
            {
                currentWaveData = currentWave.WaveData[waveDataIndex];
                return;
            }
            PrepareNextWave();
            return;
        }
        clusterIndex++;
        SpawnNewEnemy(currentWaveData.Enemy);
    }

    private void PrepareNextWave()
    {
        waveIndex++;
        waveDataIndex = 0;
        if(waveIndex > waveContainer.Waves.Length)
        {
            EndSpawningSequence();
            return;
        }
        onWaveStarted?.Invoke();
        currentWave = waveContainer.Waves[waveIndex];
        currentWaveData = currentWave.WaveData[waveDataIndex];
    }

    private void EndSpawningSequence()
    {
        hasStarted = false;
        onAllWavesCleared?.Invoke();
    }
}
