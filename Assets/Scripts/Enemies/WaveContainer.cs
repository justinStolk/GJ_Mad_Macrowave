using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveContainer : ScriptableObject
{
    public WaveData[] WaveData => waveData;

    [SerializeField] private WaveData[] waveData;

}
