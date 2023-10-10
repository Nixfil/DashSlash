using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float WalkSpeed = 5f;
    [SerializeField] private float dashSpeed = 80f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCD = 3f;
    [SerializeField] private bool controlling = true;
    [SerializeField] private float currentDistance;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool hitDashRefresher = false;
    private Input_Handler IHS;
    private Camera camera;
    private Rigidbody2D rb;
    private Vector3 dashTarget;
    public float experiment;
   // public GameObject trail;

    private void Awake()
    {
        IHS = GetComponent<Input_Handler>();
        rb = GetComponent<Rigidbody2D>();
        camera = Camera.main;

    }
    private void Update()
    {
        if (controlling)
        {
            Move();
        }
        if (IHS.dashInput && !isDashing && canDash)
        {
            PrepareDash();
        }
        if(isDashing)
        {
            CalculateDistance();
        }
    }
    private void Move()
    {
        rb.velocity = new Vector2(IHS.horizontalInput * WalkSpeed, rb.velocity.y);
    }
    private void PrepareDash()
    {
        dashTarget = camera.ScreenToWorldPoint(Input.mousePosition);
        dashTarget.z = transform.position.z;
        rb.gravityScale = 0.0f;
       // Instantiate(trail, this.transform);
        StartCoroutine("Dash");
    }
    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
            while (transform.position != dashTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null;
            }
        Debug.Log("Seks");
        isDashing = false;
       // Destroy(trail);
        rb.gravityScale = 1f;
        yield return new WaitForSeconds(dashCD);
        RefreshDash ();
    }
    public void GainControl()
    {
        controlling = true;
    }
    public void StopControl()
    {
        controlling = false;
    }
    public void CalculateDistance()
    {
        currentDistance = Vector2.Distance(transform.position, dashTarget);
    }
    public void RefreshDash()
    {
        canDash = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DashRefresher")
        { 
            RefreshDash();
            Destroy(collision.gameObject);
            hitDashRefresher = true;

        }
    }
}

