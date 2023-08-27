using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointCount : MonoBehaviourPunCallbacks
{
    public Text killText;

    private int score = 0; // ǥ�� ����

    void Start()
    {
        if (PhotonNetwork.IsConnected) // ���� ��Ʈ��ũ�� ����Ǿ� �ִ��� Ȯ��
        {
            // ���� �÷��̾��� ������ ������
            score = PhotonNetwork.LocalPlayer.GetScore();
        }

        UpdateKillText(); // �ʱ�ȭ�� ������ �ؽ�Ʈ ������Ʈ
    }

    public void UpdateKillText()
    {
        killText.text = score.ToString(); // �ؽ�Ʈ ������Ʈ
    }

}
