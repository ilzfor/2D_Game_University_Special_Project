using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makegarbage : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // 存放要生成的物件
    public float spawnInterval = 1.0f; // 生成間隔時間
    private float timer = 0.0f;

    void Update()
    {
        // 更新計時器
        timer += Time.deltaTime;

        // 如果計時器超過生成間隔時間，生成物件並重置計時器
        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0.0f;
        }
    }

    void SpawnObject()
    {
        // 確保 objectsToSpawn 不為空且至少包含一個物件
        if (objectsToSpawn != null && objectsToSpawn.Length > 0)
        {
            // 從 objectsToSpawn 中隨機選擇一個物件生成
            int randomIndex = Random.Range(0, objectsToSpawn.Length);
            Instantiate(objectsToSpawn[randomIndex], transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("No objects to spawn. Please assign objects to objectsToSpawn array in the inspector.");
        }
    }
}
