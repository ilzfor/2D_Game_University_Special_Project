using UnityEngine;

public class TreeSwing : MonoBehaviour
{
    public float swingSpeed = 1.0f; // �n�\�t��
    public float swingRange = 10.0f; // �n�\�T��

    private Transform topOfTree; // �𳻪�Transform�ե�

    void Start()
    {
        // ���𳻪�Transform�ե�
        topOfTree = transform.Find("TopOfTree");
        if (topOfTree == null)
        {
            Debug.LogError("TopOfTree not found. Please create a child object named 'TopOfTree' for the top part of the tree.");
            return;
        }
    }

    void Update()
    {
        if (topOfTree == null)
        {
            return;
        }

        // �ϥ� sin ��ƭp��n�\�������q
        float offset = Mathf.Sin(Time.time * swingSpeed) * swingRange;

        // �N�����q���Ψ�𳻪� X �b��m
        Vector3 newPosition = topOfTree.position + new Vector3(offset, 0, 0);
        topOfTree.position = newPosition;
    }
}
