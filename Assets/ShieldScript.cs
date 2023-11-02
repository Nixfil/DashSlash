using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    [SerializeField] private int HP=3;


    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) {
            Destroy(this.gameObject);
        }
    }
    public void LoseHealth(int damage)
    {
        HP -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            LoseHealth(1);
        }
    }

}
