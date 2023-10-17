using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : Entity {
    public float speed;
    private Vector2 move;

    public void OnMove (InputAction.CallbackContext context) {
        move = context.ReadValue<Vector2> ();
    }

    protected override void Start () {
        base.Start ();
    }

    void Update () {
        if (move.sqrMagnitude > 0.1f) {
            Vector3 movement = new Vector3 (move.x, 0f, move.y);
            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (movement), 0.15f);
            transform.Translate (movement * speed * Time.deltaTime, Space.World);

            // Look in direction of movement
            transform.rotation = Quaternion.LookRotation (movement);

        }
    }
}