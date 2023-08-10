using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;

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
        string roomName = createInput.text;

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("방 이름을 입력하세요");
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("마스터 서버에 연결 중... 잠시만 기다려주세요...");
            return;
        }

        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
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
// 룸이름으로 방입장 가능 확인