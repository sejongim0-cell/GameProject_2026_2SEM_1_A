using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public enum PlayerState
{
    Normal,
    
    Pickup,

    inventory
}

public class PlayerContoller : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("바닥 설정")]
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private float verticalVeolocity;

    private PlayerState currentState = PlayerState.Normal;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
;    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if(keyboard == null)
        {
            return;
        }

        //중력은 상태와 무관하게 상시 적용
        ApplyGravity();

        //normal 상태가 아니면 이동 입력X
        if (currentState != PlayerState.Normal)
        {
            return;
        }

        HandleMovement(keyboard);

    }


    private void HandleMovement(Keyboard keyboard)
    {
        Vector2 input = Vector2.zero;

        if (keyboard.aKey.isPressed)
            input.x -= 1f;
        if (keyboard.dKey.isPressed)
            input.x += 1f;
        if (keyboard.sKey.isPressed)
            input.y -= 1f;
        if (keyboard.wKey.isPressed)
            input.y += 1f;

        input = Vector2.ClampMagnitude(input, 1f);

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // 카메라 위아래 기울기는 이동에 사용X
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        //카메라 기준 이동 방향

        Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        //Shift 달리기
        bool isRunning = keyboard.leftShiftKey.isPressed;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        //수평 이동
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        //이동 방향으로 회전
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 애니메이션 설정

        float animationSpeed = 0f;

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            animationSpeed = isRunning ? 1f : 0.5f;
        }

        animator.SetFloat("speed", animationSpeed, 0.1f, Time.deltaTime);
    }


    private void ApplyGravity()
    {
        //기본 중력
        if (controller.isGrounded && verticalVeolocity < 0f)
        {
            verticalVeolocity = -2f;
        }
        else
        {
            verticalVeolocity += gravity * Time.deltaTime;
        }

        controller.Move(Vector3.up * verticalVeolocity * Time.deltaTime);
    }

    public void ChangeState(PlayerState newState)
    {
        currentState = newState;

        if(currentState != PlayerState.Normal)
        {
            animator.SetFloat("speed", 0);
        }

        Debug.LogError(currentState);
    }

    public bool IsNormalState()
    {
        return currentState == PlayerState.Normal;
    }

}
