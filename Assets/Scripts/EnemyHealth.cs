using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    public System.Action OnDamaged;

    private int currentHealth;
    private Rigidbody rb;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    public bool IsKnockedBack => isKnockedBack;

    private Animator animator;
    public float deathAnimationDuration = 2.0f;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
                isKnockedBack = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
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

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        OnDamaged?.Invoke();

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
}
