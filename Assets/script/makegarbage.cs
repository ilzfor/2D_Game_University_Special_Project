using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makegarbage : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // �s��n�ͦ�������
    public float spawnInterval = 1.0f; // �ͦ����j�ɶ�
    private float timer = 0.0f;

    void Update()
    {
        // ��s�p�ɾ�
        timer += Time.deltaTime;

        // �p�G�p�ɾ��W�L�ͦ����j�ɶ��A�ͦ�����í��m�p�ɾ�
        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0.0f;
        }
    }

    void SpawnObject()
    {
        // �T�O objectsToSpawn �����ťB�ܤ֥]�t�@�Ӫ���
        if (objectsToSpawn != null && objectsToSpawn.Length > 0)
        {
            // �q objectsToSpawn ���H����ܤ@�Ӫ���ͦ�
            int randomIndex = Random.Range(0, objectsToSpawn.Length);
            Instantiate(objectsToSpawn[randomIndex], transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("No objects to spawn. Please assign objects to objectsToSpawn array in the inspector.");
        }
    }
}
