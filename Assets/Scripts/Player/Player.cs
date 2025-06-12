using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStates playerStates;

    public CharacterController characterController;
    public float speed = 1f; 
    public float turnSpeed = 1f; 
    public float gravity = 9.8f; 
    private float vSpeed = 0f;
    public float jumpSpeed = 15f;

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

        characterController.Move(speedVector * Time.deltaTime);

        if (characterController.velocity.x != 0 && characterController.isGrounded || characterController.velocity.z != 0 && characterController.isGrounded)
        {
            playerStates.stateMachine.SwitchState(PlayerStates.PlayerMovementStates.RUN);
        }
        else if (characterController.velocity.x == 0 && characterController.isGrounded || characterController.velocity.z == 0 && characterController.isGrounded)
        {
            playerStates.stateMachine.SwitchState(PlayerStates.PlayerMovementStates.IDLE);
        }
    }
}
