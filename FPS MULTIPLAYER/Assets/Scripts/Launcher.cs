using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    public string originalRoomName;

    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_InputField playerName;
    public Text RoomList;
    private List<RoomInfo> roomInfos = new List<RoomInfo>();

    private bool isReady = false;

    private void Start()
    {
        playerName.characterLimit = 10;
        createInput.characterLimit = 10;

        // 서버 연결 시도
        PhotonNetwork.Disconnect(); // 먼저 연결을 끊고
        PhotonNetwork.ConnectUsingSettings(); // 다시 연결을 시도합니다.
    }

    public void CreateRoom()
    {
        if (isReady)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 5;
            originalRoomName = createInput.text;  // 생성한 방의 이름 저장
            PhotonNetwork.CreateRoom(originalRoomName, options);
        }
        else
        {
            Debug.Log("방을 만들 준비가 되지 않았습니다.");
        }
    }

    public void JoinRoom()
    {
        if (isReady)
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
        else
        {
            Debug.Log("입장할 방이 없습니다");
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("World");
        //PhotonNetwork.LoadLevel("Map");
        //PhotonNetwork.LoadLevel("Map2");
        if (playerName.text != "")
        {
            PhotonNetwork.NickName = playerName.text;
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 100);
        }
    }

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        isReady = true;
    }

    public override void OnLeftLobby()
    {
        isReady = false;
        RoomList.text = "";
        roomInfos.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomInfos = roomList;
        RefreshRoomList();
    }

    private void RefreshRoomList()
    {
        string roomString = "";
        for (int i = 0; i < roomInfos.Count; i++) // List 사용
        {
            roomString += i + 1 + " : " + roomInfos[i].Name + " [ " + roomInfos[i].PlayerCount + " / " + roomInfos[i].MaxPlayers + " ]" + "\n";
        }
        RoomList.text = roomString;
    }
}