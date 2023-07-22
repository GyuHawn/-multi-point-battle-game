using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private CinemachineVirtualCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = player.transform; // Follow �׸� ����
        vcam.LookAt = player.transform; // LookAt �׸� ����
    }

    void LateUpdate()
    {
        // �÷��̾��� ȸ������ �����ͼ� ī�޶��� z�� ȸ������ �ݿ��մϴ�.
        float playerYRot = player.transform.eulerAngles.y;
        vcam.transform.eulerAngles = new Vector3(vcam.transform.eulerAngles.x, playerYRot, vcam.transform.eulerAngles.z);
    }
}
