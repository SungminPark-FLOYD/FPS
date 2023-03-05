using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WeaponAssultRifle weapon;   //현재 정보

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI textWeaponName; //무기 이름
    [SerializeField]
    private Image imageWeaponIcon;          //무기 아이콘
    [SerializeField]
    private Sprite[] spriteWeaponIcons;     //스프라이트 정보

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI textAmmo;       //현재 / 최대 탄수 출력

    private void Awake()
    {
        SetUpWeapon();

        //메소드가 등록되어 있는 이벤트 클래스의
        //Invoke() 메소드가 호출될 때 등록된 메소드가 실행된다
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
