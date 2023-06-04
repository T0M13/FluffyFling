using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private Rigidbody body;
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [Header("Settings")]
    [SerializeField] private float damageMultiplier = 1.1f;
    [SerializeField] private float fallThresshold = -10f;
    [SerializeField] private float fallDamage = 10f;
    [SerializeField] private float upThresshold = 50f;
    [SerializeField] private float upDamage = 1000f;
    [SerializeField] private float collisionVelocity;
    [Header("Score")]
    [SerializeField] private int scoreMin = 100;
    [SerializeField] private int scoreMax = 500;
    [SerializeField] private float scoreIncrement = 50f;
    [SerializeField] private ScorePopUp scorePopUp;


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
        if (sphereCollider == null)
            sphereCollider = GetComponent<SphereCollider>();
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
        collisionVelocity = collision.relativeVelocity.magnitude;

        float damage = collisionVelocity * damageMultiplier;

        TakeDamage(damage);
    }

    private void Update()
    {
        if (transform.position.y < fallThresshold)
        {
            TakeDamage(fallDamage);
        }
        if (transform.position.y > upThresshold)
        {
            TakeDamage(upDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            int score = Mathf.RoundToInt(Mathf.Lerp(scoreMin, scoreMax, collisionVelocity / scoreIncrement));
            GameManager.instance.AddScore(score);
            scorePopUp.SetScore(score);
            scorePopUp.gameObject.SetActive(true);
            scorePopUp.transform.SetParent(null);
            scorePopUp.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            scorePopUp.transform.localScale = Vector3.one;
            GameManager.instance.CheckEnemiesLeft(gameObject);
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
