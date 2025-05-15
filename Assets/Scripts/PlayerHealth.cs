using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public GameObject gameOverScreen;
    public GameObject healthScreen;
    public void TakeDamage(int amount)
    {
        PlayerStats.Instance.health -= amount;
        Debug.Log("Player took damage! Health: " + PlayerStats.Instance.health);

        if (PlayerStats.Instance.health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        healthScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
