using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int gun;
    public GameObject[] gunSprites;

    public float xLimit = 13.7f;
    public float yLimit = 7.5f;

    // Start is called before the first frame update
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        RandomizeGun();
        transform.position = new Vector3(Random.Range(-xLimit, xLimit), Random.Range(-yLimit, yLimit));
    }

    public void RandomizeGun()
    {
        if (gunSprites.Length > 0)
        {
            for (int i = 0; i < gunSprites.Length; i++)
            {
                gunSprites[i].SetActive(false);
            }

            gun = Random.Range(0, gunSprites.Length);

            gunSprites[gun].SetActive(true);
        }
    }
}
