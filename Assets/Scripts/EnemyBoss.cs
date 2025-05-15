using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [Header("General")]
    public Transform player;
    private Animator animator;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject winScreen;

    [Header("Health")]
    public int maxHealth = 3;
    private bool isImmune = false;
    public GameObject shield;

    [Header("Phase 1 Settings")]
    public float fireRatePhase1 = 1.0f;
    public float projectileSpeedPhase1 = 3f;
    public float angerRange = 10f;

    [Header("Phase 2 Settings")]
    public GameObject enemyMelee;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public float spawnRate = 1f;
    public int spawnCount = 3;
    public float phaseDuration = 10f;

    [Header("Phase 3 Settings")]
    public float fireRatePhase3 = 1.0f;
    public float projectileSpeedPhase3 = 3f;
    public int burstAngle = 360;
    public int projectileCount = 10;


    private int currentHealth;
    private enum BossPhase {Phase1, Phase2, Phase3}
    private BossPhase currentPhase = BossPhase.Phase1;

    private float fireTimer = 0f;
    private int spawnCounter = 0;
    private Rigidbody rb;

    

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        FacePlayer();
        switch (currentPhase)
        {
            case BossPhase.Phase1:
                PhaseOne(); 
                break;
            case BossPhase.Phase2:
                PhaseTwo();
                break;
            case BossPhase.Phase3:
                PhaseThree();
                break;
        }
        if (currentPhase == BossPhase.Phase1 && currentHealth <= maxHealth * 0.5f)
        {
            isImmune = true;
            shield.SetActive(true);
            currentPhase = BossPhase.Phase2;
        }
    }

    private void PhaseOne() 
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f) 
        {
            ShootAtPlayer();
            fireTimer = 1f / fireRatePhase1;
        }
    }

    private void ShootAtPlayer()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        if (player != null) 
        {
            Vector3 direction = (player.position - firePoint.position).normalized;
            projRb.velocity = direction * projectileSpeedPhase1;
            if (animator != null)
                animator.SetTrigger("Attack");
        }
    }

    private void PhaseTwo() 
    {
        fireTimer -= Time.deltaTime;
        phaseDuration -= Time.deltaTime;

        if (fireTimer <= 0f && spawnCounter < spawnCount)
        {
            SpawnEnemies();
            spawnCounter++;
            fireTimer = 1f / spawnRate;
        }

        if (phaseDuration <= 0f)
        {
            isImmune = false;
            shield.SetActive(false);
            currentPhase = BossPhase.Phase3;
        }
    }

    private void SpawnEnemies() 
    {
        GameObject enemyInstance = Instantiate(enemyMelee, spawnPoint1.position, spawnPoint1.rotation);
        EnemyMelee enemyScript = enemyInstance.GetComponent<EnemyMelee>();
        if (enemyScript != null)
        {
            enemyScript.isAggro = true;
        }

        GameObject enemyInstance2 = Instantiate(enemyMelee, spawnPoint2.position, spawnPoint2.rotation);
        EnemyMelee enemyScript2 = enemyInstance2.GetComponent<EnemyMelee>();
        if (enemyScript2 != null)
        {
            enemyScript2.isAggro = true;
        }

        if (animator != null)
            animator.SetTrigger("Attack");
    }

    private void PhaseThree()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            ShootBurst();
            ShootAtPlayer();
            fireTimer = 1f / fireRatePhase3;
        }
    }

    private void ShootBurst()
    {
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        float startAngle = -burstAngle / 2f;
        float angleStep = burstAngle / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
            Quaternion spreadRot = rotation * Quaternion.Euler(0f, angle, 0f);
            Vector3 shootDirection = spreadRot * Vector3.forward;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = shootDirection * projectileSpeedPhase3;
        }
        if (animator != null)
            animator.SetTrigger("Attack");
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            TakeDamage(1);
            Destroy(other);
        }
    }

    private void TakeDamage(int damage)
    {
        if (isImmune == false) 
        {
            currentHealth -= damage;
        }
        

        if (currentHealth < 0)
        {
            Die();
        }
    }

    private void Die() 
    {
        Destroy(gameObject);
        Time.timeScale = 0f;
        winScreen.SetActive(true);
    }
}
