using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float rotationAmount = 0.1f; // ���઺���׶q

    private void OnEnable()
    {
        // �b�ҥθI�����ɶi��L�p����
        transform.Rotate(Vector3.forward, rotationAmount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("�Ǫ�"))
        {
            MonsterController monster = collision.GetComponent<MonsterController>();
            if (monster != null)
            {
                Vector2 attackDirection = (collision.transform.position - transform.position).normalized;
                monster.TakeDamage(1, attackDirection); // �ǻ��A���ˮ`�ȩM������V
            }
        }
    }
}