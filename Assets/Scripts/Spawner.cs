using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private GameObject [] spawns;
    public static int totalEnemies;
    public int maxEnemies = 100;
    public GameObject [] enemies;
    private float time;
    private int spawnTimer = 2;

	void Start () {
        spawns = GameObject.FindGameObjectsWithTag("Spawn");
        time = 0f;
        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
    }

	void Update () {
        if (PlayerMovement.health == 0)
            return;

        time += Time.deltaTime;
        if (totalEnemies <= maxEnemies && time >= spawnTimer)
        {
            SpawnEnemy();
            time = 0f;
        }
	}

    private void SpawnEnemy()
    {
        int enemy = Random.Range(0, 2);
        int spawner = Random.Range(0, 5);
        GameObject newEnemy = Instantiate(enemies[enemy], spawns[spawner].transform.position, enemies[enemy].transform.rotation);
        totalEnemies += 1;
        Vector3 pos = newEnemy.transform.position;
        if (enemy == 0)
            pos.y = 11;
        else
            pos.y = 10;
        newEnemy.transform.position = pos;
        newEnemy.transform.parent = gameObject.transform;
    }
}
