using UnityEngine;

[System.Serializable]
public class WaveData
{
    public Enemy Enemy => enemyReference;
    public ushort Count => enemyCount;
    public float SpawnInterval => spawnInterval;
    public float SpawnPause => spawnPause;

    [Tooltip("The enemy that will be spawned"), SerializeField] private Enemy enemyReference;
    [Tooltip("The amount of enemies spawned"), SerializeField] private ushort enemyCount;
    [Tooltip("The time between enemy spawns"), SerializeField] private float spawnInterval;
    [Tooltip("The time between enemy spawns after the last of this type has been spawned"), SerializeField] private float spawnPause;
}
