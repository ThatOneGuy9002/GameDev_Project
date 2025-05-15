using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Retry() 
    {
        Time.timeScale = 1f;
        ResetStats();
        SceneManager.LoadScene("Scene0");
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetStats()
    {
        PlayerStats.Instance.health = 5;
    }
}
