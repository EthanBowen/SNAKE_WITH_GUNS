using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction = Vector2.right;

    public float speed = 0.2f;

    private float timeElapsed = 0;
    public float duration = 2.0f;

    public int damage;

    // Update is called once per frame
    void Update()
    {
        transform.right = direction;
        transform.position += direction * Time.deltaTime * speed;

        timeElapsed += Time.deltaTime;
        if(timeElapsed > duration)
        {
            Explode();
        }
    }

    public virtual void Explode()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Monster mon = other.gameObject.GetComponent<Monster>();
        if (mon != null)
        {
            mon.takeDamage(damage);
            Explode();
        }
    }

}
