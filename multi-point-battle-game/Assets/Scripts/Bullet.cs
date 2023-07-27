using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 이동속도
    public int damage = 10;

    void Start()
    {
        // 5초 후에 자신을 파괴한다.
        Destroy(gameObject, 1f);
    }
}
