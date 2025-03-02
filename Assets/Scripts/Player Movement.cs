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

    public float maxHoldTime = 2f;
    public float maxJumpForce = 10f;
    public float holdTime = 0f;
    public float jumpAngle = 45f;
    public bool canJump = true;
    public bool isChargingJump = false;

    public Rigidbody rigidBody;

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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            if (hit.collider.tag == "Ground")
            {
                isGrounded = true;
                canJump = true;
            }
        }
        else
        {
            isGrounded = false;
            canJump = false;
        }

        if (isGrounded)
        {
            rigidBody.velocity = new Vector3(0, 0, 0);
        }

        if (isGrounded && !isChargingJump)
        {
            GroundMovement();
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
            if (isLookingRight)
            {
                Vector3 angle = new Vector3(Mathf.Cos(jumpAngle * Mathf.Deg2Rad), Mathf.Sin(jumpAngle * Mathf.Deg2Rad), 0).normalized;
                float jumpForce = (holdTime / maxHoldTime) * maxJumpForce;
                rigidBody.AddForce(angle * jumpForce, ForceMode.Impulse);
                holdTime = 0f;
            }
            else
            {
                Vector3 angle = new Vector3(Mathf.Cos(((90 - jumpAngle) + 90) * Mathf.Deg2Rad), Mathf.Sin(((90 - jumpAngle) + 90) * Mathf.Deg2Rad), 0).normalized;
                float jumpForce = (holdTime / maxHoldTime) * maxJumpForce;
                rigidBody.AddForce(angle * jumpForce, ForceMode.Impulse);
                holdTime = 0f;
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
}
