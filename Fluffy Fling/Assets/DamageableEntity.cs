using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableEntity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Rigidbody body;
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [Header("Settings")]
    [SerializeField] private float damageMultiplier = .1f;
    [SerializeField] private float fallThresshold = -10f;
    [SerializeField] private float fallDamage = 10f;

    private void Awake()
    {
        GetCollider();
        GetRigidbody();
    }

    private void OnValidate()
    {
        GetCollider();
        GetRigidbody();
    }

    private void GetCollider()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();
    }

    private void GetRigidbody()
    {
        if (body == null)
            body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float collisionVelocity = collision.relativeVelocity.magnitude;

        float damage = collisionVelocity * damageMultiplier;

        TakeDamage(damage);
    }

    private void Update()
    {
        if (transform.position.y < fallThresshold)
        {
            TakeDamage(fallDamage);
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            // The entity has been destroyed or reached 0 health
            DestroyEntity();
        }
        else
        {
            // The entity has taken damage but still has remaining health
            // Implement any necessary visual or audio feedback here
        }
    }

    private void DestroyEntity()
    {
        Destroy(gameObject);
    }




}
