using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerController : NetworkBehaviour
{
    CharacterInput characterInput;
    Vector2 moveInput;
    Vector3 moveDirection;
    NetworkCharacterControllerPrototype characterControllerPrototype;
    Animator animator;
    float speed = 5f;
    private int target, beforeTarget;
    private float lateMagnitude, currentSpeed;
    /*[Networked]
    Vector3 velocity { get; set; }*/
    bool isGround, isJump;
    private void Awake()
    {
        characterInput= new CharacterInput();
        characterControllerPrototype= GetComponent<NetworkCharacterControllerPrototype>();
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
         if(characterInput.Character.Jump.triggered)
        {
            isJump = true; ;
        };
        if (isGround)
        {
          //  velocity = Vector3.zero;
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
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        
        if (moveDirection.magnitude > 0)
        {
            animator.SetFloat("Speed", 1f);
            Quaternion angleCamera = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up);
            Quaternion lookRotation = Quaternion.LookRotation(angleCamera * moveDirection);
            transform.rotation = lookRotation;
            characterControllerPrototype.Move(transform.forward * 5 * Time.deltaTime);
            CalculateAnimSpeed(1f);
        }
        else
        {
            CalculateAnimSpeed();
        }
      // characterControllerPrototype.Move(velocity * Time.deltaTime);
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        CalculateMove();
        if(isJump)
        {
            Jump(10);
        }
    }
    private void FixedUpdate()
    {
        CalculateMove();
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
    void Jump(float jumpForce)
    {
        isJump = false;
        animator.SetTrigger("Jump");
        /*velocity += new Vector3(0, jumpForce, 0);
        isGround =false;*/
        characterControllerPrototype.Move(new Vector3(0,jumpForce,0));    
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    { 
            isGround = hit.collider.gameObject.layer == 3;
        Debug.Log("vo controlerhit" + "is ground" + isGround);
    }

}
