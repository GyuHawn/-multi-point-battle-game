using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Look : MonoBehaviourPunCallbacks
{
    public static bool cursorLocked = true; // 마우스 시각화

    public Transform player; // 플레이어 변수
    public Transform cams; // 카메라 변수
    public Transform weapon; // 총 변수
    public float xSensitivity; // x축 입력값
    public float ySensitivity;  // y축 입력값
    public float maxAngle; // 최대 앵글값

    private Quaternion camCenter; // 카메라의 중심

    void Start()
    {
        camCenter = cams.localRotation; // 카메라의 회전값 할당
    }

   
    void Update()
    {
        if (!photonView.IsMine) return; // 자신인지 확인
        SetY();
        SetX();
        UpdateCursorLock();
    }

    void SetY()
    {
        float t_input = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime; // 마우스 y축 입력값을 받아 저장
        Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right); // t_input 각도 만큼 x축 회전
        Quaternion t_delta = cams.localRotation * t_adj; // 카메라 로컬 회전값을 t_adj와 곱하여 변수저장
        if (Quaternion.Angle(camCenter,t_delta) < maxAngle) // t_delta이 카메라의 중심에서 회전한 값이 최대 앵글값보다 작을때
        {
            cams.localRotation = t_delta; // 회전값 설정
           
        }
        weapon.rotation = cams.rotation; // 회전값 설정

    }
    void SetX() // SetY와 동일
    {
        float t_input = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime; 
        Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);
        Quaternion t_delta = player.localRotation * t_adj;
        player.localRotation = t_delta;
        

    }

    void UpdateCursorLock()
    {
        if(cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
            Cursor.visible = false; // 커서 비시각화

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = false; // 커서 잠금 해제
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; // 커서 잠금 해제
            Cursor.visible = true; // 커서 시각화
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = true; // 커서 잠금
            }
        }
    }
}