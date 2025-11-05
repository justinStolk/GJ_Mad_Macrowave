using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class Wave : ScriptableObject
{
    public WaveData[] WaveData => waveData;

    [SerializeField] private WaveData[] waveData;

}
