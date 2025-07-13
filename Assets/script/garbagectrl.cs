using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garbagectrl : MonoBehaviour
{
    public float minMoveSpeed = 2.0f; // ���ʳt�ת��̤p��
    public float maxMoveSpeed = 5.0f; // ���ʳt�ת��̤j��
    public float minFloatStrength = 0.3f; // �W�U�}�B���̤p��
    public float maxFloatStrength = 0.8f; // �W�U�}�B���̤j��

    private float moveSpeed;
    private float floatStrength;
    private Vector2 moveDirection = Vector2.right; // �w�]�V�k����

    void Start()
    {
        // �b���w���϶����H�����Ͳ��ʳt�שM�}�B�j��
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        floatStrength = Random.Range(minFloatStrength, maxFloatStrength);
    }

    void Update()
    {
        // ���ʪ���
        MoveObject();

        // �W�U�}�B
        FloatObject();

        // �ˬd�O�_�ݭn�R���ۨ�
        CheckDeletion();
    }

    void MoveObject()
    {
        // �ϥ� Transform.Translate ��k�Ӳ��ʪ���
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void FloatObject()
    {
        // �ھ� sine �i��ƨӭp��W�U�}�B�������q
        float yOffset = Mathf.Sin(Time.time * moveSpeed) * floatStrength;

        // ��s���󪺦�m�A�K�[�W�U�}�B���ĪG
        transform.position += new Vector3(0, yOffset, 0) * Time.deltaTime;
    }

    void CheckDeletion()
    {
        // �p�G���� X �y�Фj�� 200�A�h�R���ۨ�
        if (transform.position.x > 200)
        {
            Destroy(gameObject);
        }
    }
}
