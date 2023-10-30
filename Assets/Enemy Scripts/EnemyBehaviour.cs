using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBehaviour;

public class EnemyBehaviour : MonoBehaviour
{
    public enum EnemyType
    {
        Shooter,
        TriShooter,
        Follower,
        HoppingFollower,
        ShieldedShooter
    }


    [SerializeField] private int contactDamage;
    [SerializeField] private int health;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float patrolDistance = 5.0f;
    [SerializeField] private bool isMovingRight = true;
    [SerializeField] private float projSpeed=9f;
    [SerializeField] private bool patrolling;
    [SerializeField] private float hopForce;
    [SerializeField] private Vector3 hopDirection;

    private Vector3 spawnPosition;
    private GameObject player;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    public GameObject EnemyProjGO;
    public EnemyType enemyType;
    public PlayerStats PSS;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        PSS = player.GetComponent<PlayerStats>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        patrolling = true;

        contactDamage = GetContactDamage(enemyType);
        health = GetHealth(enemyType);

        spawnPosition = transform.position;
        switch(enemyType)
        {
            case EnemyType.Shooter:
                StartCoroutine("StopAndShootProjectile");
                break;
            case EnemyType.TriShooter:
                StartCoroutine("StopAndTriShootProjectile");
                break;
            case EnemyType.HoppingFollower:
                rb=gameObject.AddComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                StartCoroutine("StopAndHop");
                break;
        }
    }

    private int GetContactDamage(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Shooter:
                return 1;
            case EnemyType.TriShooter:
                return 1;
            case EnemyType.Follower:
                return 2;
            case EnemyType.HoppingFollower:
                return 2;
            case EnemyType.ShieldedShooter:
                return 1;
            default:
                return 0; // Default value if the type is not recognized
        }
    }
    private int GetHealth(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Shooter:
                return 3;
            case EnemyType.TriShooter:
                return 3;
            case EnemyType.Follower:
                return 2;
            case EnemyType.HoppingFollower:
                return 6;
            case EnemyType.ShieldedShooter:
                return 5;
            default:
                return 3; // Default value if the type is not recognized
        }
    }
    private void Update()
    {
        if (health <= 0) Destroy(this.gameObject);


        //Switch for all the actions


        switch (enemyType)
        {
            case EnemyType.Shooter:
                if(patrolling) Patrol();
                break;
            case EnemyType.TriShooter:
                if(patrolling) Patrol();
                break;
            case EnemyType.Follower:
                 FollowPlayer();
                break;
            case EnemyType.HoppingFollower:
                ChangeDirectionOfHopping();
                break;

            default:
                Debug.LogError("Invalid enemy type: " + enemyType);
                break;
        }
    }
    private void Patrol()
    {
        Vector3 patrolPosition = spawnPosition + (isMovingRight ? Vector3.right : Vector3.left) * patrolDistance;

        // Move the enemy towards the target position
        MoveTo(patrolPosition);

        // Check if the enemy has reached the target
        float distanceToTarget = Vector3.Distance(transform.position, patrolPosition);
        if (distanceToTarget < 0.1f)
        {
            // Switch direction when reaching the target
            isMovingRight = !isMovingRight;
        }
    }
    private void MoveTo(Vector3 patrolPosition)
    {
        // Calculate the direction to the target
        Vector3 direction = (patrolPosition - transform.position).normalized;

        // Move the enemy
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
    IEnumerator StopAndShootProjectile()
    {   
        yield return new WaitForSeconds(2);
        patrolling = false;
        Vector2 shootDirection = player.transform.position - transform.position;
        var enemyProj = Instantiate(EnemyProjGO, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(enemyProj.GetComponent<CapsuleCollider2D>(), boxCollider);
        Rigidbody2D rbProj = enemyProj.GetComponent<Rigidbody2D>();
        rbProj.velocity = shootDirection * projSpeed;
        yield return new WaitForSeconds(0.5f);
        patrolling = true;
        StartCoroutine(StopAndShootProjectile());
    }
    IEnumerator StopAndTriShootProjectile()
    {  
        yield return new WaitForSeconds(2);
        patrolling = false;
        Vector2 shootDirection = (player.transform.position - transform.position).normalized;
        float angleOffset = 20f; // Angle offset in degrees

        for (int i = 0; i < 3; i++)
        {

            float angle = (i - 1) * angleOffset;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * shootDirection;


            var enemyProj = Instantiate(EnemyProjGO, transform.position, Quaternion.identity);


            Physics2D.IgnoreCollision(enemyProj.GetComponent<CapsuleCollider2D>(), boxCollider);

            Rigidbody2D rbProj = enemyProj.GetComponent<Rigidbody2D>();
            rbProj.velocity = direction * projSpeed;
        }
        yield return new WaitForSeconds (0.5f);
        patrolling = true;

        StartCoroutine(StopAndTriShootProjectile());
    }
    IEnumerator StopAndHop()
    {
        yield return new WaitForSeconds(2f);
        Vector3 leftHopPoint = transform.Find("LeftHopPoint").position;
        Vector3 rightHopPoint = transform.Find("RightHopPoint").position;
        if (isMovingRight)
        {
            hopDirection= rightHopPoint - transform.position;
        }
        else 
        { 
            hopDirection =  leftHopPoint - transform.position;
        }
        rb.AddForce(hopDirection * hopForce, ForceMode2D.Impulse);
        StartCoroutine(StopAndHop());
    }
    public void ChangeDirectionOfHopping()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 1f, ~(1 << 6));
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 1f, ~(1 << 6));

        // Check if terrain was hit on the left or right side.
        if (hitLeft.collider != null && hitLeft.collider.CompareTag("Terrain"))
        {
            // Terrain on the left side, set isMovingRight to true.
            isMovingRight = true;
        }
        else if (hitRight.collider != null && hitRight.collider.CompareTag("Terrain"))
        {
            // Terrain on the right side, set isMovingRight to false.
            isMovingRight = false;
        }
        Debug.Log(hitRight.collider);
    }
    public void FollowPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;

        // Move the enemy towards the player.
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    public void LoseHealth(int damage)
    {
        health -= damage;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PSS.LoseHP(contactDamage);
        }
        else if (collision.gameObject.tag =="Projectile")
        {
            LoseHealth(1);
        }
    }
  
}