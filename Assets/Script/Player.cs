using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float crouchHeight = 10f;
    public float standingHeight = 2f;
    public float crouchSpeed = 3f;
    public float sprintSpeed = 10f;

    public Transform cameraTransform; 

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = false;
    private bool isAttacking = false;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float attackRange = 1.5f;
    public float attackDamage = 10f;
    public Transform attackPoint; 
    public LayerMask enemyLayers;

    private Animator mAnimator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mAnimator = GetComponentInChildren<Animator>();
        mAnimator.applyRootMotion = false;
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

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = (right * x + forward * z).normalized;

        // Attack
        if (Input.GetMouseButtonDown(0) && isGrounded && !isCrouching)
        {
            StartCoroutine(AttackRoutine());


            mAnimator.SetTrigger("Attack");
            isAttacking = true;
            Invoke("EndAttack", 0.6f);
        }

        // Sprint
        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            mAnimator.SetBool("Sprint", true);
            currentSpeed = sprintSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            mAnimator.SetBool("Sprint", false);
        }

        if (!isAttacking)
        {
            controller.Move(move * currentSpeed * Time.deltaTime);
        }


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
            mAnimator.SetBool("IsCrouch", true);
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
        controller.center = new Vector3(0, crouchHeight / 2f, 0);
        isCrouching = true;
        speed = crouchSpeed;
    }

    void StandUp()
    {
        mAnimator.SetBool("IsCrouch", false);
        controller.height = standingHeight;
        isCrouching = false;
        speed = 6f;
    }

    // Attack
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        mAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.2f); // timing avant que le coup touche

        // Simulation du coup (raycast ou overlap)
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Touché : " + enemy.name);
            // enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage); // à adapter
        }

        yield return new WaitForSeconds(0.5f); // temps de blocage total
        isAttacking = false;
    }

    void EndAttack()
    {
        Debug.Log("Fin de l'attaque");
        isAttacking = false;
    }

}
