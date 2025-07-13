using UnityEngine;

public class TreeSwing : MonoBehaviour
{
    public float swingSpeed = 1.0f; // 搖擺速度
    public float swingRange = 10.0f; // 搖擺幅度

    private Transform topOfTree; // 樹頂的Transform組件

    void Start()
    {
        // 找到樹頂的Transform組件
        topOfTree = transform.Find("TopOfTree");
        if (topOfTree == null)
        {
            Debug.LogError("TopOfTree not found. Please create a child object named 'TopOfTree' for the top part of the tree.");
            return;
        }
    }

    void Update()
    {
        if (topOfTree == null)
        {
            return;
        }

        // 使用 sin 函數計算搖擺的偏移量
        float offset = Mathf.Sin(Time.time * swingSpeed) * swingRange;

        // 將偏移量應用到樹頂的 X 軸位置
        Vector3 newPosition = topOfTree.position + new Vector3(offset, 0, 0);
        topOfTree.position = newPosition;
    }
}
