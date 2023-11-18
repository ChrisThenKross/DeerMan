using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusAI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform player;


    public LayerMask whatIsGround, whatIsPlayer;

    Animator animator;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attack
    public float timeBetweenAttack;
    private bool alreadyAttacked = true;
    private bool attacking = false;
    Rigidbody rb;
    public GameObject attackIndicator;

    //States
    public float sightRange, attackRangePhase1, attackRangePhase2;
    public bool playerInSightRange, playerInAttackRange1, playerInAttackRange2;
    private int health;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
   
    }

    private void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange1 = Physics.CheckSphere(transform.position, attackRangePhase1, whatIsPlayer);
        playerInAttackRange2 = Physics.CheckSphere(transform.position, attackRangePhase2, whatIsPlayer);
        // get current health to indicate what phase its in
        health = GetComponent<Health>().currentHealth;

        if (playerInSightRange && !playerInAttackRange1 && alreadyAttacked) ChasePlayer();
        if (playerInSightRange && playerInAttackRange1 && alreadyAttacked) ChasePlayer(); //moving while cooldown
        if (playerInSightRange && playerInAttackRange1 && !attacking) StartCoroutine(ChargedAttack());
    }


    IEnumerator ChargedAttack()
    {
        alreadyAttacked = false;
        transform.LookAt(player);
        agent.ResetPath();
        attackIndicator.SetActive(true);
        attacking = true;
        yield return new WaitForSeconds(1);
        Debug.Log("I have waited! CHARGE!!!!");
        attackIndicator.SetActive(false);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 62000, ForceMode.Impulse);
        yield return new WaitForSeconds(2);
        rb.isKinematic = true;
        alreadyAttacked = true;
        yield return new WaitForSeconds(10);
        Debug.Log("Done! Can attack again");
        attacking = false;

    }

    private void ChargeAttack()
    {
        alreadyAttacked = false;
        transform.LookAt(player);
        agent.ResetPath();
    }


    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
    }


    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRangePhase1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (attacking && collision.gameObject.layer == 10)
        {
            Debug.Log("BLOCKIGN");
            Physics.IgnoreCollision(collision.collider.GetComponent<Collider>(), GetComponent<Collider>());
        }    
    }

}
