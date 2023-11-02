using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnDestroyShoot : MonoBehaviour
{
    public GameObject Proj;
    public GameObject FoundEnemy;
    public void Shoot()
    {
            FoundEnemy = GameObject.FindWithTag("Enemy");
            Vector2 shootDirection = FoundEnemy.transform.position - gameObject.transform.position;
            float rotationAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            var instanceProj = Instantiate(Proj, transform.position, Quaternion.Euler(0f, 0f, rotationAngle));
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
