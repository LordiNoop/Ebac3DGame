using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStates playerStates;
    
    public Animator animator;

    public CharacterController characterController;
    public float speed = 1f; 
    public float turnSpeed = 1f; 
    public float gravity = 9.8f; 
    private float vSpeed = 0f;
    public float jumpSpeed = 15f;

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    [Header("Flash")]
    public List<FlashColor> flashColors;

    private void Update() 
    { 
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0); 

        var inputAxisVertical = Input.GetAxis("Vertical"); 
        var speedVector = transform.forward * inputAxisVertical * speed;

        if (characterController.isGrounded)
        {
            vSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vSpeed = jumpSpeed;
                playerStates.stateMachine.SwitchState(PlayerStates.PlayerMovementStates.JUMP);
            }
        }

        vSpeed -= gravity * Time.deltaTime; 
        speedVector.y = vSpeed;

        var isWalking = inputAxisVertical != 0;
        if (isWalking)
        {
            if (Input.GetKey(keyRun))
            {
                speedVector *= speedRun;
                animator.speed = speedRun;
            }
            else
            {
                animator.speed = 1;
            }
        }

        characterController.Move(speedVector * Time.deltaTime);

        if (characterController.velocity.x != 0 && characterController.isGrounded || characterController.velocity.z != 0 && characterController.isGrounded)
        {
            playerStates.stateMachine.SwitchState(PlayerStates.PlayerMovementStates.RUN);
        }
        else if (characterController.velocity.x == 0 && characterController.isGrounded || characterController.velocity.z == 0 && characterController.isGrounded)
        {
            playerStates.stateMachine.SwitchState(PlayerStates.PlayerMovementStates.IDLE);
        }

        if (inputAxisVertical != 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    #region LIFE

    public void Damage(float damage)
    {
        flashColors.ForEach(i => i.Flash());
    }

    public void Damage(float damage, Vector3 dir)
    {
        Damage(damage);
    }

    #endregion
}
