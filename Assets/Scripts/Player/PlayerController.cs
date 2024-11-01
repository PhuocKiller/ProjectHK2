using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static Cinemachine.DocumentationSortingAttribute;

public enum CharacterState
{
    Normal,
    Jump,
    BasicAttack,
    Injured,
    Die,

}
public class PlayerController : NetworkBehaviour, ICanTakeDamage
{
    PlayerStat playerStat = new PlayerStat(maxHealth: 100, maxMana:50, damage:20);
    CharacterInput characterInput;
    Vector2 moveInput;
    Vector3 moveDirection;
    CharacterController characterControllerPrototype;
    Animator animator;
    float speed;
    private int targetX, targetY, beforeTarget;
    private float previousSpeedX, currentSpeedX,previousSpeedY, currentSpeedY;
    bool isGround;
    [Networked]
    bool isJumping {get;set;}
    [Networked]
    bool isBasicAttackAttack { get; set; }
    [Networked]
    float jumpHeight { get; set; }
    Vector3 velocity;
    [SerializeField]
    CharacterState currentState;
    [SerializeField]
    GameObject basicAttackObject;
    [SerializeField]
    Transform basicAttackTransform;
    [SerializeField]
    TextMeshProUGUI textHealth;

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
        if (currentState == CharacterState.BasicAttack || currentState == CharacterState.Jump)
        {
            return;
        }
        moveInput =characterInput.Character.Move.ReadValue<Vector2>();
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
            isBasicAttackAttack = true;
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
            CalculateAnimSpeed("MoveX", moveInput.x,true);
            CalculateAnimSpeed("MoveY", moveInput.y,false);
            speed=2f+Vector2.Dot(moveInput, Vector2.up);
            Quaternion angleCamera = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up);
            
            if (moveDirection.magnitude > 0)
            {
                if (!isJumping)
                {
                  //  animator.SetFloat("Speed", 1f);
                }
                
               // Quaternion lookRotation = Quaternion.LookRotation(angleCamera * moveDirection);
                
                characterControllerPrototype.Move(angleCamera*moveDirection * speed *3* Time.deltaTime);
            }
            transform.rotation = angleCamera;
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
        if (currentState==CharacterState.BasicAttack)
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
        if (isBasicAttackAttack && isGround)
        {
            animator.SetTrigger("Attack");
            Runner.Spawn(basicAttackObject, basicAttackTransform.position, inputAuthority:Object.InputAuthority
     ,onBeforeSpawned: (NetworkRunner runner, NetworkObject obj) =>
     {
         obj.GetComponent<BasicAttackObject>().SetDirection(transform.forward);
     }
                        );
            isBasicAttackAttack = false;
        }
        textHealth.text=((int)playerStat.currentHealth).ToString() + "/" + ((int)playerStat.maxHealth).ToString();
    }
    
    public void SwithCharacterState(CharacterState newCatState)
    {
        //Khi ket thuc trang thai cu thi toi lam gi do...
        switch (currentState)
        {
            case CharacterState.Normal: { break; }
            case CharacterState.BasicAttack: { break; }
            case CharacterState.Injured: { break; }
            case CharacterState.Die: { break; }
        }
        //Bat dau trang thai moi thi toi lam gi do...
        switch (newCatState)
        {
            case CharacterState.Normal: { break; }
            case CharacterState.BasicAttack: { break; }
            case CharacterState.Injured:
                {
                    
                    break;
                }
            case CharacterState.Die: 
                {
                    animator.SetTrigger("Die");
                    break; }
        }
        currentState = newCatState;

    }

    private void CalculateAnimSpeed(string animationName,float speed, bool isMoveX)
    {
        if(isMoveX)
        {
            currentSpeedX = speed;
        }
        else
        {
            currentSpeedY = speed;
        }
           
        if (isMoveX && previousSpeedX != currentSpeedX)
        {
            StartCoroutine(CaculateSmoothAnimation(animationName,true, speed));
        }
        if (!isMoveX && previousSpeedY != currentSpeedY)
        {
            StartCoroutine(CaculateSmoothAnimation(animationName,false, speed));
        }

        if (isMoveX)
        {
            previousSpeedX = speed;
        }
        else
        {
            previousSpeedY = speed;
        }
    }

    IEnumerator CaculateSmoothAnimation(string animationName,bool isMoveX, float? Speedtarget = null)
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
                && Speedtarget != (isMoveX? currentSpeedX:currentSpeedY))
            {
                time = targetTime;
                break;
            }
            float valueRandomSmooth = Mathf.Lerp(start, Speedtarget == null ?
                (isMoveX? targetX:targetY ): Speedtarget.Value, x * time);
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
    public void CheckCamera(PlayerRef player, bool isFollow)
    {
        if (player == Runner.LocalPlayer)
        {
            if (isFollow)
            {
                Singleton<CameraController>.Instance.SetFollowCharacter(transform);
            }
            else
            {
                Singleton<CameraController>.Instance.RemoveFollowCharacter();
            }
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    { 
        if (hit.collider.CompareTag("Ground") && !isGround)
        {
            isGround = true;
        }
    }

    public void ApplyDamage(float damage, PlayerRef player, Action callback = null)
    {
        CalculateHealthRPC(damage, player);
        callback?.Invoke();
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void CalculateHealthRPC(float damage, PlayerRef player)
    {
        if (playerStat.currentHealth >damage)
        {
            animator.SetTrigger("Injured");
            playerStat.currentHealth -= damage;
        }
        else
        {
            playerStat.currentHealth = 0;
            SwithCharacterState(CharacterState.Die);
        }
        
    }
    /*private void OnCollisionEnter(Collision hit)
    {
        InventoryItemBase item = hit.collider.GetComponent<InventoryItemBase>();
        if (item != null)
        {
           Inventory.instance.AddItem(item);
        }
        
    }*/
    private void OnTriggerEnter(Collider other)
    {
        InventoryItemBase item = other.GetComponent<InventoryItemBase>();
        if (item != null)
        {
            Singleton<Inventory>.Instance.AddItem(item);
            item.OnPickUp();
        }
    }
    



}
