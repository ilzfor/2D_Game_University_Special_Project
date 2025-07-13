using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float minMoveDistance = 1.0f; // �̤p���ʶZ���A�i�b�s�边���վ�
    public float maxMoveDistance = 5.0f; // �̤j���ʶZ���A�i�b�s�边���վ�
    public int health = 3; // �Ǫ�����q
    public bool allowJump = false; // ����Ǫ��O�_�i�H���D
    public float moveSpeed = 1.0f; // ���ʳt��
    public float jumpHeight = 10f; // ���D���סA�i�b�s�边���վ�

    private Rigidbody2D rb;
    private Animator animator; // �ޤJAnimator
    private SpriteRenderer monsterRenderer; // �Ǫ���Sprite Renderer

    private bool isMoving = false; // �Ǫ��O�_���b����
    private Vector2 moveDirection; // ���ʤ�V
    private float knockbackDistance = 1.0f; // �����Z��
    private float knockbackJumpHeight = 15f; // �����ɪ��p������
    private bool isFlashing = false; // �O�_���b�{�{

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        monsterRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // ���Animator�ե�

        // �}�l�H�����ʨ�{
        StartCoroutine(RandomMoveCoroutine());
    }

    public void TakeDamage(int damage, Vector2 attackDirection = default(Vector2))
    {
        if (!isFlashing)
        {
            health -= damage; // ��֩Ǫ�����q

            if (health <= 0)
            {
                // �Ǫ����`���B�z
                Destroy(gameObject);
            }
            else
            {
                // �}�l�{�{�ĪG
                StartCoroutine(FlashCoroutine());

                // �p��������ؼЦ�m
                Vector2 knockbackTarget = (Vector2)transform.position + attackDirection.normalized * knockbackDistance;

                // �Ұʱ�����{
                StartCoroutine(KnockbackCoroutine(knockbackTarget));
            }
        }
        Debug.Log("�Ǫ��Q����"); // �b��x�W��X�T��
    }

    private IEnumerator KnockbackCoroutine(Vector2 targetPosition)
    {
        Vector2 originalPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < 0.1f)
        {
            // �N�Ǫ��V�ؼЦ�m����
            transform.position = Vector2.Lerp(originalPosition, targetPosition, elapsedTime / 0.1f);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // �b���h��V�K�[�p������
        rb.velocity = new Vector2(rb.velocity.x, knockbackJumpHeight);
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        // �}�l�{�{
        for (int i = 0; i < 3; i++)
        {
            // ��Sprite Renderer�C��]�m������]�A�i�H�ھڻݭn����C��^
            monsterRenderer.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            // �٭�Sprite Renderer�C��
            monsterRenderer.color = Color.white;

            yield return new WaitForSeconds(0.1f);
        }

        isFlashing = false;
    }

    private void Update()
    {
        if (isMoving)
        {
            // �ھڲ��ʤ�V�M�t�ײ��ʩǪ�
            transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

            // �ھڲ��ʤ�V�]�m SpriteRenderer �� flipX �ݩ�
            if (moveDirection.x > 0) // �p�G�V�k����
            {
                monsterRenderer.flipX = true; // ��½��
            }
            else if (moveDirection.x < 0) // �p�G�V������
            {
                monsterRenderer.flipX = false; // ½��
            }

            // �Ұʰʵe���
            animator.enabled = true;
        }
        else
        {
            // ����ʵe���
            animator.enabled = false;
        }
    }

    private IEnumerator RandomMoveCoroutine()
    {
        while (true)
        {
            // �H����ܥ��k��V
            float randomX = Random.Range(-1.0f, 1.0f);
            moveDirection = new Vector2(randomX, 0f).normalized; // �N���ʤ�V�зǤ�

            // �H����ܲ��ʶZ��
            float moveDistance = Random.Range(minMoveDistance, maxMoveDistance);

            // �p�Ⲿ�ʮɶ��A�ϱo���ʶZ���b�]�m���t�פU����
            float moveTime = moveDistance / moveSpeed;

            isMoving = true;

            // �p�G���\���D�A�hĲ�o���ʩM���D
            if (allowJump)
            {
                Jump();
            }

            // ���ݤ@�q�ɶ��ᰱ���
            yield return new WaitForSeconds(moveTime);

            isMoving = false;

            // ���ݤ@�q�ɶ��᭫�s�}�l�H������
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �I������Ҭ�"�Ǫ�"������ɡA���Ϥ�V����
        if (collision.gameObject.CompareTag("�Ǫ�"))
        {
            moveDirection *= -1;
        }
    }

    private void Jump()
    {
        // �]�w���D�������t��
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, jumpHeight);
    }
}
