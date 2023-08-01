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
        // Button click event �߰�
        joinRoomButton.onClick.AddListener(JoinRoomByName);
    }

    void JoinRoomByName()
    {
        string roomName = roomNameInputField.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            // ���� OnJoinedLobby() �޼��忡�� PhotonNetwork.JoinRandomRoom() �κ��� ������ �޼��� ȣ��
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
        }
        else
        {
            Debug.Log("�� �̸��� �Է��Ͻʽÿ�.");
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("�� ����");
        Debug.Log($"�� �̸� = {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"�� ���� Ȯ�� = {PhotonNetwork.InRoom}");
        Debug.Log($"������ ���� ���� = {PhotonNetwork.CurrentRoom.PlayerCount}");

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
    private string version = "1.0f"; // ���ӹ���

    public TMP_InputField roomName;
    public Button button;

    private string enterRoom;

    // ���� ����� ���ÿ� ������ ���� ���� �õ�
    void Start()
    {
        // ���ӿ� �ʿ��� ����(���� ����) ����
        PhotonNetwork.GameVersion = version;

        // ������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

        // �� ���� ��ư ��� ��Ȱ��ȭ
        button.interactable = false;
    }

    // ������ ���� ���� ���� �� �ڵ� ����
    public override void OnConnectedToMaster()
    {
        // �κ� ����
        PhotonNetwork.JoinLobby();

        // �� ���� ��ư Ȱ��ȭ
        button.interactable = true;
    }

    // ������ ���� ���� ���� �� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        // �� ���� ��ư ��� ��Ȱ��ȭ
        button.interactable = false;

        // ������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // �� ���� �õ�
    public void Connect()
    {
        // �ߺ� ���� �õ��� ���� ���� �� ���� ��ư ��� ��Ȱ��ȭ
        button.interactable = false;

        // ��ǲ�ʵ忡�� �Է��� �ؽ�Ʈ�� Ȯ���Ͽ� "AAA"�� ���ؼ� ���ٸ� ����
        if (roomName.text == "MyRoom")
        {
            // ������ ������ ���� ���̶��
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinOrCreateRoom(roomName.text, new RoomOptions { MaxPlayers = 20 }, null);
            }
            else
            {
                // ������ ������ ���� ���� �ƴ϶�� ������
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("���� �����ϴ�)");
            button.interactable = true;
        }
    }

    // �� ���ӿ� ������ ��� �ڵ� ����
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // �ִ� 20���� ���� ������ ��� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 });
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        // ��� �� �����ڰ� Room ���� �ε��ϰ� ��
        PhotonNetwork.LoadLevel("Room");
    }

}
*/