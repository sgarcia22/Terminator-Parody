using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour {

    private GameObject player;
    private Rigidbody rb;
    private Animator anim;
    private GameObject rightBullet;
    private GameObject leftBullet;
    private float speed = 3.5f;
    LineRenderer rightLine;
    LineRenderer leftLine;
    private TextMeshProUGUI score;
    private AudioSource[] sounds;
    public static int health = 3;

    void Start () {
        player = this.gameObject;
        rb = player.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
        rightBullet = GameObject.FindGameObjectWithTag("RightBullet");
        leftBullet = GameObject.FindGameObjectWithTag("LeftBullet");
        rightLine = GameObject.FindGameObjectWithTag("Ray").GetComponent<LineRenderer>();
        rightLine.enabled = false;
        //Totally not a great idea but will do for now
        leftLine = GameObject.FindGameObjectWithTag("RayTwo").GetComponent<LineRenderer>();
        leftLine.enabled = false;
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        sounds = gameObject.GetComponents<AudioSource>();
    }
	
	void Update () {
        if (health == 0) //Game Over
        {
            //Show GameOver Scene
            SceneManager.LoadScene(2);
        }

        Vector3 position = player.transform.position;
        Vector3 rotation = player.transform.rotation.eulerAngles;
        bool running = false;
        //Change position
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow)) //Left
        { 
            position.x -= speed * Time.deltaTime;
            rotation.y = -180f;
            running = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow)) //Right
        {
            position.x += speed * Time.deltaTime;
            rotation.y = 0;
            running = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow)) //Up
        {
            position.z += speed * Time.deltaTime;
            rotation.y = -90f;
            running = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow)) //Down
        {
            position.z -= speed * Time.deltaTime;
            rotation.y = 90f;
            running = true;
        }
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow)) &&
            (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow))) //Down & Right
        {
            rotation.y = 45f;
        }
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow)) &&
            (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow))) //Down & Left
        {
            rotation.y = 135f;
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow)) &&
            (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow))) //Up & Left
        {
            rotation.y = -135f;
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow)) &&
            (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow))) //Up & Right
        {
            rotation.y = -45f;
        }
        
        //Change animation sequence
        if (running && anim.GetCurrentAnimatorStateInfo(0).nameHash != Animator.StringToHash("Base Layer.Running"))
        {
            anim.SetFloat("speed", 1);
        }
        else if (!running && anim.GetCurrentAnimatorStateInfo(0).nameHash != Animator.StringToHash("Base Layer.Idle"))
        {
            anim.SetFloat("speed", 0);
        }
        
        player.transform.position = position;
        player.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

        //Shooting Controls
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space)) && running)//Shooting Key
        {
            StopCoroutine("Fire");
            StartCoroutine("Fire");
        }
    }

    IEnumerator Fire ()
    {
        rightLine.enabled = true;
        leftLine.enabled = true;
        Vector3 rbPos = rightBullet.transform.position;
        Vector3 lbPos = leftBullet.transform.position;
        Vector3 rTargetPos = rightBullet.transform.forward;
        Vector3 lTargetPos = leftBullet.transform.forward;
        //playerPos.y = 0f;
        while (Input.GetKey(KeyCode.Space) && anim.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.Running"))
        {
            RaycastHit hitRight;
            RaycastHit hitLeft;

            //Right Ray
            Ray rightRay = new Ray(rbPos, rTargetPos);
            rightLine.SetPosition(0, rightRay.origin);

            GameObject hit = null;

            if (sounds[1].isPlaying == false)
            {
                sounds[1].pitch = (UnityEngine.Random.Range(0.4f, .9f));
                sounds[1].Play();
            }

            if (Physics.Raycast(rightRay, out hitRight, 100) && hitRight.transform.tag == "Enemy")
            {
                rightLine.SetPosition(1, hitRight.point);
                hitRight.transform.gameObject.GetComponent<ParticleSystem>().Play();
                Destroy(hitRight.transform.gameObject);
                Spawner.totalEnemies -= 1;
                UpdateScore();
                hit = hitRight.transform.gameObject;
            }
            else
                rightLine.SetPosition(1, rightRay.GetPoint(20));

            //Left Ray
            Ray leftRay = new Ray(lbPos, lTargetPos);
            leftLine.SetPosition(0, leftRay.origin);
            if (Physics.Raycast(leftRay, out hitLeft, 100) && hitLeft.transform.tag == "Enemy")
            {
                leftLine.SetPosition(1, hitLeft.point);
                hitLeft.transform.gameObject.GetComponent<ParticleSystem>().Play();
                Destroy(hitLeft.transform.gameObject);
                if (hit != hitLeft.transform.gameObject)
                    Spawner.totalEnemies -= 1;
                UpdateScore();
            }
            else
                leftLine.SetPosition(1, leftRay.GetPoint(20));


            yield return null;
        }
        rightLine.enabled = false;
        leftLine.enabled = false;
    }

    private void UpdateScore()
    {
        string currentScore = score.text;
        int currentScoreInt = Int32.Parse(currentScore);
        currentScoreInt++;
        currentScore = currentScoreInt.ToString();
        score.text = currentScore;
    }
}
