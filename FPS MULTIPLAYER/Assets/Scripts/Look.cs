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
/*
using System.Collections;: 이 코드 줄은 System.Collections 네임스페이스를 참조합니다.
using System.Collections.Generic;: 이 코드 줄은 System.Collections.Generic 네임스페이스를 참조합니다.
using UnityEngine;: 이 코드 줄은 UnityEngine 네임스페이스를 참조합니다.
using Photon.Pun;: 이 코드 줄은 Photon.Pun 네임스페이스를 참조합니다.
public class Look : MonoBehaviourPunCallbacks: 이 코드 줄은 Look 스크립트 클래스를 선언하고 MonoBehaviourPunCallbacks 클래스를 상속합니다.
public static bool cursorLocked = true;: 이 코드 줄은 cursorLocked라는 공용 정적 불리언 변수를 선언하고 true 값을 할당합니다.
public Transform player;: 이 코드 줄은 Transform 형식의 player 변수를 선언합니다.
public Transform cams;: 이 코드 줄은 Transform 형식의 cams 변수를 선언합니다.
public Transform weapon;: 이 코드 줄은 Transform 형식의 weapon 변수를 선언합니다.
public float xSensitivity;: 이 코드 줄은 xSensitivity라는 공용 실수 변수를 선언합니다.
public float ySensitivity;: 이 코드 줄은 ySensitivity라는 공용 실수 변수를 선언합니다.
public float maxAngle;: 이 코드 줄은 maxAngle이라는 공용 실수 변수를 선언합니다.
private Quaternion camCenter;: 이 코드 줄은 camCenter라는 개인적인 쿼터니언 변수를 선언합니다.
void Start(): 이 코드 줄은 Start 메소드를 선언합니다.
camCenter = cams.localRotation;: 이 코드 줄은 camCenter 변수에 카메라의 localRotation 값을 할당합니다.
void Update(): 이 코드 줄은 Update 메소드를 선언합니다.
if (!photonView.IsMine) return;: 이 코드 줄은 현재 PhotonView가 로컬 플레이어의 것인지 확인하고 그렇지 않으면 이후 코드를 실행하지 않도록 합니다.
SetY();: 이 코드 줄은 SetY() 메서드를 호출합니다.
SetX();: 이 코드 줄은 SetX() 메서드를 호출합니다.
UpdateCursorLock();: 이 코드 줄은 UpdateCursorLock() 메서드를 호출합니다.
void SetY(): 이 코드 줄은 SetY() 메서드를 선언합니다.
float t_input = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;: 이 코드 줄은 마우스의 Y축 입력 값을 가져와 변수 t_input에 저장합니다.
Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);: 이 코드 줄은 t_input 값을 사용하여 Quaternion의 t_adj 변수를 계산합니다.
Quaternion t_delta = cams.localRotation * t_adj;: 이 코드 줄은 카메라의 로컬 회전 값을 t_adj와 곱하여 t_delta 변수에 저장합니다.
if (Quaternion.Angle(camCenter, t_delta) < maxAngle): 이 코드 줄은 camCenter를 기준으로 t_delta의 회전이 maxAngle보다 작은 경우에만 다음 코드를 실행합니다.
cams.localRotation = t_delta;: 이 코드 줄은 cams의 로컬 회전 값을 t_delta로 설정합니다.
weapon.rotation = cams.rotation;: 이 코드 줄은 weapon의 회전 값을 cams의 회전 값으로 설정합니다.
void SetX(): 이 코드 줄은 SetX() 메서드를 선언합니다.
float t_input = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;: 이 코드 줄은 마우스의 X축 입력 값을 가져와 변수 t_input에 저장합니다.
Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);: 이 코드 줄은 t_input 값을 사용하여 Quaternion의 t_adj 변수를 계산합니다.
Quaternion t_delta = player.localRotation * t_adj;: 이 코드 줄은 플레이어의 로컬 회전 값을 t_adj와 곱하여 t_delta 변수에 저장합니다.
player.localRotation = t_delta;: 이 코드 줄은 player의 로컬 회전 값을 t_delta로 설정합니다.
void UpdateCursorLock(): 이 코드 줄은 UpdateCursorLock() 메서드를 선언합니다.
if (cursorLocked): 이 코드 줄은 cursorLocked가 true인 경우에만 다음 코드를 실행합니다.
Cursor.lockState = CursorLockMode.Locked;: 이 코드 줄은 커서를 잠금 모드로 설정합니다.
Cursor.visible = false;: 이 코드 줄은 커서를 안 보이게 합니다.
if (Input.GetKeyDown(KeyCode.Escape)): 이 코드 줄은 Escape 키를 눌렀는지 확인합니다.
cursorLocked = false;: 이 코드 줄은 Escape 키를 누르면 cursorLocked 값을 false로 변경합니다.
else: 이 코드 줄은 cursorLocked가 false인 경우에 실행할 코드를 정의합니다.
Cursor.lockState = CursorLockMode.None;: 이 코드 줄은 커서 잠금 모드를 해제합니다.
Cursor.visible = true;: 이 코드 줄은 커서를 보이게 합니다.
if (Input.GetKeyDown(KeyCode.Escape)): 이 코드 줄은 Escape 키를 눌렀는지 확인합니다.
cursorLocked = true;: 이 코드 줄은 Escape 키를 누르면 cursorLocked 값을 true로 변경합니다.
 */