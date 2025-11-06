using UnityEngine;

[System.Serializable]
public class WaveData
{
    public Enemy Enemy => enemyReference;
    public ushort Count => enemyCount;

    [SerializeField] private Enemy enemyReference;
    [SerializeField] private ushort enemyCount;
}
