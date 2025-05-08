using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;
    public float chaseRange = 10f;
    public int maxHealth = 3;
    public float knockbackForce = 5f;

    private int currentHealth;
    private Transform player;
    private bool isChasing = false;
    private Rigidbody rb;

    private bool isKnockedBack = false;
    private float knockbackDuration = 0.2f;
    private float knockbackTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isChasing && Vector3.Distance(transform.position, player.position) < chaseRange)
        {
            isChasing = true;
        }

        // Count down knockback
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
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

            // Knockback from projectile velocity
            Vector3 knockbackDir = other.attachedRigidbody != null
                ? other.attachedRigidbody.velocity.normalized
                : (transform.position - other.transform.position).normalized;

            rb.velocity = Vector3.zero; // Reset current motion
            rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

            isKnockedBack = true;
            knockbackTimer = knockbackDuration;

            Destroy(other.gameObject);
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