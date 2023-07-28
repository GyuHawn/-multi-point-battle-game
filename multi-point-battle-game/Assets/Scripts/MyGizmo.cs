using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    // 플레이어 생성 위치 설정
    public Color mcolor = Color.yellow;
    public float mradius = 0.3f;

    void OnDrawGizmos()
    {
        // 기즈모 색상 설정
        Gizmos.color = mcolor;
        // 구 형태의 기즈모 생성
        Gizmos.DrawSphere(transform.position, mradius); // (위치, 크기)
    }
}
