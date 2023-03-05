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
    private float jumpForce; //���� ��
    [SerializeField]
    private float gravity; //�߷� ���
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }
    private CharacterController characterController; //�÷��̾� �̵����� ������Ʈ

    private void Awake()
    {  
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //����� �������� �߷¸�ŭ y�� �̵��ӵ� ����
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }

        characterController.Move(moveForce * Time.deltaTime);

    }

    public void MoveTo(Vector3 dir)
    {
        //�̵����� = ĳ������ ȸ�� �� * ���� ��
        dir = transform.rotation * new Vector3(dir.x, 0, dir.z);

        //�̵� �� = �̵����� * �ӵ�
        moveForce = new Vector3(dir.x * moveSpeed, moveForce.y, dir.z * moveSpeed);
    }

    public void Jump()
    {
        //�÷��̾ �ٴڿ� ���� ���� ���� ����
        if(characterController.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }
}
