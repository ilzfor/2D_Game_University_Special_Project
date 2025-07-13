using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garbagectrl : MonoBehaviour
{
    public float minMoveSpeed = 2.0f; // 移動速度的最小值
    public float maxMoveSpeed = 5.0f; // 移動速度的最大值
    public float minFloatStrength = 0.3f; // 上下漂浮的最小值
    public float maxFloatStrength = 0.8f; // 上下漂浮的最大值

    private float moveSpeed;
    private float floatStrength;
    private Vector2 moveDirection = Vector2.right; // 預設向右移動

    void Start()
    {
        // 在指定的區間內隨機產生移動速度和漂浮強度
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        floatStrength = Random.Range(minFloatStrength, maxFloatStrength);
    }

    void Update()
    {
        // 移動物件
        MoveObject();

        // 上下漂浮
        FloatObject();

        // 檢查是否需要刪除自身
        CheckDeletion();
    }

    void MoveObject()
    {
        // 使用 Transform.Translate 方法來移動物件
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void FloatObject()
    {
        // 根據 sine 波函數來計算上下漂浮的偏移量
        float yOffset = Mathf.Sin(Time.time * moveSpeed) * floatStrength;

        // 更新物件的位置，添加上下漂浮的效果
        transform.position += new Vector3(0, yOffset, 0) * Time.deltaTime;
    }

    void CheckDeletion()
    {
        // 如果物件的 X 座標大於 200，則刪除自身
        if (transform.position.x > 200)
        {
            Destroy(gameObject);
        }
    }
}
