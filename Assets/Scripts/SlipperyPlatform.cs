using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyPlatform : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float friction;

    private float playerBaseAcceleration, playerBaseDeceleration;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == Layers.Player) {
            other.gameObject.GetComponentInChildren<GroundCheck>().Grounded = true;
            playerBaseAcceleration = other.GetComponentInParent<PlayerMovementController>().Acceleration;
            playerBaseDeceleration = other.GetComponentInParent<PlayerMovementController>().Deceleration;
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == Layers.Player) {
            other.GetComponentInParent<PlayerMovementController>().Acceleration = playerBaseAcceleration / 2;
            other.GetComponentInParent<PlayerMovementController>().Deceleration = playerBaseDeceleration * friction;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == Layers.Player) {
            other.GetComponentInParent<PlayerMovementController>().Acceleration = playerBaseAcceleration;
            other.GetComponentInParent<PlayerMovementController>().Deceleration = playerBaseDeceleration;
        }
    }
}