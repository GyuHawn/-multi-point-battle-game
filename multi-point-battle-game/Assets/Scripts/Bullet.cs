using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �̵��ӵ�
    public int damage = 10;

    void Start()
    {
        // 5�� �Ŀ� �ڽ��� �ı��Ѵ�.
        Destroy(gameObject, 1f);
    }
}
