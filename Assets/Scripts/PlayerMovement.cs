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
    void Start()
    {
        m_charCont = GetComponent<CharacterController>();
    }

    void Update()
    {
        m_horizontal = Input.GetAxis("Horizontal");
        m_vertical = Input.GetAxis("Vertical");



        Vector3 m_playerMovement = new Vector3(m_horizontal, 0f, m_vertical) * PlayerSpeed * Time.deltaTime;

        var skewedInput = matrix.MultiplyPoint3x4(m_playerMovement);

        m_charCont.Move(skewedInput);
    }
}
