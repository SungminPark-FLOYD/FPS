using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        //"Player" 오브젝트 기준으로 자식 오브젝트인 
        //arms 어쩌구 오브젝트에 animtor 컴포넌트가 있다
        animator = GetComponentInChildren<Animator>();
    }
    
    public float MoveSpeed
    {
        //animator.SetFloat("paramname", value) 애니메이터 뷰에 있는 float 타입 변수 param의 값을 value로 설정
        set => animator.SetFloat("movementSpeed", value);
        //animator.GetFloat("paramname", value) 애니메이터 뷰에있는 float 타입 변수를 반환
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
