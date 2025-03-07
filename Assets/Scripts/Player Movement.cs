using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Audio;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;

    public AudioMixer audioMixer;
    public float currentVolume = 1;

    public bool frozen = true;
    private bool isLatched = false;
    private bool isFalling = false;

    public float groundSpeed = 5f;
    private bool isGrounded = false;
    private bool isLookingRight = true;
    private bool isLookingRightClimbing = true;
    public float lookRightAngle = 230;
    public float lookLeftAngle = 50;
    public float climbingLookRightAngle = 230;
    public float climbingLookLeftAngle = 50;

    private bool isClimbing = false;
    private Vector3 climbPosition;

    public float maxHoldTime = 2f;
    public float maxJumpForce = 10f;
    private float holdTime = 0f;
    public float jumpAngle = 45f;
    private bool canJump = true;
    private bool isChargingJump = false;
    private bool lastJumpDirection;

    private float ZLock;

    public new Rigidbody rigidbody;

    public Transform groundCheck1;
    public Transform groundCheck2;
    public Transform groundCheck3;
    public Transform groundCheck4;

    public Transform climbCheck1;
    public Transform climbCheck2;

    public AudioSource latchAS;
    public AudioClip[] latchSounds;
    private int currentLatchSound = 0;

    public AudioSource jumpAS;
    public AudioClip[] jumpSounds;
    private int currentJumpSound = 0;

    public AudioSource fallingAirAS;
    public AudioClip fallingAirSound;
    public float minAirSoundSpeed;
    public float maxAirSoundSpeed;

    public float windAffectingPlayerWhileClimbing = 50;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ZLock = transform.position.z;
        fallingAirAS.clip = fallingAirSound;
        fallingAirAS.volume = 0;
        fallingAirAS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentVolume += 0.1f;
            currentVolume = Mathf.Clamp(currentVolume, 0.0001f, 1f); // Avoids log(0) issue
            float dB = Mathf.Log10(currentVolume) * 30;
            audioMixer.SetFloat("MasterVolume", dB);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            currentVolume -= 0.1f;
            currentVolume = Mathf.Clamp(currentVolume, 0.0001f, 1f); // Avoids log(0) issue
            float dB = Mathf.Log10(currentVolume) * 30;
            audioMixer.SetFloat("MasterVolume", dB);
        }


        if (frozen)
        {
            //rigidbody.velocity = new Vector3(0, 0, 0);
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            return;
        }
        animator.SetBool("isWalking", false);
        float speed = Vector3.Magnitude(rigidbody.velocity);

        float volumeLevel;

        if (speed <= minAirSoundSpeed)
        {
            volumeLevel = 0.0f;
        }
        else if (speed >= maxAirSoundSpeed)
        {
            volumeLevel = 1.0f;
        }
        else
        {
            // Linear interpolation between 0 and 1 based on the range [minAirSoundSpeed, maxAirSoundSpeed]
            volumeLevel = (speed - minAirSoundSpeed) / (maxAirSoundSpeed - minAirSoundSpeed);
        }

        fallingAirAS.volume = volumeLevel;

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


                latchAS.clip = latchSounds[currentLatchSound];

                latchAS.time = 0.025f;

                latchAS.Play();

                currentLatchSound++;
                currentLatchSound = currentLatchSound % latchSounds.Length;

                isLatched = true;
                animator.SetBool("uisClimbing", isLatched);
            }
        }

        if (!isClimbing)
        {
            isLatched = false;
            animator.SetBool("uisClimbing", isLatched);
        }



        transform.position = new Vector3(transform.position.x, transform.position.y, ZLock);

        // Shoot ray down to check if player is grounded,
        // if ray hits gameobject with tag "Ground" then player is grounded.
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
            isFalling = false;
            animator.SetBool("uisFalling", isFalling);
        }
        else
        {
            isGrounded = false;
            canJump = false;
            isFalling = true;
            animator.SetBool("uisFalling", isFalling);
        }

        if (isClimbing)
        {
            transform.position = climbPosition;
            rigidbody.velocity = new Vector3(0, 0, 0);
            canJump = true;
            isFalling = false;
            animator.SetBool("uisFalling", isFalling);
        }

        if (isGrounded)
        {
            rigidbody.velocity = new Vector3(0, 0, 0);
            isFalling = false;
            animator.SetBool("uisFalling", isFalling);
        }

        if (isGrounded && !isChargingJump)
        {
            GroundMovement();
        }
        else if (isClimbing && !isChargingJump)
        {
            if (Input.GetKey(KeyCode.A))
            {
                isLookingRightClimbing = false;
            }

            if (Input.GetKey(KeyCode.D))
            {
                isLookingRightClimbing = true;
            }
        }

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    isClimbing = false;
        //    canJump = false;
        //    holdTime = 0f;
        //}


        if (isClimbing)
        {
            if (isLookingRightClimbing)
            {
                transform.rotation = Quaternion.Euler(0, climbingLookRightAngle, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, climbingLookLeftAngle, 0);
            }
        }
        else
        {
            if (isLookingRight)
            {
                transform.rotation = Quaternion.Euler(0, lookRightAngle, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, lookLeftAngle, 0);
            }
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

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
        {
            if (holdTime != 0)
            {
                jumpAS.clip = jumpSounds[currentJumpSound];
                jumpAS.time = 0.025f;
                jumpAS.Play();
                currentJumpSound++;
                currentJumpSound = currentJumpSound % jumpSounds.Length;
            }

            

            Vector3 angle;

            lastJumpDirection = isClimbing ? isLookingRightClimbing : isLookingRight;

            if (isClimbing)
            {
                isLookingRight = isLookingRightClimbing;
            }

            if (lastJumpDirection)
            {
                angle = new Vector3(Mathf.Cos(jumpAngle * Mathf.Deg2Rad), Mathf.Sin(jumpAngle * Mathf.Deg2Rad), 0).normalized;
            }
            else
            {
                angle = new Vector3(Mathf.Cos(((90 - jumpAngle) + 90) * Mathf.Deg2Rad), Mathf.Sin(((90 - jumpAngle) + 90) * Mathf.Deg2Rad), 0).normalized;
            }

            float r = 1.5f;
            float jumpForce = maxJumpForce * Mathf.Pow((holdTime / maxHoldTime), 1.0f / r);
            rigidbody.AddForce(angle * jumpForce, ForceMode.Impulse);
            holdTime = 0f;
            isClimbing = false;

            isChargingJump = false;
        }
    }

    void GroundMovement()
    {

        bool isMoving = false;

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-groundSpeed * Time.deltaTime, 0, 0);
            isLookingRight = false;
            isMoving = true;

        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(groundSpeed * Time.deltaTime, 0, 0);
            isLookingRight = true;
            animator.SetBool("isWalking", true);
            isMoving = true;
        }

        animator.SetBool("isWalking", isMoving);
    }

    public void LaunchPlayer(Transform otherTransform, float launchForce)
    {
        Vector3 launchAngle = (transform.position - otherTransform.position).normalized;
        rigidbody.AddForce(launchAngle * launchForce, ForceMode.Impulse);
    }

    public void LaunchPlayer(Vector3 direction, float launchForce)
    {
        rigidbody.AddForce(direction.normalized * launchForce, ForceMode.Impulse);
    }

    public void WindAffectingPlayer(Vector3 windDirection, float windForce)
    {
        rigidbody.AddForce(windDirection.normalized * windForce, ForceMode.Impulse);

        if (isClimbing)
        {
            climbPosition.x = transform.position.x + windDirection.normalized.x * windForce * windAffectingPlayerWhileClimbing * Time.deltaTime;

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

            if (climb1 && climb1Hit.collider.tag == "Climbable" || climb2 && climb2Hit.collider.tag == "Climbable")
            {
                isClimbing = true;
            }
            else
            {
                isClimbing = false;
            }
        }
    }

    public void StartPressed()
    {
        frozen = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Rope")
        {
            frozen = true;

            transform.position = new Vector3(collision.transform.position.x, transform.position.y, transform.position.z);
            Destroy(rigidbody);
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
