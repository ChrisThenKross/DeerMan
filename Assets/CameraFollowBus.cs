using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBus : MonoBehaviour
{
    public Transform target;
    public Transform Bus;
    public float smoothTime = 0.3f;
    public Vector3 offset;
    public Vector3 rotation;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 center = ((Bus.position - target.position)/2.0f) + target.position;
            Vector3 targetPosition = center + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // rotation is a Vector3 of degrees
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
