using UnityEngine;

[System.Serializable]
public class Wave
{
    public SpawnData[] SpawnData => spawnData;
    public float WaveExitTime => waveExitTime;

    [SerializeField] private SpawnData[] spawnData;
    [Tooltip("How long until the next wave starts after the last enemy has spawned?"), SerializeField] private float waveExitTime;
}
