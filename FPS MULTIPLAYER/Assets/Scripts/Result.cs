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
        if (PhotonNetwork.IsMasterClient) // 마스터 클라이언트인 경우에만 Scene 변경을 하도록 함
        {
            PhotonNetwork.LoadLevel("Menu"); // Menu 씬으로 전환
        }
    }
}