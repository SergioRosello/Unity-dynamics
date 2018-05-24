using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MovementController {
	public GameObject BombPrefab;
    protected float baseAcceleration;
    protected float baseDeceleration;
    protected float shootPressTime = 0;
    protected Vector3 movementForward;
    [Range(0.0f, 1.0f)]
    public float AccelerationJumpReducer;
    [Range(0.0f, 1.0f)]
    public float DecelerationJumpReducer;
    public float maxCannonBallForce;
    private float tmpMaxCannonBallForce;
    public float MinShootPercent = .4f;


    protected override Vector2 GetDesiredMovement () {
		return new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")).normalized;
	}

    protected override Vector2 GetDesiredVelocity() {
        directionForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, transform.up).normalized;
        directionRight = Vector3.ProjectOnPlane(Camera.main.transform.right, transform.up).normalized;

        Vector3 movement = _input.y * directionForward * Acceleration + _input.x * directionRight * Acceleration;
        _rb.AddForce(movement, ForceMode.VelocityChange);
        Quaternion direction = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, 100 * Time.deltaTime);
        return new Vector2(_rb.velocity.x, _rb.velocity.z);
    }

    protected override void Awake()
    {
        base.Awake();
        tmpMaxCannonBallForce = maxCannonBallForce;
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
        if (!_groundCheck.Grounded) {
            Debug.Log("Jumping...");
            // Cambio de aceleracion-deceleracion en el aire
            Acceleration *= AccelerationJumpReducer;
            Deceleration *= DecelerationJumpReducer;
        }

        //Shoot cannon ball based on input time pressed
        if (Input.GetKey(KeyCode.Q)) {
            shootPressTime += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.Q)) {
            // Calcular cuánto tengo que saltar en función a ese tiempo
            var percentStrength = Mathf.Clamp(shootPressTime, MinShootPercent, 1);
            //Debug.Log("percentStrength: " + percentStrength);
            Debug.Log("MaxCannonBallStrength before: " + maxCannonBallForce);
            maxCannonBallForce *= percentStrength;
            Debug.Log("MaxCannonBallStrength after: " + maxCannonBallForce);
            ShootCannonBall();
        }
    }

    protected void ShootCannonBall() {
        var go = Instantiate(BombPrefab, transform.position, Quaternion.identity);
        // Disparar la bomba hacia donde estoy mirando
        go.GetComponent<Rigidbody>().AddForce(transform.forward * maxCannonBallForce, ForceMode.VelocityChange);
        shootPressTime = 0;
        maxCannonBallForce = tmpMaxCannonBallForce;
    }

    protected void LateUpdate() {
        _anim.SetFloat("MoveSpeed", _input.magnitude);
    }
}
