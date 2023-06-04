using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBird : Bird
{
    [Header("Ability Stats")]
    [SerializeField] private float explosionDamage = 100;
    [SerializeField] private float explosionForce = 1000;
    [SerializeField] private float explosionRadius = 4;
    [SerializeField] private GameObject explosionEffect;

    public override void ActivateAbility(Vector2 _)
    {
        if (state != BirdState.Thrown) return;
        if (abilityActivated) return;
        if (hasCollided) return;
        abilityActivated = true;
        Ability();
    }

    public override void AutomaticAbility()
    {
        if (state != BirdState.Thrown) return;
        if (abilityActivated) return;
        if (hasCollided) return;
        abilityActivated = true;
        Ability();
    }

    private void Ability()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        explosionEffect.SetActive(true);
        explosionEffect.transform.SetParent(null);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (rb.transform.position - transform.position).normalized;
                rb.AddForce(direction * explosionForce, ForceMode.Impulse);
            }
            if (collider.gameObject.GetComponent<DamageableEntity>())
            {
                collider.gameObject.GetComponent<DamageableEntity>().TakeDamage(explosionDamage);
            }
            if (collider.gameObject.GetComponent<EnemyEntity>())
            {
                collider.gameObject.GetComponent<EnemyEntity>().TakeDamage(explosionDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
