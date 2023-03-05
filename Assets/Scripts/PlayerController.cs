using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Inpuet Keycodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift; // �޸���
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space; //����
    [SerializeField]
    private KeyCode keyCodeReload = KeyCode.R; //������

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;
    [SerializeField]
    private AudioClip audioClipRun;

    private RotateToMouse rotateToMouse; //���콺 �̵��� ī�޶� ȸ��
    private MovementCharactorController movement;
    private Status status;
    private PlayerAnimator animator;
    private AudioSource audioSource;
    private WeaponAssultRifle weapon;

    private void Awake()
    {
        //���콺 Ŀ���� ������ �ʰ� �����ϰ� ������ġ�� ������Ų��
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MovementCharactorController>();
        status = GetComponent<Status>();
        animator = GetComponent<PlayerAnimator>();
        audioSource = GetComponent<AudioSource>();
        weapon = GetComponentInChildren<WeaponAssultRifle>();
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //�̵��� �϶� (�ȱ� or �ٱ�)
        if(x != 0 || z != 0)
        {
            bool isRun = false;

            //���̳� �ڷ� �̵��� ���� �޸� �� ����
            if (z > 0) isRun = Input.GetKey(keyCodeRun);

            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            //����Ű �Է� ���δ� �� ������ Ȯ���ϱ� ������
            //������� ���� �ٽ� ������� �ʵ��� isPlaying���� äũ�ؼ� ���
            if(audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        //���ڸ��� �������� ��
        else
        {
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;

            //������ �� ���尡 ������̸� ����
            if(audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }

        movement.MoveTo(new Vector3(x, 0, z));
    }
    void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    void UpdateJump()
    {
        if(Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }

    void UpdateWeaponAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction();
        }
        else if( Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }

        if(Input.GetKeyDown(keyCodeReload))
        {
            weapon.StartReload();
        }
    }
}
