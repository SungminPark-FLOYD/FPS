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

    [Header("Magazine")]
    [SerializeField]
    private GameObject magazineUIPrefab;    //탄창 UI 프리펩
    [SerializeField]
    private Transform magazineParent;       //탄창 UI에 배치되는 패널

    private List<GameObject> magazineList;  //탄창 UI 리스트

    private void Awake()
    {
        SetUpWeapon();
        SetUpMagazine();

        //메소드가 등록되어 있는 이벤트 클래스의
        //Invoke() 메소드가 호출될 때 등록된 메소드가 실행된다
        weapon.onAmmoEvent.AddListener(UpdateAmmoHUD);
        weapon.onMagazineEvent.AddListener(UpdateMagazineHUD);
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

    private void SetUpMagazine()
    {
        //weapon에 등록되어 있는 최대 탄창 개수만큼 Image Icon을 생성
        //magazineParent 오브젝트의 자식으로 등록후 모두 비활성화 / 활성화 리스트에 저장
        magazineList = new List<GameObject>();
        for (int i = 0; i < weapon.MaxMagazine; ++i)
        {
            GameObject clone = Instantiate(magazineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
        }

        //weapon에 등록되어 있는 현재 탄창 개수만큼 오브젝트 활성화
        for(int i=0; i < weapon.CurrentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }

    private void UpdateMagazineHUD(int currentMagazine)
    {
        //전부 비활성화 하고, currentMagazine 개수만큼 활성화
        for(int i = 0; i < magazineList.Count; ++i)
        {
            magazineList[i].SetActive(false);
        }
        for(int i = 0; i < currentMagazine; ++ i)
        {
            magazineList[i].SetActive(true);
        }
    }
}
