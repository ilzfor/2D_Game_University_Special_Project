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
        //�˴��O�_�����N��Q���U
        if (Input.anyKeyDown)
        {
            // �����ثe�C����H
            gameObject.SetActive(false);
        }
    }
}
