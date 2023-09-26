using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 move, mouseLook;
    private Vector3 rotationTarget;
    public Animator anim;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay;
        //transform
        if (move.sqrMagnitude > 0.1f)
        {
            Vector3 movement = new Vector3(move.x, 0f, move.y);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
        }
        // animation
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
    }

    public void movePlayerWithAim()
    {

    }

}
