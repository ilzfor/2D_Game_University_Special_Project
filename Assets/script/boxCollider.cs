using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxCollider : MonoBehaviour
{
    public Collider2D playerCollider; // 主角的碰撞器

    private void Start()
    {
        // 忽略碰撞器與主角碰撞器之間的碰撞
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider);
    }
}


