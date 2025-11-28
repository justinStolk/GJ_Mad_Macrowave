using System.Collections;
using UnityEngine;

public class StallProjectile : Projectile
{
    [SerializeField] private float stallTime;


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Tower")) return;

        if (other.TryGetComponent(out Enemy enemy))
        {
            Collider col = GetComponent<Collider>();
            col.enabled = false;
            StartCoroutine(OnStallTriggered(enemy, stallTime));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator OnStallTriggered(Enemy target, float stallTime)
    {
        rb.linearVelocity = Vector3.zero;
        rb.Sleep();
        transform.position = target.transform.position + Vector3.up * 0.5f;
        transform.localScale *= 1.3f;
        target.TakeDamage(damage);
        target.ToggleStall(true);
        yield return new WaitForSeconds(stallTime);
        if(target != null)
        {
            target.ToggleStall(false);
        }
        Destroy(gameObject);
    }
}
