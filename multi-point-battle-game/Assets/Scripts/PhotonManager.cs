using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;

public class PhotonManager : MonoBehaviourPunCallbacks // ~~PunCallbacks �߰�
{
    // ���� �Է�
    private readonly string version = "1.0f";
    // ����� ���̵� �Է�
    private string userid = "Player1";

    void Awake()
    {
        // ���� ���� �����鿡�� �ڵ����� ���� �ε�
        PhotonNetwork.AutomaticallySyncScene = true;

        // ���� ������ �������� ���� ���
        PhotonNetwork.GameVersion = version;

        // ���� ���̵� �Ҵ�
        PhotonNetwork.NickName = userid;

        // ���� ������ ��� Ƚ�� ����. �ʴ� 30ȸ
        Debug.Log(PhotonNetwork.SendRate);

        // ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� ������ ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("������ ����");
        Debug.Log($"����Ȯ�� ����(���� ������) = {PhotonNetwork.InLobby}"); // ����Ȯ�� ���� false�� ����
        PhotonNetwork.JoinLobby(); // �κ� ����
    }

    // �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"����Ȯ�� ����(���� ������) = {PhotonNetwork.InLobby}"); // ���� Ȯ�� ���� true�� ����
        PhotonNetwork.JoinRandomRoom(); // ������ ��ġ����ŷ ��� ����(������ �� �� ������ ����)
    }

    // ������ �� ������ �������� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"�ƹ����� �������� �ʾҽ��ϴ� {returnCode}:{message}");

        // �� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20; // �ִ� ������ ��
        ro.IsOpen = true; // ���� ���� ����
        ro.IsVisible = true; // ����, ����� (�� ��� ���� ����)

        // �� ����
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    // �� ������ �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log("�� ����");
        Debug.Log($"�� �̸� = {PhotonNetwork.CurrentRoom.Name}");
    }

    // �� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"�� ���� Ȯ�� = {PhotonNetwork.InRoom}"); // �� ���� Ȯ��(���н� false)
        Debug.Log($"������ ���� ���� = {PhotonNetwork.CurrentRoom.PlayerCount}");
        
        // �뿡 ������ ����� ���� Ȯ��
        foreach(var player in PhotonNetwork.CurrentRoom.Players) 
        {
            // �÷��̾� �г��Ӱ� ������ȣ �ҷ�����
            Debug.Log($"���� �̸�, ���� ������ȣ = {player.Value.NickName}, {player.Value.ActorNumber}");
        }
        /*
        // ĳ���� ������ġ ���� ����
        // ĳ���� ���� ������ �迭�� ����
        Transform[] points = GameObject.Find("SpawnPonitGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(0, points.Length);

        // ĳ���� ���� Instantiate(������ �̸�, ��ġ, ȸ��, �׷�)
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
        */
    }
}
