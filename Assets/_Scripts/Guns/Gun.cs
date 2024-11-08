using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectilePrefab;

    protected float timeSinceLastShot = 0;
    public float coolDown = 1.0f;

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public virtual void Shoot()
    {
        if(timeSinceLastShot > coolDown)
        {
            if (projectilePrefab != null)
            {
                ShotPattern();
            }

            timeSinceLastShot = 0;
        }
    }

    public virtual void ShotPattern()
    {
        GameObject obj = GameObject.Instantiate(projectilePrefab, transform.position, transform.rotation);

        Projectile proj = obj.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.direction = transform.right;
        }
    }
}
