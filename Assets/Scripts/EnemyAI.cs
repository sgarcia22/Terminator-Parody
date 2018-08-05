using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    private GameObject enemy;
    private GameObject player;
    private Vector3 enemyLocation;
    private Vector3 playerLocation;
    ParticleSystem playerHit;
    public float speed = 2f;
    private bool attacking = false;

    void Start () {
        enemy = gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        playerHit = GameObject.FindGameObjectWithTag("PlayerHit").GetComponent<ParticleSystem>();
	}
	
	void Update () {
        if (PlayerMovement.health == 0)
            return;

        enemyLocation = enemy.transform.position;
        playerLocation = player.transform.position;
        if (Vector3.Distance(enemyLocation, playerLocation) >= 1f) //Follow the player
        {
            float xDistance = enemyLocation.x - playerLocation.x;
            float zDistance = enemyLocation.z - playerLocation.z;
            if (Mathf.Abs(xDistance) > Mathf.Abs(zDistance))
            {
                if (xDistance < 0)
                    enemyLocation.x += speed * Time.deltaTime;
                else
                    enemyLocation.x -= speed * Time.deltaTime;
            }
            else
            {
                if (zDistance < 0)
                    enemyLocation.z += speed * Time.deltaTime;
                else
                    enemyLocation.z -= speed * Time.deltaTime;
            }
            enemy.transform.position = enemyLocation;
        }
        else //Attack the player
        {
            if (attacking == false)
            {
                StartCoroutine("Attack");
                StartCoroutine("PlayExplosion");
            }
        }
	}

    IEnumerator Attack()
    {
        attacking = true;
        yield return new WaitForSeconds(1); //Wait for a bit before attacking
        Debug.Log("Player Hit");
        player.GetComponent<AudioSource>().Play(0);
        playerHit.Play(); //Play the animation explosion
        PlayerMovement.health -= 1; //Decrease the players health
        if (PlayerMovement.health == 2)
            GameObject.FindGameObjectWithTag("HealthOne").SetActive(false);
        else if (PlayerMovement.health == 1)
            GameObject.FindGameObjectWithTag("HealthTwo").SetActive(false);
        else
            GameObject.FindGameObjectWithTag("HealthThree").SetActive(false);
        attacking = false;
    }

    IEnumerator PlayExplosion()
    {
        yield return new WaitForSeconds(2);
        playerHit.Stop();
    }
}
