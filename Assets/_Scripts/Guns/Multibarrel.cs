using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multibarrel : Gun
{
    [SerializeField] List<Transform> barrels = new List<Transform>();

    public override void ShotPattern()
    {
        if (projectilePrefab != null)
        {
            for (int i = 0; i < barrels.Count; i++)
            {
                GameObject obj = GameObject.Instantiate(projectilePrefab, barrels[i].position, barrels[i].rotation) ;

                Projectile proj = obj.GetComponent<Projectile>();
                if (proj != null)
                {
                    proj.direction = barrels[i].right;
                }
            }
        }
    }
}
