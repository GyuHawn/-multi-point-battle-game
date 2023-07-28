using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;

public class PhotonManager : MonoBehaviourPunCallbacks // ~~PunCallbacks 추가
{
    // 버전 입력
    private readonly string version = "1.0f";
    // 사용자 아이디 입력
    private string userid = "Player1";

    void Awake()
    {
        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;

        // 유저 아이디 할당
        PhotonNetwork.NickName = userid;

        // 포톤 서버와 통신 횟수 설정. 초당 30회
        Debug.Log(PhotonNetwork.SendRate);

        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 연결");
        Debug.Log($"입장확인 현재(현재 입장전) = {PhotonNetwork.InLobby}"); // 입장확인 현재 false가 정상
        PhotonNetwork.JoinLobby(); // 로비 입장
    }

    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"입장확인 현재(현재 입장후) = {PhotonNetwork.InLobby}"); // 입장 확인 현재 true가 정상
        PhotonNetwork.JoinRandomRoom(); // 생성한 매치메이킹 기능 제공(생성된 룸 중 무작위 입장)
    }

    // 랜덤한 룸 입장이 실패했을 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"아무룸이 생성되지 않았습니다 {returnCode}:{message}");

        // 룸 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20; // 최대 접속자 수
        ro.IsOpen = true; // 룸의 오픈 여부
        ro.IsVisible = true; // 공개, 비공개 (룸 목록 노출 여부)

        // 룸 생성
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    // 룸 생성이 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("룸 생성");
        Debug.Log($"룸 이름 = {PhotonNetwork.CurrentRoom.Name}");
    }

    // 룸 입장 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"룸 입장 확인 = {PhotonNetwork.InRoom}"); // 룸 입장 확인(실패시 false)
        Debug.Log($"접속한 유저 숫자 = {PhotonNetwork.CurrentRoom.PlayerCount}");
        
        // 룸에 접속한 사용자 정보 확인
        foreach(var player in PhotonNetwork.CurrentRoom.Players) 
        {
            // 플레이어 닉네임과 고유번호 불러오기
            Debug.Log($"유저 이름, 유저 고유번호 = {player.Value.NickName}, {player.Value.ActorNumber}");
        }
        /*
        // 캐릭터 생성위치 랜덤 설정
        // 캐릭터 출현 정보를 배열에 저장
        Transform[] points = GameObject.Find("SpawnPonitGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(0, points.Length);

        // 캐릭터 생성 Instantiate(프리팹 이름, 위치, 회전, 그룹)
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
        */
    }
}
