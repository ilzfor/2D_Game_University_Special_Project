using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class godhandctl : MonoBehaviour
{
    public Transform rotationTarget; // �n��¶���ؼЪ���
    public Transform lookAtTarget; // �n�¦V���ؼЪ���
    public float minRadius = 3f; // �̤p����¶�b�|
    public float maxRadius = 7f; // �̤j����¶�b�|
    public float rotationSpeed = 50f; // ��¶�ؼб��઺�t��
    public float shrinkSpeed = 1f; // ���Y�t��
    public float smoothTime = 0.1f; // ���Ʋ��ʪ��ɶ�

    private float currentRadius; // ��e����¶�b�|
    private float angle; // ��e�����ਤ��
    private Vector3 currentVelocity; // �Ω󥭷Ʋ��ʪ��t�װѼ�

    // Start �O�b�Ĥ@�V��s���e�եΪ����
    void Start()
    {
        if (rotationTarget != null)
        {
            // �p���l����
            Vector3 direction = (transform.position - rotationTarget.position).normalized;
            angle = Mathf.Atan2(direction.y, direction.x);
        }
        else
        {
            Debug.LogError("�Ы��w�@�ӳ�¶���ؼЪ���");
        }

        if (lookAtTarget == null)
        {
            Debug.LogError("�Ы��w�@�Ӵ¦V���ؼЪ���");
        }

        // ��l�Ƴ�¶�b�|���̤p�b�|
        currentRadius = minRadius;
    }

    // Update �O�C�V�եΪ����
    void Update()
    {
        // �ˬd rotationTarget �O�_�s�b�A�p�G���s�b�h�P������
        if (rotationTarget == null || !rotationTarget.gameObject.activeSelf)
        {
            Destroy(gameObject);
            return;
        }

        if (rotationTarget != null && lookAtTarget != null)
        {
            // ��s����
            angle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;

            // �p��s����¶�b�|
            currentRadius = Mathf.Lerp(currentRadius, minRadius, Time.deltaTime * shrinkSpeed);

            // �p��s���ؼЦ�m
            float targetX = rotationTarget.position.x + Mathf.Cos(angle) * currentRadius;
            float targetY = rotationTarget.position.y + Mathf.Sin(angle) * currentRadius;
            Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z);

            // ���Ʋ��ʨ�s��m
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);

            // �p����V�¦V�ؼЪ���V�V�q
            Vector3 directionToLookAt = lookAtTarget.position - transform.position;

            // �p�⪫�����Ӵ¦V�����ס]�H�������e��^
            float angleToLookAt = Mathf.Atan2(directionToLookAt.y, directionToLookAt.x) * Mathf.Rad2Deg + 90;

            // �]�m���󪺱���
            transform.rotation = Quaternion.Euler(0, 0, angleToLookAt);
        }
    }
}
