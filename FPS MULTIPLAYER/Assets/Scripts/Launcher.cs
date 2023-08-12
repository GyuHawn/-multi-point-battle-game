using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_InputField playerName;

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
        if(isReady)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 5;
            PhotonNetwork.CreateRoom(createInput.text, options);
        }
        else
        {
            Debug.Log("방을 만들 준비가 되지 않았습니다.");
        }
    }

    public void JoinRoom()
    {
        if(isReady)
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
        PhotonNetwork.LoadLevel("Map");
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
        if(!PhotonNetwork.InLobby)
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
    }

}