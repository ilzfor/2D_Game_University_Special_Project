using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //檢測是否有任意鍵被按下
        if (Input.anyKeyDown)
        {
            // 關閉目前遊戲對象
            gameObject.SetActive(false);
        }
    }
}
