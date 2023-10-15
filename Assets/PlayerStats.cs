using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int hp = 5;
    public float movespeed = 5f;
    public void LoseHP(int damage)
    {
        hp = hp - damage;
    }
    private void Die()
    {
        Destroy(this.gameObject); 
    }
}
