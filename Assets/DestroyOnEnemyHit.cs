using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEnemyHit : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(this.gameObject);
        }
    }
}
