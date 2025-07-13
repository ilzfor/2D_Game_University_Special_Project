using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间

public class Godctl : MonoBehaviour
{
    public GameObject targetObject; // 指定的碰觸物件
    public List<GameObject> targetPositionObjects; // 目標位置的物件列表
    public int requiredTouches = 3; // 需要的碰觸次數
    public float blinkDuration = 0.5f; // 閃爍時間
    public float cooldownDuration = 1.0f; // 冷卻時間
    public float teleportDelay = 10.0f; // 瞬移的倒數秒數
    public Image fillImage; // UI 圖片對象

    private int currentTouches = 0; // 當前的碰觸次數
    private bool isBlinking = false; // 是否在閃爍
    private bool isCoolingDown = false; // 是否在冷卻
    private bool isTeleporting = false; // 是否在瞬移
    private int currentTargetIndex = 0; // 當前目標索引

    void Start()
    {
        // 開始倒數計時
        StartCoroutine(TeleportCountdown());
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == targetObject && !isBlinking && !isCoolingDown)
        {
            StartCoroutine(HandleBlink());
        }
    }

    private IEnumerator HandleBlink()
    {
        isBlinking = true;
        currentTouches++;

        // 閃爍效果
        for (float i = 0; i < blinkDuration; i += 0.1f)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<SpriteRenderer>().enabled = true;

        // 冷卻時間
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCoolingDown = false;

        isBlinking = false;

        // 判斷是否達到碰觸次數
        if (currentTouches >= requiredTouches)
        {
            Destroy(gameObject);
        }

        // 更新 UI 圖片填充比例
        UpdateFillImage();
    }

    private void UpdateFillImage()
    {
        if (fillImage != null)
        {
            float fillAmount = 1 - (float)currentTouches / requiredTouches; // 計算填充比例並取反
            fillImage.fillAmount = fillAmount;
        }
    }


    private IEnumerator TeleportCountdown()
    {
        while (true) // 使用无限循环确保持续执行
        {
            while (currentTargetIndex < targetPositionObjects.Count)
            {
                yield return new WaitForSeconds(teleportDelay);
                StartCoroutine(TeleportToTarget());
            }

            // 当达到列表末尾时，重置当前目标索引为0
            currentTargetIndex = 0;
        }
    }


    private IEnumerator TeleportToTarget()
    {
        isTeleporting = true;

        if (currentTargetIndex < targetPositionObjects.Count && targetPositionObjects[currentTargetIndex] != null)
        {
            // 瞬間移動到目標位置
            transform.position = targetPositionObjects[currentTargetIndex].transform.position;
            currentTargetIndex++;
        }

        isTeleporting = false;
        yield return null;
    }
}
