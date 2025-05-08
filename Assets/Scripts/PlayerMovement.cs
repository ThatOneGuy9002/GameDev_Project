using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController m_charCont;

    float m_horizontal;
    float m_vertical;
    Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public float PlayerSpeed = 0.3f;
    public float gravity = -9.81f;  // Default gravity value (same as Unity's physics gravity)
    public float jumpHeight = 2f;   // Optional: To allow jumping if needed

    private Vector3 velocity;  // To store the player's velocity (for gravity and jumping)

    void Start()
    {
        m_charCont = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get input for movement
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_vertical = Input.GetAxisRaw("Vertical");

        // Calculate the movement vector
        Vector3 inputDirection = new Vector3(m_horizontal, 0f, m_vertical);

        // Normalize to prevent diagonal speed boost
        if (inputDirection.magnitude > 0.1f)
        {
            inputDirection.Normalize();
        }

        // Isometric movement
        Vector3 movement = matrix.MultiplyVector(inputDirection) * PlayerSpeed * Time.deltaTime;

        // Apply gravity to the velocity (affects Y-axis)
        if (m_charCont.isGrounded)
        {
            velocity.y = -2f;  // Keep the character slightly on the ground to prevent floating (slight "stickiness")

            // Jumping logic (optional)
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // Jump velocity calculation
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;  // Apply gravity if not grounded
        }

        // Apply the gravity and move the character
        m_charCont.Move(movement + velocity * Time.deltaTime);
    }
}
