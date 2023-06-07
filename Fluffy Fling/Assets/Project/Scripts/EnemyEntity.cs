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
    [SerializeField] private float rightThreshhold = 45f;
    [SerializeField] private float leftThreshhold = 7.25f;
    [SerializeField] private float collisionVelocity;
    [Header("Score")]
    [SerializeField] private int scoreMin = 100;
    [SerializeField] private int scoreMax = 500;
    [SerializeField] private float scoreIncrement = 50f;
    [SerializeField] private ScorePopUp scorePopUp;
    [SerializeField] private ParticleSystem impactEffect;
    [SerializeField] private GameObject destroyEffect;

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

        TakeDamageCollision(damage, collision);
    }

    private void Update()
    {
        if (transform.position.y < fallThresshold || transform.position.x > rightThreshhold || transform.position.x < leftThreshhold)
        {
            TakeDamage(fallDamage);
        }
        if (transform.position.y > upThresshold && body.useGravity)
        {
            TakeDamage(upDamage);
        }
        else if (transform.position.y > upThresshold && !body.useGravity)
        {
            Vector3 temp = new Vector3(transform.position.x, upThresshold - 1f, transform.position.z);
            transform.position = temp;
            body.useGravity = true;
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
            GameManager.instance.RemoveEnemy(gameObject);
            GameManager.instance.CheckEnemiesLeft();

            if (destroyEffect != null)
            {
                destroyEffect.transform.position = transform.position;
                destroyEffect.gameObject.SetActive(true);
                destroyEffect.transform.SetParent(null);
            }

            DestroyEntity();
        }
    }

    public void TakeDamageCollision(float damage, Collision collision)
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
            GameManager.instance.RemoveEnemy(gameObject);
            GameManager.instance.CheckEnemiesLeft();

            if (destroyEffect != null)
            {
                destroyEffect.transform.position = transform.position;
                destroyEffect.gameObject.SetActive(true);
                destroyEffect.transform.SetParent(null);
            }

            DestroyEntity();
        }
        else
        {
            if (impactEffect != null)
            {
                ContactPoint contact = collision.contacts[0];
                impactEffect.transform.position = contact.point;
                impactEffect.Play();


                if (AudioManager.instance)
                    AudioManager.instance.Play("impactSFX");
            }
        }
    }

    private void DestroyEntity()
    {
        if (AudioManager.instance)
            AudioManager.instance.Play("poofSFX");
        Destroy(gameObject);
    }

}
