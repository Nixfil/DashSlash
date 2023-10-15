using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
   [SerializeField] private int HP = 5;
    private float movespeed = 5f;
    private void Update()
    {
        if (HP <= 0) Destroy(this.gameObject);
    }
    private void LoseHealth()
    {
        HP -= 1;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            
            LoseHealth();

            
            Destroy(collision.gameObject);
        }
    }
}
