using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : Projectile
{
    [SerializeField] GameObject ExplosionPrefab;

    public override void Explode()
    {
        if(ExplosionPrefab != null)
        {
            GameObject exlposion = Instantiate(ExplosionPrefab, transform.position, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * transform.rotation);
            ExplosionController EC = exlposion.GetComponent<ExplosionController>();
            if(EC != null )
            {
                EC.volume = Random.Range(0.2f, 0.3f);
            }
        }
        
        Destroy(gameObject);
    }

}
