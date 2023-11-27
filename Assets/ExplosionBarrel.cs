using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrel : MonoBehaviour
{
    public GameObject Barrel, Explosion;

    private void Awake()
    {
        Barrel.SetActive(true);
        Explosion.SetActive(false);
    }

    public void Explode()
    {
        Barrel.SetActive(false);
        Explosion.SetActive(true);
    }
}
