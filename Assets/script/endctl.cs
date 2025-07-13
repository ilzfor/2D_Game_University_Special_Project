using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endctl : MonoBehaviour
{
    public Transform rotationTarget; // 要圍繞的目標物件

    void Update()
    {
        // 檢查 rotationTarget 是否存在，如果不存在則銷毀物體
        if (rotationTarget == null || !rotationTarget.gameObject.activeSelf)
        {
            Destroy(gameObject);
            return;
        }
    }
    }
