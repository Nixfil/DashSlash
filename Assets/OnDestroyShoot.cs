using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyShoot : MonoBehaviour
{
    public GameObject proj;
    private void OnDestroy()
    {
        GameObject foundEnemy = GameObject.FindWithTag("Enemy");
        Vector2 shootDirection = gameObject.transform.position - foundEnemy.transform.position;
        var instanceProj = Instantiate(proj, transform.position, Quaternion.identity);
        Rigidbody2D rb = instanceProj.GetComponent<Rigidbody2D>();
        rb.AddForce(shootDirection * 5f, ForceMode2D.Impulse);
    }
}
