using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0f";
    public TMP_InputField playerNameInputField;
    public TMP_InputField roomNameInputField;
    public Button joinButton;

    void Start()
    {
        // 게임 버전 설정
        PhotonNetwork.GameVersion = gameVersion;

        // 설정값을 이용해서 PhotonNetwork 연결
        PhotonNetwork.ConnectUsingSettings();

        // 초기에는 조인 버튼 비활성화
        joinButton.interactable = false;

        // 디버그 메시지 출력
        Debug.Log("마스터 서버에 접속중...");

        // Button click event 추가
        joinButton.onClick.AddListener(Connect);
    }

    public override void OnConnectedToMaster()
    {
        // 연결되면 조인 버튼 활성화
        joinButton.interactable = true;
        Debug.Log("마스터 서버에 접속");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 연결이 끊기면 다시 조인 버튼 비활성화
        joinButton.interactable = false;
        Debug.Log("마스터 서버에 접속 끊김 / 재접속 중");
    }

    public void Connect()
    {
        // 조인 버튼 비활성화
        joinButton.interactable = false;

        // 현재 방 이름과 입력된 방 이름이 같으면 조인 시도
        if (PhotonNetwork.CurrentRoom.Name == roomNameInputField.text)
        {
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("마스터 서버에 연결");

                PhotonNetwork.LoadLevel("Room");
            }
            else
            {
                // 마스터 서버에 연결
                PhotonNetwork.ConnectUsingSettings();
                Debug.Log("마스터 서버에 연결 중...");
            }
        }
        else
        {
            Debug.Log("없는 방입니다");
        }
    }

    /*public override void OnJoinedRoom()
    {
        // 방에 조인되면 Room 씬으로 이동
        Debug.Log("방에 조인되었습니다.");
        PhotonNetwork.LoadLevel("Room");
    }*/
}
