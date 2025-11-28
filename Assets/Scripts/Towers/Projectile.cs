using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayers;

    protected Rigidbody rb;
    protected ushort damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, float power, ushort damage)
    {
        this.damage = damage;
        rb.AddForce(direction.normalized * power, ForceMode.VelocityChange);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Floor") || other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(gameObject);
        }
    }
}
