using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private CinemachineVirtualCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = player.transform; // Follow 항목 설정
        vcam.LookAt = player.transform; // LookAt 항목 설정
    }

    void LateUpdate()
    {
        // 플레이어의 회전값을 가져와서 카메라의 z축 회전값에 반영합니다.
        float playerYRot = player.transform.eulerAngles.y;
        vcam.transform.eulerAngles = new Vector3(vcam.transform.eulerAngles.x, playerYRot, vcam.transform.eulerAngles.z);
    }
}
