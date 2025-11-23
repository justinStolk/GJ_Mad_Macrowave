using UnityEngine;

public class SplitterEnemy : Enemy
{
    [SerializeField] private Enemy onDeathSpawnedEnemyPrefab;
    [SerializeField] private byte deathSpawnCount;

    protected override void OnDeath()
    {
        for (int i = 0; i < deathSpawnCount; i++)
        {
            Instantiate(onDeathSpawnedEnemyPrefab, transform.position, Quaternion.identity);
        }
        base.OnDeath();
    }
}
