using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointCount : MonoBehaviourPunCallbacks
{
    public Text killText;

    private int score = 0; // 표적 점수

    void Start()
    {
        if (PhotonNetwork.IsConnected) // 현재 네트워크에 연결되어 있는지 확인
        {
            // 로컬 플레이어의 점수를 가져옴
            score = PhotonNetwork.LocalPlayer.GetScore();
        }

        UpdateKillText(); // 초기화된 점수로 텍스트 업데이트
    }

    public void UpdateKillText()
    {
        killText.text = score.ToString(); // 텍스트 업데이트
    }

}
