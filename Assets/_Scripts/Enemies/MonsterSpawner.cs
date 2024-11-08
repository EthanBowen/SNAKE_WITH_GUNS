using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private _GameController _gameController;

    [SerializeField]
    private GameObject[] monstersCanSpawn;

    public float monstersPerSecond = 1; // How many monsters to spawn per second

    private float timeSinceLastMonster = 0;
    private int numMonstersToSpawn = 1;
    private int timesSpawnedMonsters = 0;

    private float secondsBetweenMonsters;


    public float verticalBoundry = 6;
    public float horizontalBoundry = 10;

    [HideInInspector] public List<Monster> monstersOnField;

    private double durationOfGame = 0.0f;

    private void Awake()
    {
        monstersOnField = new List<Monster>();
    }

    // Start is called before the first frame update
    void Start()
    {
        secondsBetweenMonsters = 1.0f / monstersPerSecond;

        if(_gameController == null)
        {
            _gameController = FindFirstObjectByType<_GameController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastMonster += Time.deltaTime;

        while(timeSinceLastMonster > secondsBetweenMonsters)
        {
            if (monstersOnField.Count < 600)
            {
                for (int i = 0; i < numMonstersToSpawn; i++)
                {
                    SpawnRandomMonster();
                }
                timesSpawnedMonsters++;

                if (timesSpawnedMonsters >= 10)
                {
                    numMonstersToSpawn += (numMonstersToSpawn / 10) + 1;
                    timesSpawnedMonsters -= 20;
                }
            }
            
            timeSinceLastMonster -= secondsBetweenMonsters;
        }

        durationOfGame += Time.deltaTime;
    }

    public void SpawnRandomMonster()
    {
        SpawnMonster(monstersCanSpawn[Random.Range(0, monstersCanSpawn.Length)]);
    }

    public void SpawnMonster(GameObject monster)
    {
        int side = Random.Range(0, 4);
        float xPos = 0;
        float yPos = 0;

        switch (side)
        {
            case 0:
                yPos = verticalBoundry;
                xPos = Random.Range(-horizontalBoundry, horizontalBoundry);
                break;
            case 1:
                yPos = -verticalBoundry;
                xPos = Random.Range(-horizontalBoundry, horizontalBoundry);
                break;
            case 2:
                xPos = horizontalBoundry;
                yPos = Random.Range(-verticalBoundry, verticalBoundry);
                break;
            case 3:
                xPos = -horizontalBoundry;
                yPos = Random.Range(-verticalBoundry, verticalBoundry);
                break;
        }

        SpawnMonster(monster, xPos, yPos);
    }

    public void SpawnMonster(GameObject monster, float xPos, float yPos)
    {
        Vector3 spawnPosition = new Vector3(xPos, yPos, Random.Range(0,1.0f));

        Monster monTemp = Instantiate(monster, spawnPosition, new Quaternion()).GetComponent<Monster>();
        monTemp.spawner = this;

        monstersOnField.Add(monTemp);
    }

    public void KillMonster(Monster i_toKill)
    {
        if (i_toKill != null)
        {
            monstersOnField.Remove(i_toKill);
            _gameController.ModifyScore(10);
            Destroy(i_toKill.gameObject);
        }
    }
}
