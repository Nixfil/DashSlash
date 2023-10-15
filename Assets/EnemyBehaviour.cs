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



    private GameObject player;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PSS.LoseHP(contactDamage);
        }
    }
}
