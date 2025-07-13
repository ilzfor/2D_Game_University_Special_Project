using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float minMoveDistance = 1.0f; // 最小移動距離，可在編輯器中調整
    public float maxMoveDistance = 5.0f; // 最大移動距離，可在編輯器中調整
    public int health = 3; // 怪物的血量
    public bool allowJump = false; // 控制怪物是否可以跳躍
    public float moveSpeed = 1.0f; // 移動速度
    public float jumpHeight = 10f; // 跳躍高度，可在編輯器中調整

    private Rigidbody2D rb;
    private Animator animator; // 引入Animator
    private SpriteRenderer monsterRenderer; // 怪物的Sprite Renderer

    private bool isMoving = false; // 怪物是否正在移動
    private Vector2 moveDirection; // 移動方向
    private float knockbackDistance = 1.0f; // 推擠距離
    private float knockbackJumpHeight = 15f; // 推擠時的小跳高度
    private bool isFlashing = false; // 是否正在閃爍

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        monsterRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // 獲取Animator組件

        // 開始隨機移動協程
        StartCoroutine(RandomMoveCoroutine());
    }

    public void TakeDamage(int damage, Vector2 attackDirection = default(Vector2))
    {
        if (!isFlashing)
        {
            health -= damage; // 減少怪物的血量

            if (health <= 0)
            {
                // 怪物死亡的處理
                Destroy(gameObject);
            }
            else
            {
                // 開始閃爍效果
                StartCoroutine(FlashCoroutine());

                // 計算推擠的目標位置
                Vector2 knockbackTarget = (Vector2)transform.position + attackDirection.normalized * knockbackDistance;

                // 啟動推擠協程
                StartCoroutine(KnockbackCoroutine(knockbackTarget));
            }
        }
        Debug.Log("怪物被擊中"); // 在日誌上輸出訊號
    }

    private IEnumerator KnockbackCoroutine(Vector2 targetPosition)
    {
        Vector2 originalPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < 0.1f)
        {
            // 將怪物向目標位置移動
            transform.position = Vector2.Lerp(originalPosition, targetPosition, elapsedTime / 0.1f);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 在擊退方向添加小跳高度
        rb.velocity = new Vector2(rb.velocity.x, knockbackJumpHeight);
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        // 開始閃爍
        for (int i = 0; i < 3; i++)
        {
            // 使Sprite Renderer顏色設置為紅色（你可以根據需要更改顏色）
            monsterRenderer.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            // 還原Sprite Renderer顏色
            monsterRenderer.color = Color.white;

            yield return new WaitForSeconds(0.1f);
        }

        isFlashing = false;
    }

    private void Update()
    {
        if (isMoving)
        {
            // 根據移動方向和速度移動怪物
            transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

            // 根據移動方向設置 SpriteRenderer 的 flipX 屬性
            if (moveDirection.x > 0) // 如果向右移動
            {
                monsterRenderer.flipX = true; // 不翻轉
            }
            else if (moveDirection.x < 0) // 如果向左移動
            {
                monsterRenderer.flipX = false; // 翻轉
            }

            // 啟動動畫控制器
            animator.enabled = true;
        }
        else
        {
            // 停止動畫控制器
            animator.enabled = false;
        }
    }

    private IEnumerator RandomMoveCoroutine()
    {
        while (true)
        {
            // 隨機選擇左右方向
            float randomX = Random.Range(-1.0f, 1.0f);
            moveDirection = new Vector2(randomX, 0f).normalized; // 將移動方向標準化

            // 隨機選擇移動距離
            float moveDistance = Random.Range(minMoveDistance, maxMoveDistance);

            // 計算移動時間，使得移動距離在設置的速度下完成
            float moveTime = moveDistance / moveSpeed;

            isMoving = true;

            // 如果允許跳躍，則觸發移動和跳躍
            if (allowJump)
            {
                Jump();
            }

            // 等待一段時間後停止移動
            yield return new WaitForSeconds(moveTime);

            isMoving = false;

            // 等待一段時間後重新開始隨機移動
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰撞到標籤為"怪物"的物體時，往反方向移動
        if (collision.gameObject.CompareTag("怪物"))
        {
            moveDirection *= -1;
        }
    }

    private void Jump()
    {
        // 設定跳躍的垂直速度
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, jumpHeight);
    }
}
