using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MovementController {
	public GameObject BombPrefab;
    protected float baseAcceleration;
    protected float baseDeceleration;
    [Range(0.0f, 1.0f)]
    public float AccelerationJumpReducer;
    [Range(0.0f, 1.0f)]
    public float DecelerationJumpReducer;


    protected override Vector2 GetDesiredMovement () {
		return new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")).normalized;
	}

    protected override void Awake()
    {
        base.Awake();
        baseAcceleration = Acceleration;
        baseDeceleration = Deceleration;
    }

    protected override void Update() {
		base.Update ();

        Acceleration = baseAcceleration;
        Deceleration = baseDeceleration;
        if (Input.GetKeyDown (KeyCode.Space) && _groundCheck.Grounded) {
			// Salto
			_rb.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
        }
        if (!_groundCheck.Grounded)
        {
            Debug.Log("Jumping...");
            // Cambio de aceleracion-deceleracion en el aire
            Acceleration *= AccelerationJumpReducer;
            Deceleration *= DecelerationJumpReducer;
        }


		if (Input.GetKeyDown (KeyCode.Q)) {
			Instantiate (BombPrefab, transform.position, Quaternion.identity);
		}
	}
}
