using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public int lifeTime;
    private void Update()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
