using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioctrl : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public List<AudioClip> specialSounds = new List<AudioClip>(); // �s�W�@�� List �Ӧs�x�S�w����
    private float timer = 0f;
    private float soundInterval = 3f; // �C 3 ����@���S�w����

    void Start()
    {
        // �T�O�I�����֤����ŨåB�ҥδ`������
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    void Update()
    {
        // �p�G�I�����֦s�b�A�åB���b����A�h�C�L soundInterval ����S�w����
        if (backgroundMusic != null && backgroundMusic.isPlaying)
        {
            timer += Time.deltaTime;
            if (timer >= soundInterval)
            {
                PlayRandomSpecialSound(); // ��惡�B�Ӽ����H�����S�w����
                timer = 0f; // ���m�p�ɾ�
            }
        }
    }

    void PlayRandomSpecialSound()
    {
        // �T�O�S�w���ĦC��M AudioSource ���s�b
        if (specialSounds.Count > 0 && backgroundMusic != null)
        {
            int randomIndex = Random.Range(0, specialSounds.Count);
            backgroundMusic.PlayOneShot(specialSounds[randomIndex]);
        }
    }
}
