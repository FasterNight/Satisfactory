using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchSpeed = 3f;

    public Transform cameraTransform; 

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = false;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Animator mAnimator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            mAnimator.SetBool("IsGround", true);
        }

        // Inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Direction relative à la caméra
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = (right * x + forward * z).normalized;

        controller.Move(move * speed * Time.deltaTime);

        if (move.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }

        float movementMagnitude = new Vector3(x, 0, z).magnitude;
        mAnimator.SetFloat("Speed", movementMagnitude, 0.05f, Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            mAnimator.SetBool("IsGround", false);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Crouch()
    {
        controller.height = crouchHeight;
        isCrouching = true;
        speed = crouchSpeed;
    }

    void StandUp()
    {
        controller.height = standingHeight;
        isCrouching = false;
        speed = 6f;
    }
}
