using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementCharactorController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Vector3 moveForce;

    [SerializeField]
    private float jumpForce; //점프 힘
    [SerializeField]
    private float gravity; //중력 계수
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }
    private CharacterController characterController; //플레이어 이동제어 컴포넌트

    private void Awake()
    {  
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //허공에 떠있으면 중력만큼 y축 이동속도 감소
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }

        characterController.Move(moveForce * Time.deltaTime);

    }

    public void MoveTo(Vector3 dir)
    {
        //이동방향 = 캐릭터의 회전 값 * 방향 값
        dir = transform.rotation * new Vector3(dir.x, 0, dir.z);

        //이동 힘 = 이동방향 * 속도
        moveForce = new Vector3(dir.x * moveSpeed, moveForce.y, dir.z * moveSpeed);
    }

    public void Jump()
    {
        //플레이어가 바닥에 있을 때만 점프 가능
        if(characterController.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }
}
