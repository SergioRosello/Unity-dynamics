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
    public int maxNumberOfFollowedJumps;
    private int numberOfFollowedJumps;
    public float maxJumpTimer;
    private float jumpTimer;


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
        jumpTimer = maxJumpTimer;
        numberOfFollowedJumps = 0;
    }

    protected override void Update() {
		base.Update ();

        Acceleration = baseAcceleration;
        Deceleration = baseDeceleration;

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && _groundCheck.Grounded) {
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
            numberOfFollowedJumps = 1;
        //Cuando ya he tocado el suelo y ha presionado el boton de salto hace menos de maxJumpTimer tiempo
        } else if (_groundCheck.Grounded && jumpTimer <= maxJumpTimer) {
            var jumpMultiplier = 1 + numberOfFollowedJumps * .2f;
            Debug.Log("Jump multiplier: " + jumpMultiplier);
            _rb.AddForce(Vector3.up * JumpForce * jumpMultiplier, ForceMode.VelocityChange);
            // Vamos acumulando el numero de veces que hemos saltado
            // Hasta llegar a maxNumberOfFollowedJumps
            if (numberOfFollowedJumps >= maxNumberOfFollowedJumps) numberOfFollowedJumps = 1;
            else numberOfFollowedJumps++;
            // Volvemos a asignar a jumpTimer el maximo, para que
            // para volver a entrar en el salto larago tenga que
            // presionar la tecla de salto en el aire
            jumpTimer = maxJumpTimer;
        // Si presiona la tecla de salto antes de que llegue al suelo
        } else if (Input.GetKeyDown(KeyCode.Space) && !_groundCheck.Grounded) {
            // Empezamos a contar
            jumpTimer = 0;
        }
        jumpTimer += Time.deltaTime;

        // Cuando esta en el aire
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
            var percentStrength = Mathf.Clamp(shootPressTime, MinShootPercent, 1);
            maxCannonBallForce *= percentStrength;
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

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.SlipperyPlatform) {
            
        }
    }
}
