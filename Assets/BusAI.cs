using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusAI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator MinigunAnimator;
    public Animator BladeAnimator;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attack
    public float timeBetweenAttack;
    private bool alreadyAttacked = true;
    private bool attacking = false;
    private bool PhaseTransition = false;
    Rigidbody rb;
    public GameObject attackIndicator;

    //States
    public float chargeRange;
    public float tooClose;
    public bool playerInChargeRange;
    public bool RangeTooClose;
    private int health;
    public bool phaseTwo = false;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerInChargeRange = Physics.CheckSphere(transform.position, chargeRange, whatIsPlayer);
        RangeTooClose = Physics.CheckSphere(transform.position, tooClose, whatIsPlayer);
        // get current health to indicate what phase its in
        health = GetComponent<Health>().currentHealth;

        //Phase 1 Charged Attack
        if (!playerInChargeRange && alreadyAttacked && health>=2500) ChasePlayer();
        if (playerInChargeRange && alreadyAttacked && health >= 2500) ChasePlayer(); //moving while cooldown
        if (playerInChargeRange && !attacking && health >= 2500) StartCoroutine(ChargedAttack());
        //Phase 2 Charged Attack & Turret
        if (health < 2500 & !PhaseTransition)
        {
            phaseTwo = true;
            StartCoroutine(NextPhase());
        }
        if (health < 2500 & PhaseTransition & !RangeTooClose) ChasePlayer();
        if (health < 2500 & PhaseTransition & RangeTooClose) SpinAttack();
    }


    IEnumerator ChargedAttack()
    {
        alreadyAttacked = false;
        agent.ResetPath();
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        transform.LookAt(player);
        attackIndicator.SetActive(true);
        attacking = true;
        Debug.Log(player.position);
        transform.LookAt(player);
        Vector3 target = (player.position + (player.position - transform.position));
        transform.LookAt(player);
        yield return new WaitForSeconds(1);
        Debug.Log("I have waited! CHARGE!!!!");
        transform.LookAt(player);
        attackIndicator.SetActive(false);
        /*while (target != transform.position)*/
        int i = 0;
        //while (Vector3.Distance(transform.position,target) > 10f)


        // this is beyonndddddd scuffed idk how coroutines really work
        while (i < 500)
        {
            i++;
            transform.position = Vector3.MoveTowards(transform.position, target, 25f * Time.deltaTime);
            yield return null;
        }
        //rb.isKinematic = false;
        //rb.AddForce(transform.forward * 62000, ForceMode.Impulse);
        yield return new WaitForSeconds(2);
        //rb.isKinematic = true;
        alreadyAttacked = true;
        yield return new WaitForSeconds(4);
        Debug.Log("Done! Can attack again");
        attacking = false;

    }

    IEnumerator NextPhase()
    {
        agent.ResetPath();
        MinigunAnimator.SetTrigger("ActivateMinigun");
        yield return new WaitForSeconds(2);
        PhaseTransition = true;
    }

    private void SpinAttack()
    {
        BladeAnimator.SetTrigger("BladeGo");
        StartCoroutine(SpinAttackment(2));
    }

    IEnumerator SpinAttackment(float duration)
    {
        agent.ResetPath();
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            yield return null;
        }
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
        BladeAnimator.SetTrigger("BladeNo");
        agent.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chargeRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, tooClose);
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
