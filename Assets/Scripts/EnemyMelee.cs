using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMelee : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseRange = 10f;
    public int contactDamage = 1;
    public float damageInterval = 1f;

    private Transform player;
    private Rigidbody rb;
    private EnemyHealth health;

    private float damageTimer = 0f;
    private bool isTouchingPlayer = false;
    public bool isAggro = false;

    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        health = GetComponent<EnemyHealth>();

        animator = GetComponent<Animator>();

        if (health != null)
        {
            health.OnDamaged += BecomeAggro;
        }
    }

    void FixedUpdate()
    {
        if (health != null && health.IsKnockedBack) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            BecomeAggro();
        }

        if (isAggro)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (animator != null)
            animator.SetBool("isMoving", true);
    }

    void Update()
    {
        if (isTouchingPlayer)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                TryDamagePlayer();
                damageTimer = damageInterval;
            }
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


    void TryDamagePlayer()
    {
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(contactDamage);
                if (animator != null)
                    animator.SetTrigger("Attack");
            }
        }
    }

    void BecomeAggro()
    {
        isAggro = true;
    }
}