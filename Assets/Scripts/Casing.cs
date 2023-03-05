using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Casing : MonoBehaviour
{
    [SerializeField]
    private float deactiveTime = 5.0f; //ź�� ��Ȱ��ȭ �ð�
    [SerializeField]
    private float casingSpin = 1.0f; //ź�� ȸ���ӵ�
    [SerializeField]
    private AudioClip[] audioClips; //ź�� �浹 �Ҹ�

    private Rigidbody rigidbody3D;
    private AudioSource audioSource;
    private MemoryPool memoryPool;

    public void SetUp(MemoryPool pool, Vector3 dir)
    {
        rigidbody3D = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        memoryPool = pool;

        //ź���� �̵� �ӷ°� ȸ�� �ӷ� ����
        rigidbody3D.velocity = new Vector3(dir.x, 1.0f, dir.z);
        rigidbody3D.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin),
                                                  Random.Range(-casingSpin, casingSpin),
                                                  Random.Range(-casingSpin, casingSpin));

        //ź�� �ڵ�Ȱ��ȭ�� ���� �ڷ�ƾ ����
        StartCoroutine("DeactiveAfterTime");

    }

    private void OnCollisionEnter(Collision collision)
    {
        //���� ���� ź�� ���� �� ������ ���� ����
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
