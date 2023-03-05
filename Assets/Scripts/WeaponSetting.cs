using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponName { AssaultRifle = 0 }
[System.Serializable] 
//를 이용해 직렬화를 하지 않으면 다른 클레스의 변수로 생성되었을때
//인스펙터에 뜨지 않는다
public struct WeaponSetting  //struct(구조체)는 스택 영역 클래스는 힙 영역에 메모리 할당된다 
{
    public WeaponName weaponName;   //무기 이름
    public int currentMagazine;     //현재 탄창수
    public int maxMagazine;         //최대 탄창수
    public int currentAmmo;         //현재 탄약수
    public int maxAmmo;             //최대 탄약수
    public float attackRate;        //공격속도
    public float attackDistance;    //공격 사거리
    public bool isAutomaticAttack; //연속 공격 여부
}
