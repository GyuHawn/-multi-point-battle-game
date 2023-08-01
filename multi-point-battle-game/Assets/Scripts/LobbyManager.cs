using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerNameInputField;
    public TMP_InputField roomNameInputField;
    public Button joinRoomButton;

    void Start()
    {
        // Button click event 추가
        joinRoomButton.onClick.AddListener(JoinRoomByName);
    }

    void JoinRoomByName()
    {
        string roomName = roomNameInputField.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            // 기존 OnJoinedLobby() 메서드에서 PhotonNetwork.JoinRandomRoom() 부분을 수정한 메서드 호출
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
        }
        else
        {
            Debug.Log("룸 이름을 입력하십시오.");
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("룸 생성");
        Debug.Log($"룸 이름 = {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"룸 입장 확인 = {PhotonNetwork.InRoom}");
        Debug.Log($"접속한 유저 숫자 = {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.CurrentRoom.Name == roomNameInputField.text)
        {
            PhotonNetwork.LoadLevel("Room");
        }
    }

}





/*using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public PhotonManager ptmanager;
    private string version = "1.0f"; // 게임버전

    public TMP_InputField roomName;
    public Button button;

    private string enterRoom;

    // 게임 실행과 동시에 마스터 서버 접속 시도
    void Start()
    {
        // 접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.GameVersion = version;

        // 설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        // 룸 접속 버튼 잠시 비활성화
        button.interactable = false;
    }

    // 마스터 서버 접속 성공 시 자동 실행
    public override void OnConnectedToMaster()
    {
        // 로비에 접속
        PhotonNetwork.JoinLobby();

        // 룸 접속 버튼 활성화
        button.interactable = true;
    }

    // 마스터 서버 접속 실패 시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 룸 접속 버튼 잠시 비활성화
        button.interactable = false;

        // 마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 접속 시도
    public void Connect()
    {
        // 중복 접속 시도를 막기 위해 룸 접속 버튼 잠시 비활성화
        button.interactable = false;

        // 인풋필드에서 입력한 텍스트를 확인하여 "AAA"와 비교해서 같다면 진행
        if (roomName.text == "MyRoom")
        {
            // 마스터 서버에 접속 중이라면
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinOrCreateRoom(roomName.text, new RoomOptions { MaxPlayers = 20 }, null);
            }
            else
            {
                // 마스터 서버에 접속 중이 아니라면 재접속
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("방이 없습니다)");
            button.interactable = true;
        }
    }

    // 룸 접속에 실패한 경우 자동 실행
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 최대 20명을 수용 가능한 빈방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 });
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        // 모든 룸 참가자가 Room 씬을 로드하게 함
        PhotonNetwork.LoadLevel("Room");
    }

}
*/