using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class PointGameResult : MonoBehaviourPunCallbacks
{
    private PlayerMovement playerMove;
    private Weapon weapon;
    private PointGameStart pointGameStart;

    public Button lobbyButton;

    public TMP_Text gwinnerName;
    public TMP_Text gwinnerKill;
    public TMP_Text swinnerName;
    public TMP_Text swinnerKill;
    public TMP_Text bwinnerName;
    public TMP_Text bwinnerKill;

    private float countdownTimer = 5f;

    void Start()
    {
        playerMove = FindObjectOfType<PlayerMovement>();
        weapon = FindObjectOfType<Weapon>();
        pointGameStart = FindObjectOfType<PointGameStart>();

        if (SceneManager.GetActiveScene().name == "Result")
        {
            lobbyButton.onClick.AddListener(MoveLobby);
        }

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

    void Update()
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

    string GetPlayerData()
    {
        var data = new List<string>();
        foreach (var player in PhotonNetwork.PlayerList)
        {
            data.Add(player.NickName + ":" + player.GetScore());
        }

        return string.Join(",", data);
    }

    public void MoveLobby()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerPrefs.DeleteKey("PlayerData");

            foreach (var player in PhotonNetwork.PlayerList)
                player.SetScore(0);

            //PhotonNetwork.LoadLevel("World");  

            Launcher launcher = FindObjectOfType<Launcher>();  // Launcher 컴포넌트 찾기

            if (launcher != null)
            {
                string originalRoomName = launcher.originalRoomName;  // 원래의 방 이름 가져오기

                if (!string.IsNullOrEmpty(originalRoomName))  // 만약 원래의 방 이름이 유효하다면...
                    PhotonNetwork.JoinOrCreateRoom(originalRoomName, new RoomOptions { MaxPlayers = 5 }, TypedLobby.Default);  // 해당 이름으로 룸에 접속하거나 새로 생성하기

                pointGameStart.pointGamemanager.gameObject.SetActive(false);
                playerMove.isPointGame = false;
                weapon.isWeapon = false;
                SceneManager.LoadScene("World");
            }
        }
    }
}
