using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public Transform Player;

    float cameraOffsetZ;
    void Start()
    {
        cameraOffsetZ = gameObject.transform.position.z - Player.position.z;
    }

    void Update()
    {
        Vector3 m_cameraPos = new Vector3(Player.position.x, gameObject.transform.position.y, Player.position.z + cameraOffsetZ);

        gameObject.transform.position = m_cameraPos;
    }
}
