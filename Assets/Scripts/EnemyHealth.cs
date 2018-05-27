using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float ForceToImpulsePlayerWithWhenKillsMe;

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.layer == Layers.PlayerGroundCheck){
            other.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * ForceToImpulsePlayerWithWhenKillsMe, ForceMode.VelocityChange);
            Destroy(GetComponentInParent<EnemyMovementController>().gameObject);
        }
    }
}
