using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementStat
{
    public float maxSpeed = 3f;
    public float acceleration = 10f;
    public float gravity = 9.81f;
    public float maxFallSpeed = 20f;

    [System.NonSerialized]
    public CollisionFlags collisionFlags;
    [System.NonSerialized]
    public Vector3 calculatedVelocity;
    [System.NonSerialized]
    public Vector3 lastPosition = Vector3.zero;
    [System.NonSerialized]
    public Vector3 hitPosition = Vector3.zero;
    [System.NonSerialized]
    public Vector3 lastHitPosition = Vector3.zero;

}

[System.Serializable]
public class JumpStat
{
    public bool enable = true;
    public float JumpHeight = 3f;
    public float durationToNextJump = 1.5f;

    [System.NonSerialized]
    public bool isJumping = false;
}

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour {

    public bool isControllable = true;

    public MovementStat movement;
    public JumpStat jumps;

    [System.NonSerialized]
    public bool inputJump = false;
    [System.NonSerialized]
    public Vector3 inputMoveDirection = Vector3.zero;

    CharacterController controller;
    Vector3 groundNormal = Vector3.zero;
    Vector3 lastGroundNormal = Vector3.zero;
    bool grounded = false;

    PlayerStat playerStat;

    void Start ()
    {
        controller = GetComponent<CharacterController>();
        playerStat = GetComponent<PlayerStat>();
    }
	
	void FixedUpdate ()
    {
        Movement();
    }

    void Movement()
    {
        // base input
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        inputMoveDirection = new Vector3(horizontal, 0, vertical);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (movement.maxSpeed < 6)
                movement.maxSpeed += Time.deltaTime * 10;
        }
        else
        {
            if (movement.maxSpeed > 3)
                movement.maxSpeed -= Time.deltaTime * 10;
            else
                movement.maxSpeed = 3;
        }

        inputJump = Input.GetKey(KeyCode.Space);

        if (GameplayManager.IsPauseInput || playerStat.isDead)
            return;
        //---------------------------------------------------------------

        //velocity calculation
        Vector3 velocity = movement.calculatedVelocity;

        velocity = VelocityCalculator(velocity);
        velocity = ApplyGravityToVelocity(velocity);
        velocity = ApplyJuempToVelocity(velocity);

        //-----------------------------------------------------------------

        movement.lastPosition = transform.position;
        groundNormal = Vector3.zero;

        // move character
        controller.Move(velocity * Time.deltaTime);
        //------------------------------------------------------------------

        movement.calculatedVelocity = (transform.position - movement.lastPosition) / Time.deltaTime;
        lastGroundNormal = groundNormal;

        //ready to jump
        if (grounded && !IsGroundTest())
        {
            grounded = false;

        }
        //ready to land
        else if (!grounded && IsGroundTest())
        {
            grounded = true;
            jumps.isJumping = false;
        }
    }

    Vector3 VelocityCalculator( Vector3 oriVelocity)
    {
        if (!isControllable)
            inputMoveDirection = Vector3.zero;

        Vector3 maxVelocity = transform.TransformDirection(inputMoveDirection) * movement.maxSpeed;
        //true acceleration
        Vector3 deltaVelocity = maxVelocity - movement.calculatedVelocity;
        //max acceleration
        float acc = movement.acceleration * (Time.deltaTime);

        //if (true acc > setting acc) -> true = setting
        if(deltaVelocity.sqrMagnitude > acc * acc)
        {
            deltaVelocity = deltaVelocity.normalized * acc;
        }
        //increase velocity
        if(grounded && isControllable)
        {
            oriVelocity += deltaVelocity;
        }


        return oriVelocity;
    }

    Vector3 ApplyGravityToVelocity(Vector3 originVelocity)
    {
        if (!grounded)
        {
            originVelocity.y = movement.calculatedVelocity.y - 9.81f * Time.deltaTime;
        }
        else
        {
            originVelocity.y = Mathf.Min(0, originVelocity.y) - 9.81f * Time.deltaTime;
        }
        return originVelocity;
    }

    Vector3 ApplyJuempToVelocity(Vector3 originVelocity)
    {
        if (inputJump && grounded && isControllable && jumps.enable)
        {
            grounded = false;
            jumps.isJumping = true;
            StartCoroutine(DurationForJumps(jumps.durationToNextJump));

            originVelocity.y = 0;
            originVelocity += Vector3.up * jumps.JumpHeight;
        }

        return originVelocity;
    }

    bool IsGroundTest()
    {
        return (groundNormal.y > 0.01);
    }

    IEnumerator DurationForJumps(float dur)
    {
        jumps.enable = false;
        yield return new WaitForSeconds(dur);
        jumps.enable = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0 && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0)
        {
            if (lastGroundNormal == Vector3.zero)
                groundNormal = hit.normal;
            else
                groundNormal = lastGroundNormal;
        }
    }
}
