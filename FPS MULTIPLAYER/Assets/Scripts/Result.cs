using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class Result : MonoBehaviour
{
    public TMP_Text gPlayer;
    public TMP_Text gPlayerKill;
    public TMP_Text sPlayer;
    public TMP_Text sPlayerKill;
    public TMP_Text bPlayer;
    public TMP_Text bPlayerKill;

    public void OnClick()
    {
        if (PhotonNetwork.IsMasterClient) // ������ Ŭ���̾�Ʈ�� ��쿡�� Scene ������ �ϵ��� ��
        {
            PhotonNetwork.LoadLevel("Menu"); // Menu ������ ��ȯ
        }
    }
}