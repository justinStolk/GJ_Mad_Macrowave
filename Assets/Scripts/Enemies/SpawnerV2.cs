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

    private Wave CurrentWave { get { return waveContainer.Waves[waveIndex]; } }
    private SpawnData CurrentSpawnData { get { return CurrentWave.SpawnData[spawnDataIndex]; } }
    private InputAction positioningAction;

    private bool isPaused;
    private bool hasStarted;

    private float spawnTimer;
    private float intervalTime;

    private byte waveIndex;
    private byte spawnDataIndex;
    private byte enemyCounter;


    private void Awake()
    {
        MenuHandler.OnGamePaused += () => isPaused = true;
        MenuHandler.OnGameResumed += () => isPaused = false;

        positioningAction = InputSystem.actions.FindAction("Positioning");
    }
    // Update is called once per frame
    void Update()
    {
        // Do not update anything if the game is paused or if the player hasn't started yet.
        if (isPaused || !hasStarted) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer < intervalTime)
            return;

        spawnTimer = 0;
        HandleWave();
    }


    public void BeginSpawning()
    {
        intervalTime = CurrentSpawnData.SpawnInterval;
        onWaveStarted?.Invoke();
        hasStarted = true;
    }

    public IEnumerator EvaluateEndPointAccessability(NavMeshObstacle obstacle, Action<bool> onResultFound)
    {
        positioningAction.Disable();

        float waitTime = obstacle.carvingTimeToStationary + 0.1f;
        
        yield return new WaitForSeconds(waitTime);

        NavMeshPath newPath = new();
        NavMesh.CalculatePath(transform.position, endpoint.position, NavMesh.AllAreas, newPath);

        bool result = newPath.status == NavMeshPathStatus.PathComplete;

        obstacle.enabled = result;
        onResultFound?.Invoke(result);
    }

    private void SpawnNewEnemy(Enemy template)
    {
        Instantiate(template, transform.position, transform.rotation);
    }

    private void HandleWave()
    {
        SpawnNewEnemy(CurrentSpawnData.Enemy);
        enemyCounter++;
        if (enemyCounter == CurrentSpawnData.Count)
        {
            // This is the last enemy in this data
            enemyCounter = 0;
            spawnDataIndex++;
            if (spawnDataIndex >= CurrentWave.SpawnData.Length)
            {
                // This was the last spawn data in this wave
                spawnDataIndex = 0;
                waveIndex++;
                if (waveIndex >= waveContainer.Waves.Length)
                {
                    // This was the last wave
                    EndSpawningSequence();
                    return;
                }
                onWaveStarted?.Invoke();
                intervalTime = CurrentWave.WaveExitTime;
                return;
            }
            intervalTime = CurrentSpawnData.SpawnPause;
            return;
        }
        intervalTime = CurrentSpawnData.SpawnInterval;
    }

    private void EndSpawningSequence()
    {
        hasStarted = false;
        onAllWavesCleared?.Invoke();
    }
}
