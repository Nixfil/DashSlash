using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 80f;
    [SerializeField] private float dashCD = 3f;
    [SerializeField] private bool controlling = true;
    [SerializeField] private float currentDistance;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private bool canDash = true;
    [SerializeField] private int refresherCount;

    public float dashForce;

    private PlayerStats PS;
    private Input_Handler IHS;
    private OnDestroyShoot ODSS;
    private Camera camera;
    private Rigidbody2D rb;
    private Vector3 dashTarget;

    public float experiment;
    public GameObject DashRefresherPrefab;
    // public GameObject trail;

    private void Awake()
    {
        PS=GetComponent<PlayerStats>();
        IHS = GetComponent<Input_Handler>();
        rb = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }
    private void Start()
    {
        refresherCount = 3;
    }
    private void Update()
    {

        if (controlling)
        {
            Move();
        }
        if (IHS.LeftClick && !isDashing && canDash)
        {
            PrepareDash();
        }
        if(isDashing)
        {
            CalculateDistance();
        }
        if (IHS.RightClick)
        {
            LaunchRefresher();
        }
    }
    private void Move()
    {
        rb.velocity = new Vector2(IHS.MoveInput * PS.movespeed, rb.velocity.y);
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
        /*  isDashing = true;
          canDash = false;
              while (transform.position != dashTarget)
              {
                  transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
              yield return null;
              }
          isDashing = false;
         // Destroy(trail);
          rb.gravityScale = 1f;
          yield return new WaitForSeconds(dashCD);
          RefreshDash ();*/
        isDashing = true;
        canDash = false;

        Vector3 dashDirection = (dashTarget - transform.position).normalized;

        // Apply a force to maintain momentum

        StopControl();
        while (Vector3.Distance(transform.position, dashTarget) > 0.1f)
        {
            // Continue moving towards the target
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null;
        }
        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
        GainControl();
        isDashing = false;
        rb.gravityScale = 1f;

        yield return new WaitForSeconds(dashCD);
        RefreshDash();
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
            ODSS=collision.GetComponent<OnDestroyShoot>();
            ODSS.Shoot();
            Destroy(collision.gameObject);

        }
    }
    private void LaunchRefresher()
    {
        var instance = Instantiate(DashRefresherPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; 

        Vector2 direction = (mousePosition - transform.position).normalized;


        rb.AddForce(direction * 10f, ForceMode2D.Impulse);
    }
}

