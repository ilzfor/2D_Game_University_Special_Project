using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxCollider : MonoBehaviour
{
    public Collider2D playerCollider; // �D�����I����

    private void Start()
    {
        // �����I�����P�D���I�����������I��
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider);
    }
}


