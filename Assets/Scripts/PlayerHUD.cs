using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WeaponAssultRifle weapon;   //���� ����

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI textWeaponName; //���� �̸�
    [SerializeField]
    private Image imageWeaponIcon;          //���� ������
    [SerializeField]
    private Sprite[] spriteWeaponIcons;     //��������Ʈ ����

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI textAmmo;       //���� / �ִ� ź�� ���

    private void Awake()
    {
        SetUpWeapon();

        //�޼ҵ尡 ��ϵǾ� �ִ� �̺�Ʈ Ŭ������
        //Invoke() �޼ҵ尡 ȣ��� �� ��ϵ� �޼ҵ尡 ����ȴ�
        weapon.onAmmoEvent.AddListener(UpdateAmmoHUD);
    }

    void SetUpWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();
        imageWeaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
    }

    private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    }
}
