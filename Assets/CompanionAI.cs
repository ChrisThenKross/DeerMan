using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompanionAI : Entity
{
    public NavMeshAgent agent;
    private Transform player;
    private GameObject[] enemies;
    public LayerMask Enemy;
    public float lifetime = 6.0f;
    Animator animator;

    //attack
    public float timeBetweenAttack;
    private bool alreadyAttacked;
    public GameObject projectile;
    [SerializeField] private Transform throwPoint;

    //States
    public float attackRange, sightRange;
    private bool playerInAttackRange;

    private void Awake()
    {
        //enemies = GameObject.FindGameObjectsWithTag("EnemyParent");
        //int num = Random.Range(0, enemies.Length);

        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange);
        List<Collider> Parents = new List<Collider>();

        foreach(Collider col in colliders)
        {
            Debug.Log(col.gameObject.tag);
            if (col.gameObject.tag == "EnemyParent")
                Parents.Add(col);
                //player = col.gameObject.transform;
        }
        int num = Random.Range(0, Parents.Capacity);
        player = Parents[num].transform;

        //if (player == null) player = GameObject.Find("Player").transform;




/*        if (enemies.Length <= 0)
        {
            Debug.Log("no enemies, returning");
            return;
        }
        if (enemies[num] != null)
        {
            player = enemies[num].transform;
        }
        else
        {
            player = GameObject.Find("Player").transform;
        }*/
        //Debug.Log("I chose " + enemies[num].name);
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        StartCoroutine(Life());
    }

    IEnumerator Life()
    {
        yield return new WaitForSeconds(lifetime);
        Health kill = gameObject.GetComponent<Health>();
        kill.TakeDamage(99999);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void Update()
    {
        //check for sight and attack range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Enemy);

        if (!playerInAttackRange) ChasePlayer();
        else AttackPlayer();
    }


    private void ChasePlayer()
    {
        animator.SetTrigger("Walk");
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make surer enemy doesn't move
        agent.SetDestination(transform.position);

        //transform.LookAt(player);
        Vector3 direction = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 25f * Time.deltaTime);

        animator.SetTrigger("Attack");


        if (!alreadyAttacked)
        {
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
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
