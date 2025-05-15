using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartUp : MonoBehaviour
{
    public void ResetStats() 
    {
        PlayerStats.Instance.health = 5;
    }
}
