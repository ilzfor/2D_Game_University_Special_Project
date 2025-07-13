using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESctl : MonoBehaviour
{
    public float rotationSpeed = 50.0f; // 旋轉速度

    // 更新每幀呼叫的函式
    void Update()
    {
        // 根據旋轉速度在 Z 軸上旋轉物件
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
