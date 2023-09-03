using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun.UtilityScripts;
using TMPro;

public class PointGameResult : MonoBehaviourPunCallbacks
{
    public TMP_Text gwinnerName;
    public TMP_Text gwinnerKill;
    public TMP_Text swinnerName;
    public TMP_Text swinnerKill;
    public TMP_Text bwinnerName;
    public TMP_Text bwinnerKill;

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Map")
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.GetScore() > 1)
                {
                    PlayerPrefs.SetString("PlayerData", GetPlayerData());
                    SceneManager.LoadScene("Result");
                    break;
                }
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

    public void OnClick()
    {
        if (PhotonNetwork.IsMasterClient) // 마스터 클라이언트인 경우에만 Scene 변경을 하도록 함
        {
            PhotonNetwork.LoadLevel("Menu"); // Menu 씬으로 전환
            PlayerPrefs.DeleteKey("PlayerData");
        }
    }
}
