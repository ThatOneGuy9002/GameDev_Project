using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyDasher : MonoBehaviour
{
    public float triggerRange = 10f;
    public float chargeTime = 1f;
    public float dashForce = 20f;
    public float dashDuration = 0.3f;
    public float slideFriction = 2f;
    public float cooldownTime = 2f;
    public float moveSpeed = 3f;
    public float knockbackForce = 10f;

    private Transform player;
    private Rigidbody rb;

    private enum DashState { Idle, Charging, Dashing, Sliding, Cooldown }
    private DashState currentState = DashState.Idle;

    private float stateTimer = 0f;
    private Vector3 dashDirection;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case DashState.Idle:
                if (distance <= triggerRange)
                {
                    StartCharging();
                }
                else
                {
                    MoveTowardPlayer();
                }
                break;

            case DashState.Charging:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    StartDash();
                }
                break;

            case DashState.Dashing:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    StartSliding();
                }
                break;

            case DashState.Sliding:
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * slideFriction);
                if (rb.velocity.magnitude < 0.1f)
                {
                    EndSlide();
                }
                break;

            case DashState.Cooldown:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    currentState = DashState.Idle;
                }
                break;
        }
    }

    void MoveTowardPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 move = direction * moveSpeed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    void StartCharging()
    {
        rb.velocity = Vector3.zero;
        dashDirection = (player.position - transform.position).normalized;
        currentState = DashState.Charging;
        stateTimer = chargeTime;
        Debug.Log("Charging...");
    }

    void StartDash()
    {
        rb.velocity = dashDirection * dashForce;
        currentState = DashState.Dashing;
        stateTimer = dashDuration;
        Debug.Log("Dashing!");
    }

    void StartSliding()
    {
        currentState = DashState.Sliding;
        Debug.Log("Sliding...");
    }

    void EndSlide()
    {
        rb.velocity = Vector3.zero;
        currentState = DashState.Cooldown;
        stateTimer = cooldownTime;
        Debug.Log("Cooldown...");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            RigidbodyMovement movement = collision.gameObject.GetComponent<RigidbodyMovement>();
            if (ph != null)
            {
                ph.TakeDamage(1);
            }

            if (movement != null)
            {
                Vector3 knockbackDir = (collision.gameObject.transform.position - transform.position).normalized;
                knockbackDir.y = 0f;
                movement.ApplyKnockback(knockbackDir, knockbackForce, 0.1f);
            }

            Debug.Log("Dasher hit player with knockback!");
        }
    }
}