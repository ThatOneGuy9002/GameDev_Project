using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMovement : MonoBehaviour
{
    private Vector3 PlayerMovementInput;

    [SerializeField] private Rigidbody PlayerBody;

    [SerializeField] private float Speed;
    [SerializeField] private float Jumpforce;

    Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    private void Update()
    {
        // Get input from player
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // Move the player
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Create movement vector in world space (ignores rotation)
        Vector3 MoveVector = new Vector3(PlayerMovementInput.x, 0f, PlayerMovementInput.z) * Speed;
        var skewedInput = matrix.MultiplyPoint3x4(MoveVector);

        // Set velocity, keeping the player's y velocity (gravity, jump) intact
        PlayerBody.velocity = new Vector3(skewedInput.x, PlayerBody.velocity.y, skewedInput.z);
    }
}