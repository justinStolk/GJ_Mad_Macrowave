using UnityEngine;

public class EndPoint : MonoBehaviour
{

    public void SetPointForEnemy(Enemy enemy)
    {
        enemy.SetTarget(transform.position);
    }
}
