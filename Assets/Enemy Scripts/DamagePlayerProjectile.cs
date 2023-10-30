using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerProjectile : MonoBehaviour
{
    public PlayerStats PSS;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        PSS = player.GetComponent<PlayerStats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PSS.LoseHP(1);
            Destroy(this.gameObject);
        }
    }
}
