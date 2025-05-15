using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;
    public float chaseRange = 10f;
    public int maxHealth = 3;
    public float knockbackForce = 5f;
    public float damageInterval = 1f; 
    public int contactDamage = 1;

    private int currentHealth;
    private Transform player;
    private bool isChasing = false;
    private Rigidbody rb;

    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private float knockbackDuration = 0.2f;

    private bool isTouchingPlayer = false;
    private float damageTimer = 0f;
    private GameObject playerObject;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerObject = player.gameObject;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isChasing && Vector3.Distance(transform.position, player.position) < chaseRange)
        {
            isChasing = true;
        }

        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
                isKnockedBack = false;
        }

        if (isTouchingPlayer)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                DamagePlayer();
                damageTimer = damageInterval;
            }
        }
    }

    void FixedUpdate()
    {
        if (isChasing && !isKnockedBack)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            isChasing = true;
            TakeDamage(1);

            Vector3 knockbackDir = other.attachedRigidbody != null
                ? other.attachedRigidbody.velocity.normalized
                : (transform.position - other.transform.position).normalized;

            rb.velocity = Vector3.zero;
            rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

            isKnockedBack = true;
            knockbackTimer = knockbackDuration;

            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            damageTimer = 0f; 
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    private void DamagePlayer()
    {
        PlayerHealth playerHealth = playerObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
        }
    }

    void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}