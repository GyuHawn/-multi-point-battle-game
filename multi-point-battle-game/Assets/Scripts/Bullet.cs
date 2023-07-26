using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 이동속도
    public float spd = 5;
    public int damage = 10;

    void Start()
    {
        // 5초 후에 자신을 파괴한다.
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        // 방향을 구한다
        Vector3 dir = Vector3.forward;

        // 이동
        transform.position += dir * spd * Time.deltaTime;
    }
}
