using UnityEngine;

public enum TowerAttackType { Single, Area, Multi }

public class Tower : MonoBehaviour
{

    [SerializeField] private ushort power;
    [SerializeField] private float attackInterval;
    [SerializeField] private float range;
    [SerializeField] private TowerAttackType attackType;
    [SerializeField] private LayerMask enemyLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyLayer); 
        foreach(Collider col in hits)
        {
            Debug.Log(col.name + " is in range of " + name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.aliceBlue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
