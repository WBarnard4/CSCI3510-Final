using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float maxSpawnDist = 15f;
    public float minSpawnDist = 10f;
    public int maxSpawnCount = 5;
    public float spawnFrequency = 10f;
    public float interSpawnDelay = 0.5f;


    private float timer = 0f;
    private List<GameObject> activeEnemies = new List<GameObject>();

    //Load our enemy prefabs, could easily be changed to lists to store multiple different enemies of the same type
    public GameObject WeakEnemy;
    public GameObject BasicEnemy;
    public GameObject HeavyEnemy;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnFrequency)
        {
            spawnWave();
            Debug.Log("Wave Spawned");
            timer = 0f;
        }
    }

    void spawnWave()
    {
        //remove dead enemies from our active enemy array to limit spawns properly
        cleanUpDeadEnemies();
        PlayerScript p = player.GetComponent<PlayerScript>();

        //determine the enemy amount to spawn
        int enemyCount = Math.Clamp(p.playerLevel*2, 5, maxSpawnCount);
        //int enemyCount = 50;
        Transform PlayerPos = PlayerManager.Instance.player.transform;

        //figure the amount we can spawn given the current amount of enemies active 
        int amtToSpawn = Math.Min(enemyCount, maxSpawnCount-activeEnemies.Count);

        if(amtToSpawn <= 0)
        {
            //if we are full on enemies
            Debug.Log("Couldnt spawn enemies, max enemy count reached.");
            Debug.Log("Enemy Count = " + activeEnemies.Count);
            return;
        }

        StartCoroutine(SpawnWaveCoroutine(amtToSpawn, p.playerLevel, PlayerPos.position));

    }

    private IEnumerator SpawnWaveCoroutine(int count, int playerLevel, Vector3 playerPos)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition(playerPos);
            spawnEnemy(spawnPosition, playerLevel);

            if (i < count - 1)
            {
                yield return new WaitForSeconds(interSpawnDelay);
            }
        }
    }


    Vector3 GetSpawnPosition(Vector3 playerPos)
    {
        //This function returns the spawn positions for each enemy being spawned
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized; //Direction to spawn
        float spawnDist = UnityEngine.Random.Range(minSpawnDist,maxSpawnDist);

        //given the player position, our spawn position will be a vector a random direction in the x and z axis 
        //with a distance multiplied by spawn distance
        Vector3 spawnPos = playerPos + new Vector3(randomDirection.x, 0, randomDirection.y) * spawnDist;


        return spawnPos;
    }

    void spawnEnemy(Vector3 spawnPosition, int playerLevel)
    {
        GameObject toSpawn = null;
        float spinTheWheel = UnityEngine.Random.value;
        if(playerLevel < 3)
        {
            if(spinTheWheel < 0.75f)
            {
                toSpawn = WeakEnemy;
            }
            else if(spinTheWheel < 0.9f)
            {
                toSpawn = BasicEnemy;
            }
            else
            {
                toSpawn = HeavyEnemy;
            }
            
        }
        else if(playerLevel < 6)
        {
            if(spinTheWheel < 0.5f)
            {
                toSpawn = WeakEnemy;
            }
            else if(spinTheWheel < 0.85f)
            {
                toSpawn = BasicEnemy;
            }
            else
            {
                toSpawn = HeavyEnemy;
            }
        }
        else if(playerLevel < 9)
        {
            if(spinTheWheel < 0.35f)
            {
                toSpawn = WeakEnemy;
            }
            else if(spinTheWheel < 0.75f)
            {
                toSpawn = BasicEnemy;
            }
            else
            {
                toSpawn = HeavyEnemy;
            }
        }
        else if(playerLevel < 12)
        {
            if(spinTheWheel < 0.20f)
            {
                toSpawn = WeakEnemy;
            }
            else if(spinTheWheel < 0.70f)
            {
                toSpawn = BasicEnemy;
            }
            else
            {
                toSpawn = HeavyEnemy;
            }
        }
        else if(playerLevel < 16)
        {
            if(spinTheWheel < 0.1f)
            {
                toSpawn = WeakEnemy;
            }
            else if(spinTheWheel < 0.65f)
            {
                toSpawn = BasicEnemy;
            }
            else
            {
                toSpawn = HeavyEnemy;
            }
        }
        spawnPosition.y = -3.0f;
        GameObject spawnedEnemy = Instantiate(toSpawn, spawnPosition, Quaternion.identity);
        activeEnemies.Add(spawnedEnemy);
    }

    void cleanUpDeadEnemies()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
    }
}
