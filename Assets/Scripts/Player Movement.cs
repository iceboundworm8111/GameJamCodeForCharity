using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*
    
    Overview of script!!!
    
    Player can move left and right if they are on the ground.
    Player can only start charging up their jump if they are on the ground.
    Player can't move if they are charging up their jump.
    Player can hold space to charge up jump.
    maxHoldTime is how long until force doesn't get added to the final jump.
    maxJumpForce is the maximum force that can be added to the jump (will be reached at maxHoldTime).
    lookRight/LeftAngle sets the y rotation of the player when they move that direction. Also stays like that until you move a different direction.
    jumpAngle is the angle of the jump force that will be added to the player. 90 is straight up, 0 is straight forward. Should stay between 0 and 90. I convert it to both sides.
    Most other variables are public for testing purposes.

    */

    public float groundSpeed = 5f;
    public bool isGrounded = false;
    public bool isLookingRight = true;
    public float lookRightAngle = 230;
    public float lookLeftAngle = 130;

    public bool isClimbing = false;
    public Vector3 climbPosition;

    public float maxHoldTime = 2f;
    public float maxJumpForce = 10f;
    public float holdTime = 0f;
    public float jumpAngle = 45f;
    public bool canJump = true;
    public bool isChargingJump = false;

    public Rigidbody rigidbody;

    public Transform groundCheck1;
    public Transform groundCheck2;
    public Transform groundCheck3;
    public Transform groundCheck4;

    public Transform climbCheck1;
    public Transform climbCheck2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Shoot ray down to check if player is grounded,
        // if ray hits gameobject with tag "Ground" then player is grounded.
        // Maximum distance of ray is 1.1f (currently).
        float rayDistance = 0.22f;
        Vector3 rayDirection = Vector3.down;

        RaycastHit ground1Hit;
        Vector3 ground1RayOrigin = groundCheck1.transform.position;
        bool grounded1 = Physics.Raycast(ground1RayOrigin, rayDirection, out ground1Hit, rayDistance);
        Debug.DrawRay(ground1RayOrigin, rayDirection * rayDistance, Color.red);


        RaycastHit ground2Hit;
        Vector3 gorund2RayOrigin = groundCheck2.transform.position;
        bool grounded2 = Physics.Raycast(gorund2RayOrigin, rayDirection, out ground2Hit, rayDistance);
        Debug.DrawRay(gorund2RayOrigin, rayDirection * rayDistance, Color.red);

        RaycastHit ground3Hit;
        Vector3 ground3RayOrigin = groundCheck3.transform.position;
        bool grounded3 = Physics.Raycast(ground3RayOrigin, rayDirection, out ground3Hit, rayDistance);
        Debug.DrawRay(ground3RayOrigin, rayDirection * rayDistance, Color.red);

        RaycastHit ground4Hit;
        Vector3 ground4RayOrigin = groundCheck4.transform.position;
        bool grounded4 = Physics.Raycast(ground4RayOrigin, rayDirection, out ground4Hit, rayDistance);
        Debug.DrawRay(ground4RayOrigin, rayDirection * rayDistance, Color.red);

        if (grounded1 && ground1Hit.collider.tag == "Ground" || grounded2 && ground2Hit.collider.tag == "Ground" || grounded3 && ground3Hit.collider.tag == "Ground" || grounded4 && ground4Hit.collider.tag == "Ground")
        {
            isGrounded = true;
            canJump = true;
        }
        else
        {
            isGrounded = false;
            canJump = false;
        }

        if (isClimbing)
        {
            transform.position = climbPosition;
            rigidbody.velocity = new Vector3(0, 0, 0);
            canJump = true;
        }

        if (isGrounded)
        {
            rigidbody.velocity = new Vector3(0, 0, 0);
        }

        if (isGrounded && !isChargingJump)
        {
            GroundMovement();
        }
        else if (isClimbing && !isChargingJump)
        {
            if (Input.GetKey(KeyCode.A))
            {
                isLookingRight = false;
            }

            if (Input.GetKey(KeyCode.D))
            {
                isLookingRight = true;
            }
        }

            float climbRayDistance = 2f;
        Vector3 climbRayDirection = new Vector3(0, 0, 1);

        RaycastHit climb1Hit;
        Vector3 climb1RayOrigin = climbCheck1.transform.position;
        bool climb1 = Physics.Raycast(climb1RayOrigin, climbRayDirection, out climb1Hit, climbRayDistance);
        Debug.DrawRay(climb1RayOrigin, climbRayDirection * climbRayDistance, Color.red);

        RaycastHit climb2Hit;
        Vector3 climb2RayOrigin = climbCheck2.transform.position;
        bool climb2 = Physics.Raycast(climb2RayOrigin, climbRayDirection, out climb2Hit, climbRayDistance);
        Debug.DrawRay(climb2RayOrigin, climbRayDirection * climbRayDistance, Color.red);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (climb1 && climb1Hit.collider.tag == "Climbable" || climb2 && climb2Hit.collider.tag == "Climbable")
            {
                isClimbing = true;
                climbPosition = transform.position;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isClimbing = false;
        }

        if (isLookingRight)
        {
            transform.rotation = Quaternion.Euler(0, lookRightAngle, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, lookLeftAngle, 0);
        }

        if (canJump && Input.GetKey(KeyCode.W))
        {
            holdTime += Time.deltaTime;
            if (holdTime > maxHoldTime)
            {
                holdTime = maxHoldTime;
            }

            isChargingJump = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            float r = 1.5f;
            if (isLookingRight)
            {
                Vector3 angle = new Vector3(Mathf.Cos(jumpAngle * Mathf.Deg2Rad), Mathf.Sin(jumpAngle * Mathf.Deg2Rad), 0).normalized;
                float jumpForce = maxJumpForce * Mathf.Pow((holdTime / maxHoldTime), 1.0f / r);
                rigidbody.AddForce(angle * jumpForce, ForceMode.Impulse);
                holdTime = 0f;
                isClimbing = false;
            }
            else
            {
                Vector3 angle = new Vector3(Mathf.Cos(((90 - jumpAngle) + 90) * Mathf.Deg2Rad), Mathf.Sin(((90 - jumpAngle) + 90) * Mathf.Deg2Rad), 0).normalized;
                float jumpForce = maxJumpForce * Mathf.Pow((holdTime / maxHoldTime), 1.0f / r);
                rigidbody.AddForce(angle * jumpForce, ForceMode.Impulse);
                holdTime = 0f;
                isClimbing = false;
            }

            isChargingJump = false;
        }
    }

    void GroundMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-groundSpeed * Time.deltaTime, 0, 0);
            isLookingRight = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(groundSpeed * Time.deltaTime, 0, 0);
            isLookingRight = true;
        }
    }

    public void LaunchPlayer(Transform otherTransform, float launchForce)
    {
        Vector3 launchAngle = (transform.position - otherTransform.position).normalized;
        rigidbody.AddForce(launchAngle * launchForce, ForceMode.Impulse);
    }
}
