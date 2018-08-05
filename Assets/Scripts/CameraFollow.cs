using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform player;               
    private Vector3 offset;

    public float smoothing = 5f;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        offset = gameObject.transform.position - player.position;
    }
	
	void Update () {
        Vector3 targetCamPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
