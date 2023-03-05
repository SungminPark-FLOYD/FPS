using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponName { AssaultRifle = 0 }
[System.Serializable] 
//�� �̿��� ����ȭ�� ���� ������ �ٸ� Ŭ������ ������ �����Ǿ�����
//�ν����Ϳ� ���� �ʴ´�
public struct WeaponSetting  //struct(����ü)�� ���� ���� Ŭ������ �� ������ �޸� �Ҵ�ȴ� 
{
    public WeaponName weaponName;   //���� �̸�
    public int currentMagazine;     //���� źâ��
    public int maxMagazine;         //�ִ� źâ��
    public int currentAmmo;         //���� ź���
    public int maxAmmo;             //�ִ� ź���
    public float attackRate;        //���ݼӵ�
    public float attackDistance;    //���� ��Ÿ�
    public bool isAutomaticAttack; //���� ���� ����
}
