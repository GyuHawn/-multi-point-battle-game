using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0f";
    public TMP_InputField playerNameInputField;
    public TMP_InputField roomNameInputField;
    public Button joinButton;

    void Start()
    {
        // ���� ���� ����
        PhotonNetwork.GameVersion = gameVersion;

        // �������� �̿��ؼ� PhotonNetwork ����
        PhotonNetwork.ConnectUsingSettings();

        // �ʱ⿡�� ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;

        // ����� �޽��� ���
        Debug.Log("������ ������ ������...");

        // Button click event �߰�
        joinButton.onClick.AddListener(Connect);
    }

    public override void OnConnectedToMaster()
    {
        // ����Ǹ� ���� ��ư Ȱ��ȭ
        joinButton.interactable = true;
        Debug.Log("������ ������ ����");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // ������ ����� �ٽ� ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;
        Debug.Log("������ ������ ���� ���� / ������ ��");
    }

    public void Connect()
    {
        // ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;

        // ���� �� �̸��� �Էµ� �� �̸��� ������ ���� �õ�
        if (PhotonNetwork.CurrentRoom.Name == roomNameInputField.text)
        {
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("������ ������ ����");

                PhotonNetwork.LoadLevel("Room");
            }
            else
            {
                // ������ ������ ����
                PhotonNetwork.ConnectUsingSettings();
                Debug.Log("������ ������ ���� ��...");
            }
        }
        else
        {
            Debug.Log("���� ���Դϴ�");
        }
    }

    /*public override void OnJoinedRoom()
    {
        // �濡 ���εǸ� Room ������ �̵�
        Debug.Log("�濡 ���εǾ����ϴ�.");
        PhotonNetwork.LoadLevel("Room");
    }*/
}
