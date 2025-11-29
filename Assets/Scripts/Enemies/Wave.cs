using UnityEngine;

[System.Serializable]
public class Wave
{
    public WaveData[] WaveData => waveData;
    public float WaveExitTime => waveExitTime;

    [SerializeField] private WaveData[] waveData;
    [Tooltip("How long until the next wave starts after the last enemy has spawned?"), SerializeField] private float waveExitTime;
}
