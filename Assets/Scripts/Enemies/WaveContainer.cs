using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveContainer : ScriptableObject
{
    public Wave[] Waves => waves;

    [SerializeField] private Wave[] waves;

}
