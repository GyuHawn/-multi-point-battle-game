using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Launcher : MonoBehaviourPunCallbacks
{
    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // 새로운 플레이어가 방에 들어왔을 때 씬을 자동으로 동기화 여부
        Connect();
    }
    public override void OnConnectedToMaster() // 마스터 서버에 연결시 실행
    {
        Debug.Log("연결완료!");
        Join();
        base.OnConnectedToMaster();
    }

    public override void OnJoinedRoom() // 플레이어가 방에 참가시 실행
    {
        StartGame();
        base.OnJoinedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // 방에 참가 실패시 실행
    {
        Create();
        base.OnJoinRandomFailed(returnCode, message);
    }
    public void Connect()
    {
        Debug.Log("연결 시도 중");
        PhotonNetwork.GameVersion = "0.0.0"; // 게임 버전
        PhotonNetwork.ConnectUsingSettings(); // PhotonNetwork 설정값에 따라 연결
    }
      
   public void Join()
   {
        PhotonNetwork.JoinRandomRoom(); // 무작위 방에 입장
   }

    public void Create()
    {
        PhotonNetwork.CreateRoom(""); // 자동으로 방의 이름 설정 
    }

   public void StartGame()
   {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1) // 플레이어 수가 1명일때
        {
            PhotonNetwork.LoadLevel(1); // 씬 로드
        }
   } 
}

   

