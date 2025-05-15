using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovement : MonoBehaviour
{
    private Vector3 PlayerMovementInput;
    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private float Speed;
    [SerializeField] private float Jumpforce;

    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private float knockbackMinSpeed = 0.005f;
    private Vector3 lastAppliedKnockback;

    Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    private void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0f && PlayerBody.velocity.magnitude <= knockbackMinSpeed)
            {
                isKnockedBack = false;
            }

            return; 
        }

        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = new Vector3(PlayerMovementInput.x, 0f, PlayerMovementInput.z) * Speed;
        var skewedInput = matrix.MultiplyPoint3x4(MoveVector);

        PlayerBody.velocity = new Vector3(skewedInput.x, PlayerBody.velocity.y, skewedInput.z);
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        isKnockedBack = true;
        knockbackTimer = duration;

        direction.y = 0f;
        lastAppliedKnockback = direction.normalized * force;

        PlayerBody.velocity = Vector3.zero; 
        PlayerBody.AddForce(lastAppliedKnockback, ForceMode.Impulse);
    }
}