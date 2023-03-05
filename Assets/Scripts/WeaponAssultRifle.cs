using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }
public class WeaponAssultRifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazineEvent onMagazineEvent = new MagazineEvent();

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;

    [Header("Sapawn Points")]
    [SerializeField]
    private Transform casingSpawnPoint;
    [SerializeField]
    private Transform bulletSpawnPoint; 

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
    private ImpactMemoryPool impactMemoryPool;
    private Camera mainCamera;

    //�ܺο��� �ʿ��� ������ �����ϱ� ���� ������ Get Property's
    public WeaponName WeaponName => weaponSetting.weaponName;
    public int CurrentMagazine => weaponSetting.currentMagazine;
    public int MaxMagazine => weaponSetting.maxMagazine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator= GetComponentInParent<PlayerAnimator>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamera =Camera.main;

        //ó�� źâ ���� �ִ�� ����
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        //ó�� ź ���� �ִ�� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeapon);

        muzzleFlashEffect.SetActive(false);

        //���Ⱑ Ȱ��ȭ �� �� �ش� ������ źâ ������ �����Ѵ�
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
        //���Ⱑ Ȱ��ȭ �� �� �ش� ������ ź�� ������ �����Ѵ�
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    public void StartWeaponAction(int type = 0)
    {
        //������ ���� ���� ���� �׼��� �� �� ����
        if (isReload == true) return;
        //���콺 ���� Ŭ��( ���� ���� )
        if(type == 0)
        {
            
            //���Ӱ���
            if(weaponSetting.isAutomaticAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }
            //�ܹ߰���
            else
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int type = 0)
    {
        //���콺 ����Ŭ�� - ��������
        if(type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        //���� ������ ���̰ų� źâ ���� 0�̸� ������ �Ұ���      
        if(isReload == true || weaponSetting.currentMagazine <= 0) return;
        //���� �׼� ���߿� 'R'Ű�� ���� �������� �õ��ϸ� ���� �׼� ������ ����
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
            //�ٰ����� ���� ������ �� ����
            if(animator.MoveSpeed > 0.5f)
            {
                return;
            }
            //���� �ֱⰡ �Ǿ�� ������ �� �ֵ��� �ϱ����� ����ð� ����
            lastAttackTime = Time.time;

            //ź���� ������ ���� �Ұ���
            if (weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            //���ݽ� currentAmmo 1 ����
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            //���� �ִϸ��̼� ���
            animator.Play("Fire", -1, 0);
            //����Ʈ ���
            StartCoroutine("OnMuzzleFlashEffect");

            PlaySound(audioClipFire);

            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);

            //������ �߻��� ���ϴ� ��ġ ��� (Impact Effect) 
            TwoStepRaycast();
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

        //������ �ִϸ��̼�, ���� ���
        animator.OnReload();
        PlaySound(audioClipReload);

        while(true)
        {
            //���尡 ������� �ƴϰ�, ���� �ִϸ��̼��� movement�̸�
            //������ �ִϸ��̼�(,����) ����� ����Ǿ��ٴ� ��
            if(audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
            {
                isReload = false;

                //���� źâ ���� 1 ���ҽ�Ű��, �ٲ� źâ ������ ������Ʈ
                weaponSetting.currentMagazine--;
                onMagazineEvent.Invoke(weaponSetting.currentMagazine);
                //���� ź���� �ִ�� �����ϰ�, �ٲ� ź �� ������ ������Ʈ
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break;
            }

            yield return null;
        }
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        //ȭ���� �߾� ��ǥ (Aim �������� Raycast ����)
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        //���� ��Ÿ� �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� ������ �ε��� ��ġ
        if(Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }
        //���� ��Ÿ� �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� �ִ� ��Ÿ� ��ġ
        else
        {
            targetPoint = ray.origin + ray.direction*weaponSetting.attackDistance;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red);

        //ù��° Raycast�������� ����� targetPoint�� ��ǥ�������� �����ϰ�,
        //�ѱ��� ������������ �Ͽ� Raycast ����
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        if(Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }
    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
