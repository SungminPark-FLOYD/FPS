using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Inpuet Keycodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift; // 달리기
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space; //점프
    [SerializeField]
    private KeyCode keyCodeReload = KeyCode.R; //재장전

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;
    [SerializeField]
    private AudioClip audioClipRun;

    private RotateToMouse rotateToMouse; //마우스 이동을 카메라 회전
    private MovementCharactorController movement;
    private Status status;
    private PlayerAnimator animator;
    private AudioSource audioSource;
    private WeaponAssultRifle weapon;

    private void Awake()
    {
        //마우스 커서를 보이지 않게 설정하고 현재위치에 고정시킨다
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

        //이동중 일때 (걷기 or 뛰기)
        if(x != 0 || z != 0)
        {
            bool isRun = false;

            //옆이나 뒤로 이동할 때는 달릴 수 없다
            if (z > 0) isRun = Input.GetKey(keyCodeRun);

            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            //방향키 입력 여부는 매 프레임 확인하기 때문에
            //재생중일 때는 다시 재생하지 않도록 isPlaying으로 채크해서 재생
            if(audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        //제자리에 멈춰있을 때
        else
        {
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;

            //멈췄을 때 사운드가 재생중이면 정지
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
