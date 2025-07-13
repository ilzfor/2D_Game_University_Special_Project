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
    [Header("������r")]
    public Text textcoin;
    [Tooltip("����")]
    public int coin;
    [Tooltip("�}���q")]
    public float HP = 500;
    [Tooltip("���D����")]
    public AudioClip sounjump;
    [Tooltip("��������")]
    public AudioClip sounat;
    [Tooltip("�Ʀ歵��")]
    public AudioClip sounhy;
    [Tooltip("�I������")]
    public AudioClip soungethit;
    [Tooltip("��������")]
    public AudioClip soungetcoin;
    [Tooltip("���`�T�{")]
    public bool dead;
    [Tooltip("���ʳt��")]
    public float speed = 10.0f;
    [Tooltip("���D�t��")]
    public float jumpForce = 10.0f;

    private bool isInvincible = false; // �O�_�B��L�Ī��A

    [Header("�ʵe���")]
    public Animator ani;
    private bool isGround = true; // �b�}�l�ɰ��w����b�a�O�W
    [Header("���")]
    public UnityEngine.UI.Image imghp;
    private float hpMax;
    [Header("����")]
    public AudioSource and;
    [Header("�C������")]
    public GameObject final;
    [Header("���D")]
    public Text textTitle;
    [Header("���������ƶq")]
    public Text textCurrent;


    private bool facingRight = true;
    private bool isJumping = false; // �s�W���D���A�����L�Ѽ�
    private float horizontalInput; // �x�s������J����

    private bool isAttacking = false; // �������A�����L�Ѽ�
    private bool isAttackCooldown = false; // �����N�o���A�����L�Ѽ�

    private int maxJumpCount = 1; // �̤j���D����
    private int jumpCount = 0; // �l�ܸ��D����
    [Header("����")]
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent <SpriteRenderer>();
        hpMax = HP;
        jumpCount = 0; // ��l�Ƹ��D���Ƭ�0
    }

    private void hit()
    {
        if (HP <= 0) Dead();

        if (!isAttacking)
        {
            if (!isInvincible) // �p�G���O�L�Ī��A�A�~�|����
            {
                HP -= 50;
                imghp.fillAmount = HP / hpMax;
                //and.PlayOneShot(soungethit);

                StartCoroutine(InvincibleCoroutine());
                // �ҰʵL�Ĩ�{�A�]�m�L�Ī��A
                StartCoroutine(FlashCoroutine()); // �Ұʰ{�{��{�A��{�{�{�ĪG

            }
               

        }
        
    }
    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f); // �L�Įɶ���1��
        isInvincible = false;
    }

    private IEnumerator FlashCoroutine()
    {
        float duration = 1f; // �{�{�ɶ���1��
        float blinkTime = 0.1f; // �{�{�W�v
        float elapsedTime = 0f;
        Color originalColor = sr.color; // �x�s�����l���C��

        while (elapsedTime < duration)
        {
            // �]�m����z���׬�0.5
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f); // �]�m�z���׬�0.5�A�z�i�H�ھڻݭn�վ�o�ӭ�
            yield return new WaitForSeconds(blinkTime);

            // ��_��l�z����
            sr.color = originalColor;
            yield return new WaitForSeconds(blinkTime);

            elapsedTime += blinkTime * 2; // �]���⦸�{�{�A�ҥH�ɶ��n���H2
        }

        sr.color = originalColor; // �T�O�̲ר�����ܥ��`
    }

    private void pass()
    {
        final.SetActive(true);
        textTitle.text = "���߳q��";
        speed = 0;
        rb.velocity = Vector3.zero;
        // ����Ǫ����
        MonsterController[] monsters = FindObjectsOfType<MonsterController>();
        foreach (MonsterController monster in monsters)
        {
            monster.enabled = false;
        }
        ani.SetBool("�]�B", false);
        ani.SetBool("����", true);
        // ����o�Ӹ}��
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
        //ani.SetTrigger("���`Ĳ�o");
        // ��w�b���߰ʵe
        ani.SetBool("�]�B", false);
        ani.SetBool("����", true);
        // ����Ǫ����
        MonsterController[] monsters = FindObjectsOfType<MonsterController>();
        foreach (MonsterController monster in monsters)
        {
            monster.enabled = false;
        }

        // ����o�Ӹ}��
        enabled = false;

        speed = 0;
        rb.velocity = Vector3.zero;
    }

    private void getcoin(Collider2D collision)
    {
        coin++;
        Destroy(collision.gameObject);
        textcoin.text = "����:" + coin;
        and.PlayOneShot(soungetcoin);
        HP += 10;
        imghp.fillAmount = HP / hpMax;
    }

    //private int attackDamage = 1; // ���⪺�����O

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

        // �ˬd�O�_�b�������A�U
        if (isAttacking)
        {
            horizontalInput = 0; // �b������������k�첾
        }

        // �ˬd�O�_�b�a���W�Φ����D����
        if (isGround || jumpCount > 0)
        {
            if (horizontal != 0)
            {
                ani.SetBool("�]�B", true);
                ani.SetBool("����", false);
            }
            else
            {
                ani.SetBool("�]�B", false);
                ani.SetBool("����", true);
            }
        }
        else
        {
            // �b�Ť��ɤ�����]�B�ʵe
            ani.SetBool("�]�B", false);
            ani.SetBool("����", false);
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
            StartAttack(); // �}�l����
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
        if (isJumping == false) // �ˬd���D���ƬO�_�j��0
        {
            ani.SetTrigger("���D"); // ������D�ʵe
            isJumping = true; // �]�w���D���A�� true
            
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //and.PlayOneShot(sounjump); // ������D����

            jumpCount--; // ��ָ��D����
            
            StartCoroutine(ResetJumpAnimation()); // �I�s��k���m���D�ʵe
        }
    }
    private IEnumerator ResetJumpAnimation()
    {
        yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length); // ���ݸ��D�ʵe���񧹲�
        ani.ResetTrigger("���D"); // ���m���D�ʵeĲ�o
        ani.SetBool("�]�B", horizontalInput != 0); // ���m�]�B�ʵe
        ani.SetBool("����", horizontalInput == 0); // ���m���߰ʵe
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("�a�O"))
        {
            isGround = true;
            isJumping = false;
            jumpCount = maxJumpCount; // ���m���D���Ƭ��̤j��
        }
    }
    private void StartAttack()
    {
        ani.SetTrigger("����");
        isAttacking = true;
        attackHitbox.SetActive(true); // �ҥΧ����P�w�C������
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.5f); // �����w���ɶ���N��
        isAttacking = false;
        attackHitbox.SetActive(false); // �T�Χ����P�w�C������
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "����")
        {
            getcoin(collision);
        }
        if (collision.tag==("�Ǫ�"))
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
                ani.SetBool("�]�B", true);
                ani.SetBool("����", false);
            }
            else
            {
                ani.SetBool("�]�B", false);
                ani.SetBool("����", true);
            }

            ani.SetBool("���D", false);
        }
    }
}
