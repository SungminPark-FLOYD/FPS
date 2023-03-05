using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
public class WeaponAssultRifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;

    [Header("Sapawn Points")]
    [SerializeField]
    private Transform casingSpawnPoint;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon;
    [SerializeField]
    private AudioClip audioClipFire;
    [SerializeField]
    private AudioClip audioClipReload;

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting weaponSetting;

    private float lastAttackTime = 0;
    private bool isReload = false;

    private AudioSource audioSource;
    private PlayerAnimator animator;
    private CasingMemoryPool casingMemoryPool;

    //외부에서 필요한 정보를 열람하기 위해 정의한 Get Property's
    public WeaponName WeaponName => weaponSetting.weaponName;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator= GetComponentInParent<PlayerAnimator>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();

        //처음 탄 수는 최대로 설정
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeapon);

        muzzleFlashEffect.SetActive(false);

        //무기가 활성화 될 때 해당 무기의 탄수 정보를 갱신한다
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    public void StartWeaponAction(int type = 0)
    {
        //재장전 중일 때는 무기 액션을 할 수 없다
        if (isReload == true) return;
        //마우스 왼쪽 클릭( 공격 시작 )
        if(type == 0)
        {
            
            //연속공격
            if(weaponSetting.isAutomaticAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }
            //단발공격
            else
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int type = 0)
    {
        //마우스 왼쪽클릭 - 공격종료
        if(type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        //현재 재장전 중이면 재장전 불가능
        if(isReload == true) return;
        //무기 액션 도중에 'R'키를 눌러 재장전을 시도하면 무기 액션 종료후 장전
        StopWeaponAction();

        StartCoroutine("OnReload");
    }

    private IEnumerator OnAttackLoop()
    {
        while(true)
        {
            OnAttack();

            yield return null;
        }
    }

    public void OnAttack()
    {
        if(Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            //뛰고있을 때는 공격할 수 없다
            if(animator.MoveSpeed > 0.5f)
            {
                return;
            }
            //공격 주기가 되어야 공격할 수 있도록 하기위해 현재시간 저장
            lastAttackTime = Time.time;

            //탄수가 없으면 공격 불가능
            if (weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            //공격시 currentAmmo 1 감소
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            //무기 애니메이션 재생
            animator.Play("Fire", -1, 0);
            //이펙트 재생
            StartCoroutine("OnMuzzleFlashEffect");

            PlaySound(audioClipFire);

            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);
        }
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);

        muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        isReload = true;

        //재장전 애니메이션, 사운드 재생
        animator.OnReload();
        PlaySound(audioClipReload);

        while(true)
        {
            //사운드가 재생중이 아니고, 현재 애니메이션이 movement이면
            //재장전 애니메이션(,사운드) 재생이 종료되었다는 뜻
            if(audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
            {
                isReload = false;

                //현재 탄수를 최대로 설정하고, 바뀐 탄 수 정보를 업데이트
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break;
            }

            yield return null;
        }
    }
    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
