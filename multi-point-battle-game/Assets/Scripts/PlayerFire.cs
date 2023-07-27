using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 총알 생성 위치
    public GameObject bulletObj;

    // 총구
    public GameObject firePosition;

    // 총알 이동 속도
    public float bulletSpeed = 10f;

    void Update()
    {
        // 버튼 입력시 발사
        if (Input.GetButtonDown("Fire1"))
        {
            // 총알 생성
            GameObject bullet = Instantiate(bulletObj);

            // 카메라가 보는 방향의 앞으로 발사되도록 수정
            bullet.transform.forward = Camera.main.transform.forward;

            // 총알 발사 시, 속도를 추가해줌.
            Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
            rigidbody.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);

            // 총알 발사
            bullet.transform.position = firePosition.transform.position;
        }
    }
}
