using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESctl : MonoBehaviour
{
    public float rotationSpeed = 50.0f; // ����t��

    // ��s�C�V�I�s���禡
    void Update()
    {
        // �ھڱ���t�צb Z �b�W���ફ��
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
