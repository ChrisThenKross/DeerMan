using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    Animator animator;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attack
    public float timeBetweenAttack;
    bool alreadyAttacked;
    public BoxCollider boxCollider;
    //phase 2 attack
    public GameObject projectile;
    [SerializeField] private Transform throwPoint;
    bool spawnEnemies = true;
    public GameObject enemy;

    //States
    public float sightRange, attackRangePhase1, attackRangePhase2;
    public bool playerInSightRange, playerInAttackRange1, playerInAttackRange2;
    private int health;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        //boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange1 = Physics.CheckSphere(transform.position, attackRangePhase1, whatIsPlayer);
        playerInAttackRange2 = Physics.CheckSphere(transform.position, attackRangePhase2, whatIsPlayer);
        // get current health to indicate what phase its in
        health = GetComponent<Health>().currentHealth;

        if (playerInSightRange && !playerInAttackRange1) ChasePlayer();
        if (playerInSightRange && playerInAttackRange1 && health > 250) AttackPlayerPhaseOne();
        if (playerInSightRange && playerInAttackRange2 && health <= 250) AttackPlayerPhaseTwo();
    }


    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }


    private void ChasePlayer()
    {
        animator.SetTrigger("Walk");
        agent.SetDestination(player.position);
    }

    private void AttackPlayerPhaseOne()
    {
        //Make surer enemy doesn't move
        agent.SetDestination(transform.position);

        //transform.LookAt(player);
        Vector3 direction = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 25f * Time.deltaTime);

        //This will count as the attack, since theres a hitbox here
        animator.SetTrigger("Attack");


        if (!alreadyAttacked)
        {
            //Animation is already going so we are just going to detect collision
            //Attack code here
            // see ontriggerenter

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), 0);
        }
    }

    private void AttackPlayerPhaseTwo()
    {
        //Make surer enemy doesn't move
        agent.SetDestination(transform.position);

        //transform.LookAt(player);
        Vector3 direction = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 25f * Time.deltaTime);

        //This will count as the attack, since theres a hitbox here
        //animator.SetTrigger("Attack");

        // Spawn enemies
        Vector3 basePosition = transform.position;
        if (spawnEnemies)
        {
            spawnEnemies = false;
            for (int i = 0; i < 5; i++)
            {
                Vector3 position = basePosition + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                Instantiate(enemy, position, Quaternion.identity);
            }
        }    



        if (!alreadyAttacked)
        {
            //Animation is already going so we are just going to detect collision
            //Attack code here
            Rigidbody rb = Instantiate(projectile, throwPoint.position, throwPoint.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 16f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRangePhase1);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRangePhase2);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    /*    private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("RACTHC!!!");
            }
        }*/

    // THIS DOESNT WORK!!! IT DOESN'T DETECT THE BOX COLLIDER????////
    // I MOVED ALL THE LOGIC TO THE FREAKING ARM INSTEAD!!! HOW AWFUL!
}
