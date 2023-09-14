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

public class Enemy : MonoBehaviour {

    public float speed = 1f;
    public float rotationDamping = 10f;
    public GameObject player;

    protected float health = 100f;

    protected AI_MODE aiMode = AI_MODE.RANDOM_WALK;
    private float desiredRot = 0;

    [Range (0, 360)]
    public float directionDelta = 1f;

    // Start is called before the first frame update
    void Start () {
        aiMode = AI_MODE.RANDOM_WALK;
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

        // Randomly change rotation
        desiredRot += Random.Range (-directionDelta, directionDelta);

        // Apply rotation
        t.rotation = Quaternion.Lerp (t.rotation, Quaternion.Euler (0, desiredRot, 0), Time.deltaTime * rotationDamping);

        // Move forward
        t.position += t.forward * speed * Time.deltaTime;
    }

    void ChasePlayer () {
        Transform t = GetComponent<Transform> ();
        // Get current rotation
        Vector3 rotation = t.rotation.eulerAngles;

        // Get direction to player
        Vector3 direction = player.transform.position - t.position;

        // Get angle to player
        float angle = Vector3.Angle (t.forward, direction);

        // If angle is greater than directionDelta, rotate
        if (angle > directionDelta) {
            // Get rotation to player
            Quaternion targetRotation = Quaternion.LookRotation (direction);

            // Rotate towards player
            t.rotation = Quaternion.RotateTowards (t.rotation, targetRotation, directionDelta);
        }

        // Move forward
        t.position += t.forward * speed * Time.deltaTime;
    }

    void FleePlayer () {
        Transform t = GetComponent<Transform> ();
        // Get current rotation
        Vector3 rotation = t.rotation.eulerAngles;

        // Get direction to player
        Vector3 direction = player.transform.position - t.position;

        // Get angle to player
        float angle = Vector3.Angle (t.forward, direction);

        // If angle is greater than directionDelta, rotate
        if (angle > directionDelta) {
            // Get rotation to player
            Quaternion targetRotation = Quaternion.LookRotation (direction);

            // Rotate away from player
            t.rotation = Quaternion.RotateTowards (t.rotation, targetRotation, -directionDelta);
        }

        // Move forward
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