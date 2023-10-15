using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyShoot : MonoBehaviour
{
    public GameObject proj;
    private void OnDestroy()
    {
        GameObject foundEnemy = GameObject.FindWithTag("Enemy");
        Vector2 shootDirection = foundEnemy.transform.position - gameObject.transform.position;
        float rotationAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        var instanceProj = Instantiate(proj, transform.position, Quaternion.Euler(0f, 0f, rotationAngle));
        Rigidbody2D rb = instanceProj.GetComponent<Rigidbody2D>();
        rb.AddForce(shootDirection * 5f, ForceMode2D.Impulse);
        StartCoroutine(DestroyIfNoHit(instanceProj));
    }
    IEnumerator DestroyIfNoHit(GameObject proj)
    {
        yield return new WaitForSeconds(2);
        Destroy(proj);
    }
}
