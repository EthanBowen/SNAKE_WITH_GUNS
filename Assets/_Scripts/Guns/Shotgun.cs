using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public Transform barrel;

    private void Start()
    {
        if(barrel == null)
            barrel = transform;
    }

    public override void ShotPattern()
    {
        if (projectilePrefab != null)
        {
            GameObject obj = GameObject.Instantiate(projectilePrefab, barrel.position, barrel.rotation);

            Projectile proj = obj.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.direction = barrel.right;
            }

            obj = GameObject.Instantiate(projectilePrefab, barrel.position, barrel.rotation);

            proj = obj.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.direction = Quaternion.Euler(0,0,30) * barrel.right;
            }

            obj = GameObject.Instantiate(projectilePrefab, barrel.position, barrel.rotation);

            proj = obj.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.direction = Quaternion.Euler(0, 0, 15) * barrel.right;
            }

            obj = GameObject.Instantiate(projectilePrefab, barrel.position, barrel.rotation);

            proj = obj.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.direction = Quaternion.Euler(0, 0, -15) * barrel.right;
            }

            obj = GameObject.Instantiate(projectilePrefab, barrel.position, barrel.rotation);

            proj = obj.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.direction = Quaternion.Euler(0, 0, -30) * barrel.right;
            }
        }
    }
}
