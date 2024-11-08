using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MissileLauncher : Gun
{
    [SerializeField] Transform barrel;

    MonsterSpawner monsterSpawnerRef;

    private Transform target;
    private float distance;
    public bool findTarget = true;

    [SerializeField] private float range = 10;

    // Start is called before the first frame update
    void Start()
    {
        if (barrel == null)
            barrel = transform;

        monsterSpawnerRef = GameObject.FindGameObjectWithTag("MonsterSpawner").GetComponent<MonsterSpawner>();

        if (monsterSpawnerRef)
            StartCoroutine(GetTarget());
        else
            findTarget = false;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            transform.right = dir;
            distance = dir.magnitude;
        }
    }

    public override void ShotPattern()
    {
        if (projectilePrefab != null && distance < range)
        {
            GameObject obj = GameObject.Instantiate(projectilePrefab, barrel.position, barrel.rotation);

            Projectile proj = obj.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.direction = Quaternion.AngleAxis(Random.Range(-5.0f, 5.0f), Vector3.forward) * barrel.right;
            }
        }
    }

    IEnumerator GetTarget()
    {
        while (findTarget)
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
