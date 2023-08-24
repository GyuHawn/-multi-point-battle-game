using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMove : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool moveForward = true;

    public float moveDistance = 10f;
    public float moveSpeed = 200f;

    void Start()
    {
        // �ʱ� ��ġ ����
        initialPosition = transform.position;
    }

    void Update()
    {
        if (moveForward)
        {
            // ������ �̵�
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // �̵� �Ÿ� Ȯ���Ͽ� ó�� ��ġ�� ���ư�
            if (transform.position.z >= initialPosition.z + moveDistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, initialPosition.z + moveDistance);
                moveForward = false; // �ڷ� �̵��ϱ� ���� ���� ����
            }
        }
        else
        {
            // �ڷ� �̵�
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

            // �̵� �Ÿ� Ȯ���Ͽ� ó�� ��ġ�� ���ư�
            if (transform.position.z <= initialPosition.z - moveDistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, initialPosition.z - moveDistance);
                moveForward = true; // ������ �̵��ϱ� ���� ���� ����
            }
        }

        // X�� Y ���� ������ ���� (������ �κ�)
        var newPosition = new Vector3(initialPosition.x, initialPosition.y, transform.position.z);
        transform.SetPositionAndRotation(newPosition, Quaternion.identity);
    }
}
