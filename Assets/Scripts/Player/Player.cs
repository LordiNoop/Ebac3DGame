using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cloth;
using Ebac.Core.Singleton;

public class Player : Singleton<Player>//, IDamageable
{
    [SerializeField] private PlayerStates playerStates;

    public List<Collider> colliders;
    
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

    [Header("Life")]
    public HealthBase healthBase;

    [Space]
    [SerializeField] private ClothChanger _clothChanger;

    private bool _alive = true;
    private bool _jumping = false;

    private void OnValidate()
    {
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
    }

    protected override void Awake()
    {
        base.Awake();
        OnValidate();

        healthBase.OnDamage += Damage;
        healthBase.OnKill += OnKill;

        LoadPlayerPosition();
    }

    private void LoadPlayerPosition()
    {
        transform.position = SaveManager.Instance.Setup.playerPosition;
    }

    private void Update() 
    { 
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0); 

        var inputAxisVertical = Input.GetAxis("Vertical"); 
        var speedVector = transform.forward * inputAxisVertical * speed;

        if (characterController.isGrounded)
        {
            if (_jumping)
            {
                _jumping = false;
                animator.SetTrigger("Land");
            }

            vSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vSpeed = jumpSpeed;
                playerStates.stateMachine.SwitchState(PlayerStates.PlayerMovementStates.JUMP);

                if (!_jumping)
                {
                    _jumping = true;
                    animator.SetTrigger("Jump");
                }
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
    private void OnKill(HealthBase h)
    {
        if (_alive)
        {
            _alive = false;
            animator.SetTrigger("Death");
            colliders.ForEach(i => i.enabled = false);

            Invoke(nameof(Revive), 3f);
        }
    }

    private void Revive()
    {
        Respawn();
        healthBase.ResetLife();
        animator.SetTrigger("Revive");
        colliders.ForEach(i => i.enabled = true);
        healthBase.UpdateUI();
        _alive = true;
    }

    public void Damage(HealthBase h)
    {
        //flashColors.ForEach(i => i.Flash());
        EffectsManager.Instance.ChangeVignette();
        ShakeCamera.Instance.Shake();
    }

    public void Damage(float damage, Vector3 dir)
    {
        //Damage(damage);
    }

    #endregion

    [NaughtyAttributes.Button]
    public void Respawn()
    {
        if(CheckpointManager.Instance.HasCheckpoint())
        {
            transform.position = CheckpointManager.Instance.GetPositionFromLastCheckPoint();
        }
    }

    public void ChangeSpeed(float speed, float duration)
    {
        StartCoroutine(ChangeSpeedCoroutine(speed, duration));
    }

    IEnumerator ChangeSpeedCoroutine(float localSpeed, float duration)
    {
        var deafultSpeed = speed;
        speed = localSpeed;
        yield return new WaitForSeconds(duration);
        speed = deafultSpeed;
    }

    public void ChangeTexture(ClothSetup setup, float duration)
    {
        StartCoroutine(ChangeTextureCoroutine(setup, duration));
    }

    IEnumerator ChangeTextureCoroutine(ClothSetup setup, float duration)
    {
        _clothChanger.ChangeTexture(setup);
        yield return new WaitForSeconds(duration);
        _clothChanger.ResetTexture();
    }
}
