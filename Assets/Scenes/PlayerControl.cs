using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour {
    public float speed;
    private Vector2 move;

    public void OnMove (InputValue value) {
        move = value.Get<Vector2> ();
    }

    void Start () {

    }

    void Update () {
        if (move.sqrMagnitude > 0.1f) {
            Vector3 movement = new Vector3 (move.x, 0f, move.y);
            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (movement), 0.15f);
            transform.Translate (movement * speed * Time.deltaTime, Space.World);
            Debug.Log (move);
        }
    }
}