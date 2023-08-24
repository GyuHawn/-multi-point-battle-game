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
        // 초기 위치 저장
        initialPosition = transform.position;
    }

    void Update()
    {
        if (moveForward)
        {
            // 앞으로 이동
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // 이동 거리 확인하여 처음 위치로 돌아감
            if (transform.position.z >= initialPosition.z + moveDistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, initialPosition.z + moveDistance);
                moveForward = false; // 뒤로 이동하기 위해 상태 변경
            }
        }
        else
        {
            // 뒤로 이동
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

            // 이동 거리 확인하여 처음 위치로 돌아감
            if (transform.position.z <= initialPosition.z - moveDistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, initialPosition.z - moveDistance);
                moveForward = true; // 앞으로 이동하기 위해 상태 변경
            }
        }

        // X와 Y 축의 움직임 제한 (수정된 부분)
        var newPosition = new Vector3(initialPosition.x, initialPosition.y, transform.position.z);
        transform.SetPositionAndRotation(newPosition, Quaternion.identity);
    }
}
