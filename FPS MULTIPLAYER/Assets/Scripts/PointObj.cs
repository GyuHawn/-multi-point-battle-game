using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class PointObj : MonoBehaviourPunCallbacks, IPunObservable
{
    PointSpwan pointspwan;

    private string currentScene;

    public float max_health;
    public float current_health;

    private Vector3 initialPosition;
    private bool moveForward = true;

    public float moveDistance = 10f;
    public float moveSpeed = 200f;

    public float coolTime = 1f;
    private float lastDamageTime = 0f; // 마지막 데미지 처리 시간 저장용 변수

    private void Awake()
    {
        pointspwan = FindObjectOfType<PointSpwan>();
        // 현재 씬의 이름을 가져옵니다.
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        // 현재 씬의 이름이 "Map"인 경우에만 PDamage 스크립트를 활성화합니다.
        if (currentScene == "Map")
        {
            enabled = true;
        }
        else
        {
            enabled = false;
        }
    }

    void Start()
    {
        gameObject.layer = 11;
        current_health = max_health;
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

    [PunRPC]
    public void PointUp(int p_damage)
    {
        current_health -= p_damage;

        if (current_health <= 0)
        {
            if (photonView != null && photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);

                pointspwan.isTargetDestroyed = true;
            }
            else
            {
                Debug.Log("오브젝트 없음");
            }
        }
    }


    [PunRPC]
    public void PointUpWithFloat(float p_damage)
    {
        current_health -= (int)p_damage;

        if (current_health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }

    private void IncreaseScore()
    {
        if (PhotonNetwork.IsConnected && photonView != null && photonView.Owner != null)
        {
            int playerId = photonView.Owner.ActorNumber;

            PhotonNetwork.LocalPlayer.AddScore(1);

            FindObjectOfType<PointCount>()?.UpdateKillText();
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (currentScene == "Map")
        {
            if (stream.IsWriting)
            {
                // 로컬 플레이어의 데이터 전송
                stream.SendNext((int)current_health); // int로 형변환하여 전송
            }
            else
            {
                // 원격 플레이어의 데이터 수신
                current_health = (int)stream.ReceiveNext(); // int로 형변환하여 받아옴
            }
        }
    }


}
