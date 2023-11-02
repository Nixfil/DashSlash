using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEnemyHit : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine("DestroyFix");
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.tag=="Shield")
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator DestroyFix()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
