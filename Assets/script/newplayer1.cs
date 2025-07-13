using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class newplayer1 : MonoBehaviour
{
    public LayerMask groundLayer;
    public GameObject attackHitbox;
    [Header("金幣文字")]
    public Text textcoin;
    [Tooltip("金幣")]
    public int coin;
    [Tooltip("腳色血量")]
    public float HP = 500;
    [Tooltip("跳躍音效")]
    public AudioClip sounjump;
    [Tooltip("攻擊音效")]
    public AudioClip sounat;
    [Tooltip("滑行音效")]
    public AudioClip sounhy;
    [Tooltip("碰撞音效")]
    public AudioClip soungethit;
    [Tooltip("金幣音效")]
    public AudioClip soungetcoin;
    [Tooltip("死亡確認")]
    public bool dead;
    [Tooltip("移動速度")]
    public float speed = 10.0f;
    [Tooltip("跳躍速度")]
    public float jumpForce = 10.0f;

    private bool isInvincible = false; // 是否處於無敵狀態

    [Header("動畫控制器")]
    public Animator ani;
    private bool isGround = true; // 在開始時假定角色在地板上
    [Header("血條")]
    public UnityEngine.UI.Image imghp;
    private float hpMax;
    [Header("音效")]
    public AudioSource and;
    [Header("遊戲結束")]
    public GameObject final;
    [Header("標題")]
    public Text textTitle;
    [Header("本次金幣數量")]
    public Text textCurrent;


    private bool facingRight = true;
    private bool isJumping = false; // 新增跳躍狀態的布林參數
    private float horizontalInput; // 儲存水平輸入的值

    private bool isAttacking = false; // 攻擊狀態的布林參數
    private bool isAttackCooldown = false; // 攻擊冷卻狀態的布林參數

    private int maxJumpCount = 1; // 最大跳躍次數
    private int jumpCount = 0; // 追蹤跳躍次數
    [Header("剛體")]
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent <SpriteRenderer>();
        hpMax = HP;
        jumpCount = 0; // 初始化跳躍次數為0
    }

    private void hit()
    {
        if (HP <= 0) Dead();

        if (!isAttacking)
        {
            if (!isInvincible) // 如果不是無敵狀態，才會受傷
            {
                HP -= 50;
                imghp.fillAmount = HP / hpMax;
                //and.PlayOneShot(soungethit);

                StartCoroutine(InvincibleCoroutine());
                // 啟動無敵協程，設置無敵狀態
                StartCoroutine(FlashCoroutine()); // 啟動閃爍協程，實現閃爍效果

            }
               

        }
        
    }
    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f); // 無敵時間為1秒
        isInvincible = false;
    }

    private IEnumerator FlashCoroutine()
    {
        float duration = 1f; // 閃爍時間為1秒
        float blinkTime = 0.1f; // 閃爍頻率
        float elapsedTime = 0f;
        Color originalColor = sr.color; // 儲存角色原始的顏色

        while (elapsedTime < duration)
        {
            // 設置角色透明度為0.5
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f); // 設置透明度為0.5，您可以根據需要調整這個值
            yield return new WaitForSeconds(blinkTime);

            // 恢復原始透明度
            sr.color = originalColor;
            yield return new WaitForSeconds(blinkTime);

            elapsedTime += blinkTime * 2; // 因為兩次閃爍，所以時間要乘以2
        }

        sr.color = originalColor; // 確保最終角色顯示正常
    }

    private void pass()
    {
        final.SetActive(true);
        textTitle.text = "恭喜通關";
        speed = 0;
        rb.velocity = Vector3.zero;
        // 停止怪物控制器
        MonsterController[] monsters = FindObjectsOfType<MonsterController>();
        foreach (MonsterController monster in monsters)
        {
            monster.enabled = false;
        }
        ani.SetBool("跑步", false);
        ani.SetBool("站立", true);
        // 停止這個腳本
        enabled = false;
    }

    private void Dead()
    {
        if (dead) return;

        HP = 0;
        imghp.fillAmount = HP / hpMax;
        final.SetActive(true);
        textTitle.text = "YOU DIED";
        speed = 0;
        dead = true;
        //ani.SetTrigger("死亡觸發");
        // 鎖定在站立動畫
        ani.SetBool("跑步", false);
        ani.SetBool("站立", true);
        // 停止怪物控制器
        MonsterController[] monsters = FindObjectsOfType<MonsterController>();
        foreach (MonsterController monster in monsters)
        {
            monster.enabled = false;
        }

        // 停止這個腳本
        enabled = false;

        speed = 0;
        rb.velocity = Vector3.zero;
    }

    private void getcoin(Collider2D collision)
    {
        coin++;
        Destroy(collision.gameObject);
        textcoin.text = "金幣:" + coin;
        and.PlayOneShot(soungetcoin);
        HP += 10;
        imghp.fillAmount = HP / hpMax;
    }

    //private int attackDamage = 1; // 角色的攻擊力

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1;
        }

        // 檢查是否在攻擊狀態下
        if (isAttacking)
        {
            horizontalInput = 0; // 在攻擊期間停止左右位移
        }

        // 檢查是否在地面上或有跳躍次數
        if (isGround || jumpCount > 0)
        {
            if (horizontal != 0)
            {
                ani.SetBool("跑步", true);
                ani.SetBool("站立", false);
            }
            else
            {
                ani.SetBool("跑步", false);
                ani.SetBool("站立", true);
            }
        }
        else
        {
            // 在空中時不執行跑步動畫
            ani.SetBool("跑步", false);
            ani.SetBool("站立", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else
        {
            vertical = 0;
        }

        if (Input.GetKeyDown(KeyCode.J) && !isAttacking && !isAttackCooldown)
        {
            StartAttack(); // 開始攻擊
        }

        if (horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip();
        }

        if (dead) return;
        if (transform.position.y <= -7) Dead();
    }

    private void Jump()
    {
        if (isJumping == false) // 檢查跳躍次數是否大於0
        {
            ani.SetTrigger("跳躍"); // 播放跳躍動畫
            isJumping = true; // 設定跳躍狀態為 true
            
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //and.PlayOneShot(sounjump); // 播放跳躍音效

            jumpCount--; // 減少跳躍次數
            
            StartCoroutine(ResetJumpAnimation()); // 呼叫方法重置跳躍動畫
        }
    }
    private IEnumerator ResetJumpAnimation()
    {
        yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length); // 等待跳躍動畫播放完畢
        ani.ResetTrigger("跳躍"); // 重置跳躍動畫觸發
        ani.SetBool("跑步", horizontalInput != 0); // 重置跑步動畫
        ani.SetBool("站立", horizontalInput == 0); // 重置站立動畫
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("地板"))
        {
            isGround = true;
            isJumping = false;
            jumpCount = maxJumpCount; // 重置跳躍次數為最大值
        }
    }
    private void StartAttack()
    {
        ani.SetTrigger("攻擊");
        isAttacking = true;
        attackHitbox.SetActive(true); // 啟用攻擊判定遊戲物件
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.5f); // 攻擊硬直時間為N秒
        isAttacking = false;
        attackHitbox.SetActive(false); // 禁用攻擊判定遊戲物件
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "金幣")
        {
            getcoin(collision);
        }
        if (collision.tag==("怪物"))
        {
            hit();
            
        }
        if (collision.name == "ENDPOIN")
        {
            pass();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void FixedUpdate()
    {
        if (HP <= 0)
        {
            Dead();
        }

        Vector2 movement = new Vector2(horizontalInput * speed, rb.velocity.y);
        rb.velocity = movement;

        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }

        if (!isGround && rb.velocity.y <= 0.1f)
        {
            if (horizontalInput != 0)
            {
                ani.SetBool("跑步", true);
                ani.SetBool("站立", false);
            }
            else
            {
                ani.SetBool("跑步", false);
                ani.SetBool("站立", true);
            }

            ani.SetBool("跳躍", false);
        }
    }
}
