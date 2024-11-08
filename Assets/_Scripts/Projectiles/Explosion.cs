using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : Projectile
{

    public override void Explode()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Monster mon = other.gameObject.GetComponent<Monster>();
        if (mon != null)
        {
            mon.takeDamage(damage);
        }
    }

}
