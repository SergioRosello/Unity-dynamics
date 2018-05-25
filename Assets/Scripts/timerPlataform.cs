using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerPlataform : MonoBehaviour {

    private bool playerCollided;
    private bool hasToStartFalling;
    private bool isInOriginalPosition;
    private Vector3 originalPosition;
    public float timeTillFall;
    public float timeTillReset;
	// Use this for initialization
	void Start () {
        isInOriginalPosition = true;
        hasToStartFalling = false;
        playerCollided = false;
        originalPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (playerCollided && hasToStartFalling){
            transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y - .1f, transform.position.z), Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            playerCollided = true;
            other.gameObject.GetComponentInChildren<GroundCheck>().Grounded = true;
            StartCoroutine(Fall());
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player") {
            other.gameObject.GetComponentInChildren<GroundCheck>().Grounded = false;
            StartCoroutine(Reset());
        }
    }

    IEnumerator Fall() {
        yield return new WaitForSeconds(timeTillFall);
        hasToStartFalling = true;
        isInOriginalPosition = false;
    }

    IEnumerator Reset() {
        yield return new WaitForSeconds(timeTillReset);
        transform.position = originalPosition;
        playerCollided = false;
        hasToStartFalling = false;
    }
}
