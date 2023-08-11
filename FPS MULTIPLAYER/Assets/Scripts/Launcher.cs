using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Launcher : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public string joinRoomName;
    public string createRoomName;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 연결 성공!");
        base.OnConnectedToMaster();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom(createInput.text);
    }

    private void Connect()
    {
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRandom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom(string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("방 이름을 입력하세요");
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
    }

    private void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void OnClickCreateRoom()
    {
        createRoomName = createInput.text;

        if (string.IsNullOrEmpty(createRoomName))
        {
            Debug.LogWarning("만드려는 방의 이름을 입력하세요");
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("마스터 서버에 연결 중... 잠시만 기다려주세요...");
            return;
        }

        PhotonNetwork.CreateRoom(createRoomName, options, TypedLobby.Default);
    }

    public void OnClickJoinRoom()
    {
        string joinRoomName = joinInput.text;
        if (string.IsNullOrEmpty(joinRoomName))
        {
            Debug.LogWarning("입장하려는 방의 이름을 입력하세요");
            return;
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // Photon Server에 연결
            return;
        }

        if (PhotonNetwork.InLobby) // 로비에 들어왔는지 확인
        {
            RoomInfo[] roomList = PhotonNetwork.GetRoomList(); // 모든 방의 정보를 가져옴
            foreach (RoomInfo roomInfo in roomList)
            {
                if (roomInfo.Name == joinRoomName)
                { // joinInput에 입력한 이름과 같은 방이 있을 경우
                    PhotonNetwork.JoinRoom(joinRoomName); // 해당 방에 입장
                    return;
                }
            }
            Debug.LogWarning("입력한 이름의 방이 존재하지 않습니다"); // 해당 이름을 가진 방이 없을 경우
        }
        else
        {
            Debug.LogWarning("로비에 입장한 후에 방에 입장할 수 있습니다"); // 로비에 입장하지 않았을 경우
        }
    }



    private IEnumerator WaitForJoin(string roomName)
    {
        while (!PhotonNetwork.InRoom)
        {
            yield return null;
        }

        while (!PhotonNetwork.IsMasterClient)
        {
            yield return null;
        }

        PhotonNetwork.LoadLevel(1);
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        StartGame();
    }
}