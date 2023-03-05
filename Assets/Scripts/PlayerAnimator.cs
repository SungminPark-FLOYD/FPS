using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        //"Player" ������Ʈ �������� �ڽ� ������Ʈ�� 
        //arms ��¼�� ������Ʈ�� animtor ������Ʈ�� �ִ�
        animator = GetComponentInChildren<Animator>();
    }
    
    public float MoveSpeed
    {
        //animator.SetFloat("paramname", value) �ִϸ����� �信 �ִ� float Ÿ�� ���� param�� ���� value�� ����
        set => animator.SetFloat("movementSpeed", value);
        //animator.GetFloat("paramname", value) �ִϸ����� �信�ִ� float Ÿ�� ������ ��ȯ
        get => animator.GetFloat("movementSpeed");
    }

    public void OnReload()
    {
        animator.SetTrigger("onReload");
    }

    public void Play(string stateName, int layer, float normalizedTime)
    {
       animator.Play(stateName, layer, normalizedTime);
    }

    public bool CurrentAnimationIs(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
