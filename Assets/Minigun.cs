using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : MonoBehaviour
{
    Transform Player;
    public Transform head;
    public GameObject boulette;
    public Transform ShootPoint;
    public float fireRate, nextFire;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        head.LookAt(Player);
        if (Time.time >= nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Rigidbody rb = Instantiate(boulette, ShootPoint.position, head.rotation).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
    }
}
