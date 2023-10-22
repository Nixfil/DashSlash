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



    [SerializeField] private float movespeed = 2f;
    [SerializeField] private int contactDamage;
    [SerializeField] private int health;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float patrolDistance = 5.0f;
    [SerializeField] private bool isMovingRight = true;
    [SerializeField] private float projSpeed;

    private Vector3 spawnPosition;
    private GameObject player;
    private BoxCollider2D boxCollider;


    public GameObject EnemyProjGO;
    public EnemyType enemyType;
    public PlayerStats PSS;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        PSS = player.GetComponent<PlayerStats>();
    }
    private void Start()
    {
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
                return 5;
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
        switch (enemyType)
        {
            case EnemyType.Shooter:
                Patrol();
                break;
            case EnemyType.TriShooter:
                Patrol();
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
        Vector2 shootDirection = player.transform.position - transform.position;
        var enemyProj = Instantiate(EnemyProjGO, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(enemyProj.GetComponent<CapsuleCollider2D>(), boxCollider);
        Rigidbody2D rbProj = enemyProj.GetComponent<Rigidbody2D>();
        rbProj.velocity = shootDirection * projSpeed;
        StartCoroutine(StopAndShootProjectile());
    }
    IEnumerator StopAndTriShootProjectile()
    {
        yield return new WaitForSeconds(2);
        Vector2 shootDirection = (player.transform.position - transform.position).normalized;
        float angleOffset = 30f; // Angle offset in degrees

        for (int i = 0; i < 3; i++)
        {

            float angle = (i - 1) * angleOffset;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * shootDirection;


            var enemyProj = Instantiate(EnemyProjGO, transform.position, Quaternion.identity);


            Physics2D.IgnoreCollision(enemyProj.GetComponent<CapsuleCollider2D>(), boxCollider);

            Rigidbody2D rbProj = enemyProj.GetComponent<Rigidbody2D>();
            rbProj.velocity = direction * projSpeed;
        }
            StartCoroutine(StopAndTriShootProjectile());
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
