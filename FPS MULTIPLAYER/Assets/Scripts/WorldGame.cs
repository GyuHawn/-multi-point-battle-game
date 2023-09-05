using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class WorldGame : MonoBehaviourPunCallbacks
{
    public GameObject Press_Key;
    public GameObject pointGame;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject == pointGame)
        {
            Press_Key.SetActive(true);

            if (pointGame.name == "PointGamePortal")
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // 현재 플레이어 수 확인
                    int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

                    // 최대 플레이어 수가 아직 안 찼으면 같은 룸에 입장
                    if (playerCount < 4)
                        SceneManager.LoadScene("Map");
                    else
                        CreateNewRoomAndJoin(); // 새로운 룸 생성 후 입장

                    // 현재 마스터 클라이언트의 플레이어 오브젝트 삭제
                    if (PhotonNetwork.IsMasterClient)
                        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                }
            }
            else if (pointGame.name == "DodgeGamePortal")
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // 현재 플레이어 수 확인
                    int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

                    // 최대 플레이어 수가 아직 안 찼으면 같은 룸에 입장
                    if (playerCount < 4)
                        SceneManager.LoadScene("Map2");
                    else
                        CreateNewRoomAndJoin(); // 새로운 룸 생성 후 입장

                    // 현재 마스터 클라이언트의 플레이어 오브젝트 삭제
                    if (PhotonNetwork.IsMasterClient)
                        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Press_Key.SetActive(false);
        }
    }

    private void CreateNewRoomAndJoin()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4; // 최대 플레이어 수 설정

        string newRoomName = "Room" + Random.Range(0, 1000); // 임의의 방 이름 생성

        PhotonNetwork.CreateRoom(newRoomName, options); // 새로운 방 생성

        Debug.Log("Created a new room: " + newRoomName);
    }

}
