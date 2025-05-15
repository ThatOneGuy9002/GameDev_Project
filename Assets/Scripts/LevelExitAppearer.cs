using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject objectToAppear; // Reference to the object that appears

    void Update()
    {
        // Find all GameObjects with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // If no enemies are found, activate the object
        if (enemies.Length == 0)
        {
            objectToAppear.SetActive(false);
        }
    }
}
