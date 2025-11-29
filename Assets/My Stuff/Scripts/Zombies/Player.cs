using System;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour
{

    public float health = 100;
    public float baseSpeed;
    public float runningAmplifier;

    public bool lockCursor;

    private CharacterController characterController;
    private Animator animator;

    private float airTime;
    private float gravity = -9.81f;

    private PlayerMovementInfo playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        playerMovement = new PlayerMovementInfo();
        playerMovement.baseSpeed = baseSpeed;
        playerMovement.runningAmplifier = runningAmplifier;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        airTime = 0;
        
    }

    void Update()
    {
        ProcessInput();

        PerformBlendTreeAnimation();

        CalculateDirectionAndDistance();

        PerformPhysicalMovement();

        RotatePlayerWithCamera();
    }

    public void ProcessInput()
    {
        playerMovement.leftAndRight = Input.GetAxis("Horizontal");
        playerMovement.forwardAndBack = Input.GetAxis("Vertical");

        playerMovement.movingForwards = playerMovement.forwardAndBack > 0.0f;
        playerMovement.movingBackwards = playerMovement.forwardAndBack < 0.0f;

        bool running = (playerMovement.movingForwards && Input.GetKey(KeyCode.LeftShift))
                    || (!playerMovement.movingBackwards && (playerMovement.leftAndRight > 0.0f || playerMovement.leftAndRight < 0.0f));

        if (running)
        {
            playerMovement.speed = playerMovement.baseSpeed * playerMovement.runningAmplifier;
        }
        else
        {
            playerMovement.speed = playerMovement.baseSpeed;

            playerMovement.forwardAndBack = playerMovement.forwardAndBack / 2.0f;
        }

    }

    public void PerformBlendTreeAnimation()
    {
  
        float leftAndRight = playerMovement.leftAndRight;

        if (playerMovement.movingBackwards)
        {
            leftAndRight = 0;
        }

        animator.SetFloat("LeftAndRight", leftAndRight);
        animator.SetFloat("ForwardAndBack", playerMovement.forwardAndBack);
    }


    public void CalculateDirectionAndDistance()
    {
        Vector3 moveDirectionForward = transform.forward * playerMovement.forwardAndBack;
        Vector3 moveDirectionSide = transform.right * playerMovement.leftAndRight;

        playerMovement.direction = moveDirectionForward + moveDirectionSide;

        playerMovement.normalizedDirection = playerMovement.direction.normalized;

        playerMovement.distance = playerMovement.normalizedDirection * playerMovement.speed * Time.deltaTime;

        GroundPlayer();
    }

    public void PerformPhysicalMovement()
    {
        characterController.Move(playerMovement.distance);
    }

    public void GroundPlayer()
    {
        if (characterController.isGrounded)
        {
            airTime = 0;
        }
        else // jumping / falling 
        {
            airTime += Time.deltaTime;
            Vector3 direction = playerMovement.normalizedDirection;

            direction.y += 0.5f * gravity * airTime;

            playerMovement.normalizedDirection = direction;
            playerMovement.distance = playerMovement.normalizedDirection * airTime;
        }
    }
    
    public void RotatePlayerWithCamera()
    {
        Vector3 rotation;

        if (Input.GetKey(KeyCode.T))
        {
            return;
        }
        else
        {
            rotation = Camera.main.transform.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;

            transform.eulerAngles = rotation;
        }
    }

    public void takeDamage(float damage)
    {
        //force damage upon the player
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            //kill the player and end the game.
            Debug.Log("Player is Dead!");
        }
    }
}
