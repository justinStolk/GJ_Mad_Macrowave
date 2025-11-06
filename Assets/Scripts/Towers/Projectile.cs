using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayers;

    private Rigidbody rb;
    private ushort damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, float power, ushort damage)
    {
        this.damage = damage;
        rb.AddForce(direction.normalized * power, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Tower")) return;

        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
