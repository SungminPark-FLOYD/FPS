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

    [Header("Magazine")]
    [SerializeField]
    private GameObject magazineUIPrefab;    //źâ UI ������
    [SerializeField]
    private Transform magazineParent;       //źâ UI�� ��ġ�Ǵ� �г�

    private List<GameObject> magazineList;  //źâ UI ����Ʈ

    private void Awake()
    {
        SetUpWeapon();
        SetUpMagazine();

        //�޼ҵ尡 ��ϵǾ� �ִ� �̺�Ʈ Ŭ������
        //Invoke() �޼ҵ尡 ȣ��� �� ��ϵ� �޼ҵ尡 ����ȴ�
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
        //weapon�� ��ϵǾ� �ִ� �ִ� źâ ������ŭ Image Icon�� ����
        //magazineParent ������Ʈ�� �ڽ����� ����� ��� ��Ȱ��ȭ / Ȱ��ȭ ����Ʈ�� ����
        magazineList = new List<GameObject>();
        for (int i = 0; i < weapon.MaxMagazine; ++i)
        {
            GameObject clone = Instantiate(magazineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
        }

        //weapon�� ��ϵǾ� �ִ� ���� źâ ������ŭ ������Ʈ Ȱ��ȭ
        for(int i=0; i < weapon.CurrentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }

    private void UpdateMagazineHUD(int currentMagazine)
    {
        //���� ��Ȱ��ȭ �ϰ�, currentMagazine ������ŭ Ȱ��ȭ
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
