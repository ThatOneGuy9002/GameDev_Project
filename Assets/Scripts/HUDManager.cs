using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{

    public TMP_Text healthText;

    

    private void Update()
    {
        int currentHealth = PlayerStats.Instance.health;
        healthText.text = currentHealth.ToString() + " HEALTH";
    }
}
