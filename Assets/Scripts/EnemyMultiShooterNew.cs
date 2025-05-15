using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMultiShooterNew : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    public float chaseRange = 20f;
    public float attackRange = 10f;
    public float fireRate = 2f;
    public float projectileSpeed = 10f;
    public float moveSpeed = 3f;

    public int projectileCount = 5;
    public float spreadAngle = 45f;

    private Transform player;
    private Rigidbody rb;
    private EnemyHealth health;
    private float fireTimer = 0f;

    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        health = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (health != null && health.IsKnockedBack)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            FacePlayer();

            if (distance > attackRange)
                MoveTowardsPlayer();
            else
            {
                StopMovement();
                HandleShooting();
            }
        }
        else
        {
            StopMovement();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        if (animator != null)
            animator.SetBool("isMoving", true);
    }

    void StopMovement()
    {
        rb.velocity = Vector3.zero;

        if (animator != null)
            animator.SetBool("isMoving", false);
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void HandleShooting()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = 1f / fireRate;
        }
    }

    void Shoot()
    {
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
            Quaternion spreadRot = rotation * Quaternion.Euler(0f, angle, 0f);
            Vector3 shootDirection = spreadRot * Vector3.forward;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = shootDirection * projectileSpeed;
        }
        if (animator != null)
            animator.SetTrigger("Attack");
    }
}