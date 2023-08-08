using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sway : MonoBehaviour
{
    public float intensity; // 좌우 모션 강도
    public float smooth; // 위아래(반동) 모션 강도
    private Quaternion origin_rotation; // 총의 기본 회전값
   //public bool isMine; // 현재 총을 가지고 있는지
   

    private void Start()
    {    
        origin_rotation = transform.localRotation; // 총의 초기 회전값 저장
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        float t_x_mouse = Input.GetAxis("Mouse X"); // 마우스의 현재 x,y축의 변화량
        float t_y_mouse = Input.GetAxis("Mouse Y");

        /*if(isMine) // 일단 보류
        {
            t_x_mouse = 0;
            t_y_mouse = 0;
        }*/

        // 총의 회전값 계산
        Quaternion t_x_adj = Quaternion.AngleAxis(-intensity * t_x_mouse, Vector3.up); // 마우스의 x,y축 이동량 
        Quaternion t_y_adj = Quaternion.AngleAxis(intensity * t_y_mouse, Vector3.right); // Quaternion.AngleAxis() - 회전 값 계산
        Quaternion target_rotation = origin_rotation * t_x_adj * t_y_adj; // 새로운 방향 계산

        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);
        // 총의 움직임 구현 
    }
}
