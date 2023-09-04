using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine.SceneManagement;

public class PointGameResult : MonoBehaviourPunCallbacks
{
    public TMP_Text gwinnerName;
    public TMP_Text gwinnerKill;
    public TMP_Text swinnerName;
    public TMP_Text swinnerKill;
    public TMP_Text bwinnerName;
    public TMP_Text bwinnerKill;
    public TMP_Text lobbyText;

    private float countdownTimer = 5f;

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Map")
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.GetScore() > 5)
                {
                    PlayerPrefs.SetString("PlayerData", GetPlayerData());
                    SceneManager.LoadScene("Result");
                    break;
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Result")
        {
            countdownTimer -= Time.deltaTime; // 경과 시간 감소

            // 남은 시간 표시
            lobbyText.text = "Go to Lobby   " + Mathf.CeilToInt(countdownTimer).ToString();

            if (countdownTimer <= 0)
            {
                MoveLobby(); // 카운트다운 종료 후 LobbyCount 함수 호출
            }
        }
    }

    string GetPlayerData()
    {
        var data = new List<string>();
        foreach (var player in PhotonNetwork.PlayerList)
        {
            data.Add(player.NickName + ":" + player.GetScore());
        }

        return string.Join(",", data);
    }

    void Start()
    {
        // 이 부분은 이전 코드와 동일합니다.
        if (SceneManager.GetActiveScene().name == "Result")
        {
            var data = PlayerPrefs.GetString("PlayerData").Split(',');
            var scores = new List<(string name, int score)>();

            foreach (var entry in data)
            {
                var parts = entry.Split(':');
                scores.Add((parts[0], int.Parse(parts[1])));
            }

            scores.Sort((x, y) => y.score.CompareTo(x.score));

            if (scores.Count >= 1)
            {
                gwinnerName.text = scores[0].name;
                gwinnerKill.text = scores[0].score.ToString();
            }
            if (scores.Count >= 2)
            {
                swinnerName.text = scores[1].name;
                swinnerKill.text = scores[1].score.ToString();
            }
            if (scores.Count >= 3)
            {
                bwinnerName.text = scores[2].name;
                bwinnerKill.text = scores[2].score.ToString();
            }
        }
    }

    public void MoveLobby()
    {
        if (PhotonNetwork.IsMasterClient) // 마스터 클라이언트인 경우에만 Scene 변경을 하도록 함
        {
            PlayerPrefs.DeleteKey("PlayerData");

            // 모든 플레이어의 점수 초기화
            foreach (var player in PhotonNetwork.PlayerList)
            {
                player.SetScore(0);
            }

            PhotonNetwork.LoadLevel("World"); // Menu 씬으로 전환  
        }
    }
}
