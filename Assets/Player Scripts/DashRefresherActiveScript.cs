using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRefresherActiveScript : MonoBehaviour
{
    private Renderer renderer;
    private CircleCollider2D triggerbox;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        triggerbox = GetComponent<CircleCollider2D>();
        StartCoroutine("Activate");
    }

    private IEnumerator Activate()
    {
        yield return new WaitForSeconds(0.75f);
        triggerbox.enabled = true;
        renderer.material.SetColor("_Color", Color.green);
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
