using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    // �÷��̾� ���� ��ġ ����
    public Color mcolor = Color.yellow;
    public float mradius = 0.3f;

    void OnDrawGizmos()
    {
        // ����� ���� ����
        Gizmos.color = mcolor;
        // �� ������ ����� ����
        Gizmos.DrawSphere(transform.position, mradius); // (��ġ, ũ��)
    }
}
