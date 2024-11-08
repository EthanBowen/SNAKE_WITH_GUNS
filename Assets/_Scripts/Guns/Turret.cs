using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : Gun
{
    [SerializeField] Transform barrel;

    MonsterSpawner monsterSpawnerRef;

    private Transform target;
    public bool findTarget = true;

    // Start is called before the first frame update
    void Start()
    {
        if(barrel == null)
            barrel = transform;

        monsterSpawnerRef = GameObject.FindGameObjectWithTag("MonsterSpawner").GetComponent<MonsterSpawner>();

        if(monsterSpawnerRef)
            StartCoroutine(GetTarget());
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (target != null)
        {
            transform.right = target.position - transform.position; 
        }
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
        }
    }

    IEnumerator GetTarget()
    {
        while(findTarget)
        {
            if (monsterSpawnerRef.monstersOnField.Count > 0)
            {
                target = monsterSpawnerRef.monstersOnField[0].transform;
                foreach (Monster mon in monsterSpawnerRef.monstersOnField)
                {
                    if (Vector3.Distance(transform.position, mon.transform.position) < Vector3.Distance(transform.position, target.position))
                    {
                        target = mon.transform;
                    }
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
        
        yield return null;
        
    }
}
