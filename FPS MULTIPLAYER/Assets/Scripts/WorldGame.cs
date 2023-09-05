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
                    // ���� �÷��̾� �� Ȯ��
                    int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

                    // �ִ� �÷��̾� ���� ���� �� á���� ���� �뿡 ����
                    if (playerCount < 4)
                        SceneManager.LoadScene("Map");
                    else
                        CreateNewRoomAndJoin(); // ���ο� �� ���� �� ����

                    // ���� ������ Ŭ���̾�Ʈ�� �÷��̾� ������Ʈ ����
                    if (PhotonNetwork.IsMasterClient)
                        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                }
            }
            else if (pointGame.name == "DodgeGamePortal")
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // ���� �÷��̾� �� Ȯ��
                    int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

                    // �ִ� �÷��̾� ���� ���� �� á���� ���� �뿡 ����
                    if (playerCount < 4)
                        SceneManager.LoadScene("Map2");
                    else
                        CreateNewRoomAndJoin(); // ���ο� �� ���� �� ����

                    // ���� ������ Ŭ���̾�Ʈ�� �÷��̾� ������Ʈ ����
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
        options.MaxPlayers = 4; // �ִ� �÷��̾� �� ����

        string newRoomName = "Room" + Random.Range(0, 1000); // ������ �� �̸� ����

        PhotonNetwork.CreateRoom(newRoomName, options); // ���ο� �� ����

        Debug.Log("Created a new room: " + newRoomName);
    }

}
