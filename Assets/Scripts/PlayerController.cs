using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static Cinemachine.DocumentationSortingAttribute;
public enum CharacterState
{
    Normal,
    RightAttack,
    Damaged
}
public class PlayerController : NetworkBehaviour, ICanTakeDamage
{
    PlayerStat playerStat = new PlayerStat(damage: 20, health: 100);
    CharacterInput characterInput;
    Vector2 moveInput;
    Vector3 moveDirection;
    CharacterController characterControllerPrototype;
    Animator animator;
    float speed = 5f;
    private int target, beforeTarget;
    private float lateMagnitude, currentSpeed;
    bool isGround;
    [Networked]
    bool isJumping {get;set;}
    [Networked]
    bool isRightClickAttack { get; set; }
    [Networked]
    float jumpHeight { get; set; }
    Vector3 velocity;
    [SerializeField]
    CharacterState currentState;
    [SerializeField]
    GameObject rightAttackObject;
    [SerializeField]
    Transform rightAttackTransform;

    

    private void Awake()
    {
        characterInput= new CharacterInput();
        characterControllerPrototype= GetComponent<CharacterController>();
        animator=GetComponent<Animator>();
    }
    public override void Spawned()
    {
        base.Spawned();
       

        if (Object.InputAuthority.PlayerId == Runner.LocalPlayer.PlayerId)
        {
            Singleton<CameraController>.Instance.SetFollowCharacter(transform);
        }
    }
    void Update()
    {
        moveInput=characterInput.Character.Move.ReadValue<Vector2>();
         if(characterInput.Character.Jump.triggered &&isGround)
        {
            isJumping = true;
        };
        if (isGround)
        {
            velocity = Vector3.zero;
        }
        if (characterInput.Character.Attack.triggered && currentState==CharacterState.Normal)
        {
            isRightClickAttack = true;
        }
        
    }
    private void OnEnable()
    {
        characterInput.Enable();
    }
    private void OnDisable()
    {
        characterInput.Disable();
    }
    void CalculateMove()
    {
        if (HasStateAuthority)
        {
            moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

            if (moveDirection.magnitude > 0)
            {
                if (!isJumping)
                {
                    animator.SetFloat("Speed", 1f);
                }
                Quaternion angleCamera = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up);
                Quaternion lookRotation = Quaternion.LookRotation(angleCamera * moveDirection);
                transform.rotation = lookRotation;
                characterControllerPrototype.Move(transform.forward * 20 * Time.deltaTime);
                CalculateAnimSpeed(1f);
            }
            else
            {
                CalculateAnimSpeed();
            }
            if (!isGround)
            {
                if (isJumping)
                {
                    isJumping = false;
                }
                velocity += new Vector3(0, -100f * Runner.DeltaTime, 0);

                characterControllerPrototype.Move(velocity * Time.deltaTime);
            }
            else if (isJumping)
            {
                isGround = false;
                velocity.y=0;
               // velocity.y += 5f;
                characterControllerPrototype.Move(velocity * Time.deltaTime);
            }
            
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (currentState==CharacterState.RightAttack)
        {
            return;
        }
        if (isJumping&& HasStateAuthority)
        {
            isGround=false;
            Jump(50);
            Debug.Log("jump");
        }
     CalculateMove();
        if (isRightClickAttack && isGround)
        {
            animator.SetTrigger("Attack");
            Runner.Spawn(rightAttackObject, rightAttackTransform.position, inputAuthority:Object.InputAuthority
     ,onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
     {
         obj.GetComponent<RightClickAttackObject>().SetDirection(transform.forward);
     }
                        );
            isRightClickAttack = false;
        }
        
       
    }
    
    public void SwithCharacterState(CharacterState newCatState)
    {
        //Khi ket thuc trang thai cu thi toi lam gi do...
        switch (currentState)
        {
            case CharacterState.Normal: { break; }
            case CharacterState.RightAttack: { break; }
            case CharacterState.Damaged: { break; }
        }
        //Bat dau trang thai moi thi toi lam gi do...
        switch (newCatState)
        {
            case CharacterState.Normal: { break; }
            case CharacterState.RightAttack: { break; }
            case CharacterState.Damaged:
                {
                    animator.SetTrigger("Damaged");
                    break;
                }
        }
        currentState = newCatState;

    }

    private void CalculateAnimSpeed(float speed = 0)
    {
        currentSpeed = speed;
        if (lateMagnitude != currentSpeed)
        {
            StartCoroutine(CaculateSmoothAnimation("Speed", speed));
        }
        lateMagnitude = speed;
    }

    IEnumerator CaculateSmoothAnimation(string animationName, float? Speedtarget = null)
    {
        float time = 0;
        float start = animator.GetFloat(animationName);
        float x = Speedtarget == null ? 2 : 5;
        float targetTime = 1 / x;
        while (time <= targetTime)
        {
            //doi lai 1 khung hinh
            yield return null;
            if (Speedtarget != null
                && Speedtarget != currentSpeed)
            {
                time = targetTime;
                break;
            }
            float valueRandomSmooth = Mathf.Lerp(start, Speedtarget == null ? target : Speedtarget.Value, x * time);
            animator.SetFloat(animationName, valueRandomSmooth);
            time += Time.deltaTime;
        }
    }
    void Jump(float jumpHeight)
    {
        isJumping = false;
        animator.SetTrigger("Jump");
        velocity += new Vector3(0, jumpHeight , 0);

    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    { 
        if (hit.collider.CompareTag("Ground") && !isGround)
        {
            isGround = true;
        }
    }

    public void ApplyDamage(int damage, PlayerRef player, Action callback = null)
    {
        CalculateHealthRPC(damage, player);
        callback?.Invoke();
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void CalculateHealthRPC(int damage, PlayerRef player)
    {

    }
}
