using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTerrainHit : MonoBehaviour
{

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Terrain")
        {
            Destroy(this.gameObject);
        }
    }
}
