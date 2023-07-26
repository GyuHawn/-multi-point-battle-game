using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �̵��ӵ�
    public float spd = 5;
    public int damage = 10;

    void Start()
    {
        // 5�� �Ŀ� �ڽ��� �ı��Ѵ�.
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        // ������ ���Ѵ�
        Vector3 dir = Vector3.forward;

        // �̵�
        transform.position += dir * spd * Time.deltaTime;
    }
}
