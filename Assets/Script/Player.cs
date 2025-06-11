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
        mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        float movementMagnitude = new Vector3(x, 0, z).magnitude;
        mAnimator.SetFloat("Speed", movementMagnitude);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
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