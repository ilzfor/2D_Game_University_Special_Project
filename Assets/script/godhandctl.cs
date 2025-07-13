using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class godhandctl : MonoBehaviour
{
    public Transform rotationTarget; // 要圍繞的目標物件
    public Transform lookAtTarget; // 要朝向的目標物件
    public float minRadius = 3f; // 最小的圍繞半徑
    public float maxRadius = 7f; // 最大的圍繞半徑
    public float rotationSpeed = 50f; // 圍繞目標旋轉的速度
    public float shrinkSpeed = 1f; // 收縮速度
    public float smoothTime = 0.1f; // 平滑移動的時間

    private float currentRadius; // 當前的圍繞半徑
    private float angle; // 當前的旋轉角度
    private Vector3 currentVelocity; // 用於平滑移動的速度參數

    // Start 是在第一幀更新之前調用的函數
    void Start()
    {
        if (rotationTarget != null)
        {
            // 計算初始角度
            Vector3 direction = (transform.position - rotationTarget.position).normalized;
            angle = Mathf.Atan2(direction.y, direction.x);
        }
        else
        {
            Debug.LogError("請指定一個圍繞的目標物件");
        }

        if (lookAtTarget == null)
        {
            Debug.LogError("請指定一個朝向的目標物件");
        }

        // 初始化圍繞半徑為最小半徑
        currentRadius = minRadius;
    }

    // Update 是每幀調用的函數
    void Update()
    {
        // 檢查 rotationTarget 是否存在，如果不存在則銷毀物體
        if (rotationTarget == null || !rotationTarget.gameObject.activeSelf)
        {
            Destroy(gameObject);
            return;
        }

        if (rotationTarget != null && lookAtTarget != null)
        {
            // 更新角度
            angle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;

            // 計算新的圍繞半徑
            currentRadius = Mathf.Lerp(currentRadius, minRadius, Time.deltaTime * shrinkSpeed);

            // 計算新的目標位置
            float targetX = rotationTarget.position.x + Mathf.Cos(angle) * currentRadius;
            float targetY = rotationTarget.position.y + Mathf.Sin(angle) * currentRadius;
            Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z);

            // 平滑移動到新位置
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);

            // 計算指向朝向目標的方向向量
            Vector3 directionToLookAt = lookAtTarget.position - transform.position;

            // 計算物件應該朝向的角度（以左側為前方）
            float angleToLookAt = Mathf.Atan2(directionToLookAt.y, directionToLookAt.x) * Mathf.Rad2Deg + 90;

            // 設置物件的旋轉
            transform.rotation = Quaternion.Euler(0, 0, angleToLookAt);
        }
    }
}
