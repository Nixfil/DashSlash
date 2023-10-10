using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRefresherLauncher : MonoBehaviour
{
    public GameObject DashRefresherPrefab;
    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var instance= Instantiate(DashRefresherPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb=instance.GetComponent<Rigidbody2D>();
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Ensure the same Z position as the object.

            Vector2 direction = (mousePosition - transform.position).normalized;

            // Apply the launch force to the object's Rigidbody2D.
            rb.AddForce(direction * 10f, ForceMode2D.Impulse);
        }
    }

}
