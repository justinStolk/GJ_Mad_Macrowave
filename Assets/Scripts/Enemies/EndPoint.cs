using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void Awake()
    {
        Enemy.OnEnemySpawn += SetPointForEnemy; 
    }

    public void SetPointForEnemy(Enemy enemy)
    {
        enemy.SetTarget(transform.position);
    }
}
