using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int healthPoints = 3;

    public float speed = 1;

    Transform target;

    [HideInInspector] public MonsterSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (target.position - transform.position).normalized * Time.deltaTime * speed;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void takeDamage(int damage)
    {
        healthPoints -= damage;

        if(healthPoints <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(spawner != null) 
            spawner.monstersOnField.Remove(this);
        Destroy(gameObject);
    }
}
