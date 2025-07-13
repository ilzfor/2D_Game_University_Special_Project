using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float rotationAmount = 0.1f; // 旋轉的角度量

    private void OnEnable()
    {
        // 在啟用碰撞器時進行微小旋轉
        transform.Rotate(Vector3.forward, rotationAmount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("怪物"))
        {
            MonsterController monster = collision.GetComponent<MonsterController>();
            if (monster != null)
            {
                Vector2 attackDirection = (collision.transform.position - transform.position).normalized;
                monster.TakeDamage(1, attackDirection); // 傳遞適當的傷害值和攻擊方向
            }
        }
    }
}