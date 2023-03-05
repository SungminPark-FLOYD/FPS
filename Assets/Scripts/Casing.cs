using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Casing : MonoBehaviour
{
    [SerializeField]
    private float deactiveTime = 5.0f; //탄피 비활성화 시간
    [SerializeField]
    private float casingSpin = 1.0f; //탄피 회전속도
    [SerializeField]
    private AudioClip[] audioClips; //탄피 충돌 소리

    private Rigidbody rigidbody3D;
    private AudioSource audioSource;
    private MemoryPool memoryPool;

    public void SetUp(MemoryPool pool, Vector3 dir)
    {
        rigidbody3D = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        memoryPool = pool;

        //탄피의 이동 속력과 회전 속력 설정
        rigidbody3D.velocity = new Vector3(dir.x, 1.0f, dir.z);
        rigidbody3D.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin),
                                                  Random.Range(-casingSpin, casingSpin),
                                                  Random.Range(-casingSpin, casingSpin));

        //탄피 자동활정화를 위한 코루틴 실행
        StartCoroutine("DeactiveAfterTime");

    }

    private void OnCollisionEnter(Collision collision)
    {
        //여러 개의 탄피 사운드 중 임의의 사운드 선택
        int index = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    private IEnumerator DeactiveAfterTime()
    {
        yield return new WaitForSeconds(deactiveTime);

        memoryPool.DeactivePoolItem(this.gameObject);
    }


}
