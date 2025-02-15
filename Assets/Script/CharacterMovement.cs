using System;
using Unity.Mathematics;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float jumpForce = 10f;
    private CharacterController controller;
    private Animator animator;
   private float downwardVelocity;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        animator.SetBool("Run", false);
    }


    void Update()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float moveAmount = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        Vector3 velocity = new Vector3(horizontal, 0, vertical).normalized * movementSpeed ;
        velocity = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z)) * velocity;
        if (controller.isGrounded)
        {
            downwardVelocity = -2f;
            if (Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("Jump");
                downwardVelocity = jumpForce;
            }
        }
        else
        {
            downwardVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime ;
            if (Input.GetButtonDown("Jump") && downwardVelocity > 0f)
            {
                downwardVelocity *= 0.5f;
            }
        }
        velocity.y = downwardVelocity;
        controller.Move(velocity * Time.deltaTime);
        if(moveAmount > 0)
        {
            animator.SetBool("Run", true);
            var targetRotation = Quaternion.LookRotation(new Vector3(velocity.x, 0f, velocity.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Run", false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        void Attack()
        {
            animator.SetTrigger("Attack");
        }
    }
}
