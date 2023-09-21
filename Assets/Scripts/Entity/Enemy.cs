using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI_MODE {
    RANDOM_WALK,
    CHASE_PLAYER,
    FLEE_PLAYER,
    PATROL,
    IDLE
}

public class Enemy : Entity {

    public float speed = 1f;
    public float rotationDamping = 10f;
    public float agroDistance = 10f;
    public float forgetDistance = 20f;
    public float fieldOfView = 45f;
    public GameObject player;

    protected float health = 100f;

    protected AI_MODE aiMode = AI_MODE.RANDOM_WALK;

    private float desiredRot = 0;

    [Range (0, 360)]
    public float directionDelta = 1f;

    // Start is called before the first frame update
    protected override void Start () {
        base.Start ();

        aiMode = AI_MODE.IDLE;
    }

    // Update is called once per frame
    void Update () {
        Transform t = GetComponent<Transform> ();
        //Keep him upright
        Vector3 rotation = t.rotation.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        t.rotation = Quaternion.Euler (rotation);

        switch (aiMode) {
            case AI_MODE.RANDOM_WALK:
                RandomWalk ();
                break;
            case AI_MODE.CHASE_PLAYER:
                ChasePlayer ();
                break;
            case AI_MODE.FLEE_PLAYER:
                FleePlayer ();
                break;
            case AI_MODE.PATROL:
                Patrol ();
                break;
            case AI_MODE.IDLE:
                Idle ();
                break;
        }

        if (health <= 0) {
            Destroy (gameObject);
        }
    }

    void RandomWalk () {
        if (Random.Range (0, 10000) == 0)
            aiMode = AI_MODE.IDLE;

        Transform t = GetComponent<Transform> ();
        desiredRot += Random.Range (-directionDelta, directionDelta);
        t.rotation = Quaternion.Lerp (t.rotation, Quaternion.Euler (0, desiredRot, 0), Time.deltaTime * rotationDamping);
        t.position += t.forward * speed * Time.deltaTime;

        // Check if we should chase player
        if (Vector3.Distance (t.position, player.transform.position) < agroDistance) {
            aiMode = AI_MODE.CHASE_PLAYER;
        }
    }

    void ChasePlayer () {
        Transform t = GetComponent<Transform> ();
        Vector3 direction = player.transform.position - t.position;
        float angle = Vector3.Angle (t.forward, direction);

        Quaternion targetRotation = Quaternion.LookRotation (direction);
        t.rotation = Quaternion.RotateTowards (t.rotation, targetRotation, angle);
        t.position += t.forward * speed * Time.deltaTime;

        // Check if we should forget player
        if (Vector3.Distance (t.position, player.transform.position) > forgetDistance) {
            if (Random.Range (0, 100) < 50)
                aiMode = AI_MODE.IDLE;
            else
                aiMode = AI_MODE.RANDOM_WALK;
        }
    }

    void FleePlayer () {
        Transform t = GetComponent<Transform> ();
        Vector3 direction = player.transform.position - t.position;
        float angle = Vector3.Angle (t.forward, direction);

        Quaternion targetRotation = Quaternion.LookRotation (direction);
        t.rotation = Quaternion.RotateTowards (t.rotation, targetRotation, -angle);
        t.position += t.forward * speed * Time.deltaTime;
    }

    void Patrol () {

    }

    void Idle () {
        int roll = Random.Range (0, 10000);

        if (roll == 0) {
            aiMode = AI_MODE.RANDOM_WALK;
            Debug.Log ("Starting walk");
        } else if (roll < 10) {
            desiredRot = Random.Range (0, 360);
        }

        Transform t = GetComponent<Transform> ();
        t.rotation = Quaternion.Lerp (t.rotation, Quaternion.Euler (0, desiredRot, 0), Time.deltaTime * rotationDamping);

        // Check if the player is in cone of vision
        Vector3 direction = player.transform.position - t.position;
        float angle = Vector3.Angle (t.forward, direction);
        float distance = Vector3.Distance (t.position, player.transform.position);
        if (angle < fieldOfView && distance < agroDistance) {
            aiMode = AI_MODE.CHASE_PLAYER;
        }
    }

    void OnCollisionEnter (Collision collision) {
        // If we hit a wall, turn around
        if (collision.gameObject.tag == "GameController") {
            Transform t = GetComponent<Transform> ();
            desiredRot += 180;
        }
    }

    void OnDrawGizmos () {
        // Draw line going forward
        Gizmos.color = Color.red;
        Gizmos.DrawLine (transform.position, transform.position + transform.forward * 2f);
    }
}