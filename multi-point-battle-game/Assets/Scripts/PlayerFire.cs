using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 총알 생성 위치
    public GameObject bulletObj;

    // 총구
    public GameObject firePosition;
    
    void Update()
    {
        // 버튼 입력시 발사
        if (Input.GetButtonDown("Fire1"))
        {
            // 총알 생성
            GameObject bullet = Instantiate(bulletObj);

            // 총알 발사
            bullet.transform.position = firePosition.transform.position;
        }
    }
}
