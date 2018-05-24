using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerPlataform : MonoBehaviour {

    private bool playerCollided;
	// Use this for initialization
	void Start () {
        playerCollided = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (playerCollided){
            transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y - .1f, transform.position.z), Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            playerCollided = true;
            other.gameObject.GetComponentInChildren<GroundCheck>().Grounded = true;
        }
    }
}
