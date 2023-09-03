using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PointCount : MonoBehaviourPunCallbacks
{
    public Text killText;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Map")
        {
            GameObject obj = GameObject.Find("KillCount");
            killText = obj?.GetComponent<Text>();
        }
    }


    private void OnDestroy()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
        }
    }

    private void OnEventReceived(EventData photonEvent)
    {
        if (photonEvent.Code == 1)
        {
            object[] data = (object[])photonEvent.CustomData;
            int playerId = (int)data[0];
            int scoreIncrease = (int)data[1];

            if (playerId == PhotonNetwork.LocalPlayer.ActorNumber && SceneManager.GetActiveScene().name == "Map")
            {
                PhotonNetwork.LocalPlayer.AddScore(scoreIncrease);
                UpdateKillTextRPC(PhotonNetwork.LocalPlayer.GetScore());
            }
        }
    }

    [PunRPC]
    public void DecreaseKillTextRPC()
    {
        if (SceneManager.GetActiveScene().name == "Map")
        {
            int score = int.Parse(killText.text);
            score = Mathf.Max(0, score - 1);
            killText.text = score.ToString();
        }
    }

    [PunRPC]
    public void UpdateKillTextRPC(int score)
    {
        if (SceneManager.GetActiveScene().name == "Map")
        {
            killText.text = score.ToString();
        }
    }
}
